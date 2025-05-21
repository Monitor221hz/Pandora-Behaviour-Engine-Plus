using Pandora.API.Patch;
using Pandora.Core;
using Pandora.Models.Patch.Skyrim64;
using Pandora.Models.Patch.Skyrim64.Format.FNIS;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Pandora.Patch.Patchers.Skyrim.FNIS;

public partial class FNISAnimationList
{
	private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

	[GeneratedRegex("^([^('|\\s)]+)\\s*(?:-(\\S+)*)?\\s*(\\S+)\\s+(\\S+.hkx)(?:[^\\S\\r\\n]+(\\S+))*", RegexOptions.Compiled)]
	private static partial Regex FNISAnimLineRegex();

	private static readonly Regex animLineRegex = FNISAnimLineRegex();

	private static readonly Dictionary<string, string> linkedCharacterNameMap = new Dictionary<string, string>()
	{
		{"defaultmale", "defaultfemale" },
		{"defaultfemale", "defaultmale" },
		{"draugrskeletonproject", "draugrproject" },
		{"draugrproject", "draugrskeletonproject" },
		{"wolfproject", "dogproject" },
		{"dogproject", "wolfproject" }
	};


	public List<BasicAnimation> Animations { get; private set; } = new(); 
	public IModInfo ModInfo { get; private set; }

	private FNISAnimationList(IModInfo modInfo)
	{
		ModInfo = modInfo;	
	}
	public static FNISAnimationList FromFile(FileInfo file)
	{
		FNISModInfo modInfo = new FNISModInfo(file);
		var animlist = new FNISAnimationList(modInfo);
		if (file.Directory== null || file.Directory.Parent == null) { return animlist; }
		string animRoot = Path.Combine(file.Directory.Parent.Name, file.Directory.Name);
		using (var readStream  = File.OpenRead(file.FullName))
		{
			using (var reader = new StreamReader(readStream))
			{
				FNISAnimationFactory factory = new(); 
				string? expectedLine; 
				while((expectedLine = reader.ReadLine()) != null)
				{
					if (string.IsNullOrWhiteSpace(expectedLine) || expectedLine[0] == '\'')
					{
						continue; 
					}
					if (factory.CreateFromLine(animRoot, expectedLine, out var animation))
					{
						animlist.Animations.Add(animation);
					}
					//else
					//{
					//	logger.Warn($"FNIS Animlist > New Animation > From Line > FAILED > String > \"{expectedLine}\" > File > {file.Name}");
					//}
				}
			}
		}

		return animlist; 
	}

	public bool BuildPatches(Project project, ProjectManager projectManager)
	{
		FNISAnimationListBuildContext buildContext = new FNISAnimationListBuildContext(project, projectManager, ModInfo); 
		foreach (BasicAnimation animation in Animations)
		{
			animation.BuildPatch(buildContext);
		}
		return true;
	}
}
