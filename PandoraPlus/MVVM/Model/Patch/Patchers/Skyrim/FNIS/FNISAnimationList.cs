using HKX2E;
using NLog;
using Pandora.Core;
using Pandora.Core.Patchers.Skyrim;
using Pandora.Patch.Patchers.Skyrim.Hkx;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Pandora.Patch.Patchers.Skyrim.FNIS;
public class FNISAnimationListBuildContext
{
	private Dictionary<string, hkbStringEventPayload> stringEventPayloadNameMap = new();
	private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
	public FNISAnimationListBuildContext(PatchNodeCreator helper, Project targetProject, ProjectManager projectManager)
	{
		Helper = helper;
		TargetProject = targetProject;
		ProjectManager = projectManager;
	}

	public PatchNodeCreator Helper { get; private set; }
	public Project TargetProject { get; private set; }
	public ProjectManager ProjectManager { get; private set; }

	public hkbStringEventPayload BuildCommonStringEventPayload(string name)
	{
		hkbStringEventPayload? payload;
		lock (stringEventPayloadNameMap)
		{
			if (stringEventPayloadNameMap.TryGetValue(name, out payload))
			{
				return payload;
			}
		}
		payload = new hkbStringEventPayload() {  data = name };
		lock (stringEventPayloadNameMap)
		{
			stringEventPayloadNameMap.Add(name, payload);
		}
		return payload;
	}
}

public partial class FNISAnimationList
{
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


	public List<IFNISAnimation> Animations { get; private set; } = new(); 
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
				string? expectedLine; 
				while((expectedLine = reader.ReadLine()) != null)
				{
					if (string.IsNullOrWhiteSpace(expectedLine) || expectedLine[0] == '\'')
					{
						continue; 
					}
					animlist.Animations.Add(FNISAnimationFactory.CreateFromLine(animRoot, expectedLine)); 
				}
			}
		}

		return animlist; 
	}

	public void BuildPatches(Project project, ProjectManager projectManager, PatchNodeCreator patchNodeCreator)
	{
		FNISAnimationListBuildContext buildContext = new FNISAnimationListBuildContext(patchNodeCreator, project, projectManager); 
		foreach (FNISAnimation animation in Animations)
		{
			animation.BuildPatch(buildContext);
		}
	}
}
