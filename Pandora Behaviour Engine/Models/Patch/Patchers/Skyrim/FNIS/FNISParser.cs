using HKX2E;
using Pandora.API.Patch;
using Pandora.Core;
using Pandora.Core.Patchers.Skyrim;
using Pandora.Patch.Patchers.Skyrim.Hkx;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Pandora.Patch.Patchers.Skyrim.FNIS;
public class FNISParser
{
	private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

	private static readonly HashSet<string> animTypePrefixes = new HashSet<string>() { "b", "s", "so", "fu", "fuo", "+", "ofa", "pa", "km", "aa", "ch" };

	private static readonly Regex hkxRegex = new Regex("\\S*\\.hkx", RegexOptions.IgnoreCase);

	private static readonly Regex animLineRegex = new Regex(@"^([^('|\s)]+)\s*(-\S+)*\s*(\S+)\s+(\S+.hkx)(?:[^\S\r\n]+(\S+))*", RegexOptions.Compiled);

	private static readonly Dictionary<string, string> stateMachineMap = new Dictionary<string, string>()
	{
		{"atronachflame~atronachflamebehavior", "#0414" },
		{"atronachfrostproject~atronachfrostbehavior", "#0439" },
		{"atronachstormproject~atronachstormbehavior", "#0369" },
		{"bearproject~bearbehavior", "#0151" },
		{"chaurusproject~chaurusbehavior", "#0509" },
		{"dogproject~dogbehavior", "#0144" },
		{"wolfproject~wolfbehavior", "#0169" },
		{"defaultfemale~0_master", "#0340" },
		{"defaultmale~0_master", "#0340" },
		{"firstperson~0_master", "#0167" },
		{"spherecenturion~scbehavior", "#0780" },
		{"dwarvenspidercenturionproject~dwarvenspiderbehavior", "#0394" },
		{"steamproject~steambehavior", "#0538" },
		{"falmerproject~falmerbehavior", "#1294" },
		{"frostbitespiderproject~frostbitespiderbehavior", "#0402" },
		{"giantproject~giantbehavior", "#0795" },
		{"goatproject~goatbehavior", "#0140" },
		{"highlandcowproject~h-cowbehavior", "#0152" },
		{"deerproject~deerbehavior", "#0145" },
		{"dragonproject~dragonbehavior", "#1610" },
		{"dragon_priest~dragon_priest", "#0758" },
		{"draugrproject~draugrbehavior", "#2026" },
		{"draugrskeletonproject~draugrbehavior", "#2026" },
		{"hagravenproject~havgravenbehavior", "#0634" },
		{"horkerproject~horkerbehavior", "#0161" },
		{"horseproject~horsebehavior", "#0493" },
		{"icewraithproject~icewraithbehavior", "#0262" },
		{"mammothproject~mammothbehavior", "#0155" },
		{"mudcrabproject~mudcrabbehavior", "#0481" },
		{"sabrecatproject~sabrecatbehavior", "#0140" },
		{"skeeverproject~skeeverbehavior", "#0132" },
		{"slaughterfishproject~slaughterfishbehavior", "#0128" },
		{"spriggan~sprigganbehavior", "#0610" },
		{"trollproject~trollbehavior", "#0708" },
		{"vampirelord~vampirelord", "#1114" },
		{"werewolfbeastproject~werewolfbehavior", "#1207" },
		{"wispproject~wispbehavior", "#0410" },
		{"witchlightproject~witchlightbehavior", "#0152" },
		{"chickenproject~chickenbehavior", "#0322" },
		{"hareproject~harebehavior", "#0297" },
		{"chaurusflyer~chaurusflyerbehavior", "#0402" },
		{"vampirebruteproject~vampirebrutebehavior", "#0502" },
		{"benthiclurkerproject~benthiclurkerbehavior", "#0708" },
		{"boarproject~boarbehavior", "#0584" },
		{"ballistacenturion~bcbehavior", "#0475" },
		{"hmdaedra~hmdaedra", "#0490" },
		{"netchproject~netchbehavior", "#0279" },
		{"rieklingproject~rieklingbehavior", "#0730" },
		{"scribproject~scribbehavior", "#0578" }
	};

	private static readonly Dictionary<string, string> animListExcludeMap = new Dictionary<string, string>()
	{
		{"dogproject", "wolf" },
		{"wolfproject", "dog" }
	};
	private readonly HashSet<PackFile> parsedBehaviorFiles = new(); 
	private readonly HashSet<Project> skipAnimlistProjects = new();
	private DirectoryInfo outputDirectory;
	public HashSet<IModInfo> ModInfos { get; private set; } = new HashSet<IModInfo>();

    public FNISParser(ProjectManager manager, DirectoryInfo outputPath)
    {
		outputDirectory = outputPath;
		projectManager = manager;
    }
	private ProjectManager projectManager;
	private PatchNodeCreator patchNodeCreator = new("fnis");
	public void ScanProjectBehaviors(Project project, DirectoryInfo absoluteOutputDirectory)
	{
		lock (parsedBehaviorFiles)
		{
			if (!parsedBehaviorFiles.Add(project.BehaviorFile))
			{
				return;
			}
		}

		var behaviorFolder = new DirectoryInfo(Path.Join(absoluteOutputDirectory.FullName, project.BehaviorFile.InputHandle.Directory!.Name));
		if (!behaviorFolder.Exists)
		{
			return;
		}
		var modFiles = behaviorFolder.GetFiles("FNIS*.hkx");

		if (modFiles.Length > 0) { projectManager.TryActivatePackFile(project.BehaviorFile); }

		foreach (var modFile in modFiles)
		{
			try
			{
				InjectGraphReference(modFile, project.BehaviorFile);
			}
			catch
			{
				logger.Warn($"FNIS Parser > Inject > Behavior > {modFile.Name} > FAILED");
			}
		}
	}
	public void ScanProjectAnimations(Project project, DirectoryInfo absoluteOutputDirectory)
	{
		lock (skipAnimlistProjects)
		{
			if (skipAnimlistProjects.Contains(project)) 
			{ 
				return; 
			}
			if (project.Sibling != null)
			{
				skipAnimlistProjects.Add(project.Sibling);
			}
		}
		var animationsFolder = new DirectoryInfo(Path.Join(absoluteOutputDirectory.FullName, "animations"));
		if (!animationsFolder.Exists) { return; }
		var modAnimationFolders = animationsFolder.GetDirectories();

		if (modAnimationFolders.Length == 0) { return; }
		Parallel.ForEach(modAnimationFolders, folder => { ParseAnimlistFolder(folder, project, projectManager); });
	}
    public void ScanProjectAnimlist(Project project)
	{
		var currentDirectory = new DirectoryInfo(Path.Join((BehaviourEngine.SkyrimGameDirectory ?? BehaviourEngine.AssemblyDirectory).FullName, project.ProjectFile.RelativeOutputDirectoryPath));

		ScanProjectBehaviors(project, currentDirectory);
		ScanProjectAnimations(project, currentDirectory);
	}
	private bool InjectGraphReference(FileInfo sourceFile, PackFileGraph destPackFile)
	{
		string stateFolderName;
		if (!stateMachineMap.TryGetValue(destPackFile.UniqueName, out stateFolderName!)) { return false; }
		projectManager.TryActivatePackFile(destPackFile); 
		string nameWithoutExtension = Path.GetFileNameWithoutExtension(sourceFile.Name);
		string graphPath = $"{destPackFile.InputHandle.Directory?.Name}\\{nameWithoutExtension}.hkx";
		hkbStateMachine rootState = destPackFile.GetPushedObjectAs<hkbStateMachine>(stateFolderName);
		hkbBehaviorReferenceGenerator refGenerator = new() { name = nameWithoutExtension, variableBindingSet = null, userData = 0, behaviorName = graphPath };
		hkbStateMachineStateInfo stateInfo = new() {  name = "PN_StateInfo", enable = true, probability=1.0f, stateId = (graphPath.GetHashCode() & 0xfffffff), generator=refGenerator };
		lock (rootState.states)
		{
			rootState.states.Add(stateInfo);
		}
		return true;
	}

	private void ParseAnimlistFolder(DirectoryInfo folder, Project project, ProjectManager projectManager)
	{
		var animlistFiles = folder.GetFiles("*list.txt");

		if (animListExcludeMap.TryGetValue(project.Identifier, out var excludeName))
		{
			animlistFiles = animlistFiles.Where(f => !f.Name.EndsWith(excludeName)).ToArray();
		}

		if (animlistFiles.Length == 0) { return; }

		List<FNISAnimationList> animLists = new();
		foreach (var animlistFile in animlistFiles)
		{
#if DEBUG
			FNISAnimationList animList = FNISAnimationList.FromFile(animlistFile);
			lock (ModInfos)
			{
				ModInfos.Add(animList.ModInfo);
			}
			animLists.Add(animList);
#else
			try
			{
				FNISAnimationList animList = FNISAnimationList.FromFile(animlistFile);
				lock (ModInfos)
				{
					ModInfos.Add(animList.ModInfo);
				}
				animLists.Add(animList);
			}
			catch (Exception ex)
			{
				logger.Warn($"FNIS Parser > Serialize > Animlist > {animlistFile.Name} > FAILED > {ex.ToString()}");
			}
#endif
		}
		if (animLists.Count > 1)
		{
			Parallel.ForEach(animLists, animlist => { animlist.BuildPatches(project, projectManager, patchNodeCreator); });
		}
		else if (animLists.Count > 0)
		{
			animLists[0].BuildPatches(project, projectManager, patchNodeCreator);
		}
	}
	public void SetOutputPath(DirectoryInfo outputPath)
	{
		outputDirectory = outputPath;
	}
}
