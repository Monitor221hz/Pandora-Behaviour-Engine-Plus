using Pandora.API.Patch;
using Pandora.Models.Patch.Mod;
using Pandora.Models.Patch.Skyrim64;
using Pandora.Models.Patch.Skyrim64.Format.FNIS;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Pandora.Patch.Patchers.Skyrim.FNIS;

public partial class FNISAnimationList
{
	private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

	[GeneratedRegex("^([^('|\\s)]+)\\s*(?:-(\\S+)*)?\\s*(\\S+)\\s+(\\S+.hkx)(?:[^\\S\\r\\n]+(\\S+))*", RegexOptions.Compiled)]
	private static partial Regex FNISAnimLineRegex();

	private static readonly Regex animLineRegex = FNISAnimLineRegex();

	private static readonly Dictionary<string, string> linkedCharacterNameMap = new()
	{
		{"defaultmale", "defaultfemale" },
		{"defaultfemale", "defaultmale" },
		{"draugrskeletonproject", "draugrproject" },
		{"draugrproject", "draugrskeletonproject" },
		{"wolfproject", "dogproject" },
		{"dogproject", "wolfproject" }
	};


	public List<BasicAnimation> Animations { get; private set; } = []; 
	public IModInfo ModInfo { get; private set; }

	private FNISAnimationList(IModInfo modInfo)
	{
		ModInfo = modInfo;	
	}
	public static FNISAnimationList FromFile(FileInfo file)
	{
		FNISModInfo modInfo = new(file);
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
	public void BuildAllAnimations(Project project, ProjectManager projectManager)
	{
		if (project.Sibling == null)
		{
			foreach (BasicAnimation animation in Animations)
			{
				lock (project.CharacterPackFile.uniqueAnimationLock) animation.BuildAnimation(project, projectManager);
			}
		}
		else
		{
			foreach (BasicAnimation animation in Animations)
			{
				lock(project.CharacterPackFile.uniqueAnimationLock)
				{
					animation.BuildAnimation(project, projectManager);
					animation.BuildAnimation(project.Sibling, projectManager);
				}
			}
		}

	}
	//public void BuildAllAnimations(Project project, Project sibling, ProjectManager projectManager)
	//{
	//	foreach (BasicAnimation animation in Animations)
	//	{		
	//		animation.BuildAnimation(project, projectManager);
	//		animation.BuildAnimation(sibling, projectManager);
	//	}
	//}
	//public void BuildAllAnimations(ProjectManager projectManager, params Project[] projects)
	//{
	//	foreach (BasicAnimation animation in Animations)
	//	{
	//		foreach(Project project in projects)
	//		{
	//			animation.BuildAnimation(project, projectManager);
	//		}
	//	}
	
	public void BuildAllBehaviorsParallel(Project project, ProjectManager projectManager)
	{
		FNISAnimationListBuildContext buildContext = new(project, projectManager, ModInfo);
		Parallel.ForEach(Animations, animation =>
		{
			animation.BuildBehavior(buildContext);
		});
	}
	public bool BuildAllBehaviors(Project project, ProjectManager projectManager)
	{
		FNISAnimationListBuildContext buildContext = new(project, projectManager, ModInfo); 
		foreach (BasicAnimation animation in Animations)
		{
			animation.BuildBehavior(buildContext);
		}
		return true;
	}
}