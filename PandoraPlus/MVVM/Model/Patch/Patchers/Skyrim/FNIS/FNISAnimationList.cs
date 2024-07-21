using HKX2;
using Pandora.Core;
using Pandora.Core.Patchers.Skyrim;
using Pandora.Patch.Patchers.Skyrim.Hkx;
using System;
using System.Collections.Generic;
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

	public FNISAnimationListBuildContext(PatchNodeCreator helper, Project targetProject, ProjectManager projectManager, PackFileTargetCache targetCache)
	{
		Helper = helper;
		TargetProject = targetProject;
		ProjectManager = projectManager;
		TargetCache = targetCache;
	}

	public PatchNodeCreator Helper { get; private set; }
	public Project TargetProject { get; private set; }
	public ProjectManager ProjectManager { get; private set; }
	public PackFileTargetCache TargetCache { get; private set; }

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
		payload = new hkbStringEventPayload() {  m_data = name };
		lock (stringEventPayloadNameMap)
		{
			stringEventPayloadNameMap.Add(name, payload);
		}
		return payload;
	}
}

public partial class FNISAnimationList
{
	[GeneratedRegex("^([^('|\\s)]+)\\s*(?:-(\\S+)*)?\\s*(\\S+)\\s+(\\S+.hkx)(?:[^\\S\\r\\n]+(\\S+))*", RegexOptions.Compiled | RegexOptions.Multiline)]
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
		var parentFolder = modInfo.Folder.Parent; 
		if (parentFolder != null)
		{
			modInfo.Name = Path.Combine(parentFolder.Name, modInfo.Folder.Name);
		}
		var animlist = new FNISAnimationList(modInfo);
		var matches = animLineRegex.Matches(File.ReadAllText(file.FullName));
		foreach(Match match in matches)
		{
			animlist.Animations.Add(FNISAnimationFactory.CreateFromMatch(match)); 
		}
		return animlist; 
	}

	public void BuildPatches(Project project, ProjectManager projectManager, PatchNodeCreator patchNodeCreator)
	{
		PackFileTargetCache targetCache = new(ModInfo, projectManager); 
		FNISAnimationListBuildContext buildContext = new FNISAnimationListBuildContext(patchNodeCreator, project, projectManager, targetCache); 
		foreach (FNISAnimation animation in Animations)
		{
			animation.BuildPatch(buildContext);
		}
		IEnumerable<PackFileTarget> targets = targetCache.Targets;
		foreach(PackFileTarget target in targets)
		{
			target.Build();
		}
	}
}
