using Avalonia.Skia;
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
	private static readonly Dictionary<string, string[]> manualScanDirectories = new Dictionary<string, string[]>()
	{
		{"canine", ["wolf", "dog"]}, 

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

		var behaviorFolder = new DirectoryInfo(Path.Join(absoluteOutputDirectory.FullName, project.BehaviorFile.InputHandle.Directory!.Name));
		if (!behaviorFolder.Exists)
		{
			return;
		}
		if (modAnimationFolders.Length == 0) { return; }
		Parallel.ForEach(modAnimationFolders, folder => { ParseAnimlistFolder(folder, behaviorFolder, project, projectManager); });
	}
    public void ScanProjectAnimlist(Project project)
	{
		var currentDirectory = new DirectoryInfo(Path.Join((BehaviourEngine.SkyrimGameDirectory ?? outputDirectory ?? BehaviourEngine.CurrentDirectory ?? BehaviourEngine.AssemblyDirectory).FullName, project.ProjectFile.RelativeOutputDirectoryPath));

		ScanProjectAnimations(project, currentDirectory);
	}
	private bool InjectGraphReference(string listName, DirectoryInfo folder, DirectoryInfo behaviorFolder, PackFileGraph destPackFile)
	{
		string stateFolderName;
		FileInfo sourceFile = new(Path.Join(behaviorFolder.FullName, $"FNIS_{listName}_Behavior.hkx")); 
		if (!sourceFile.Exists) 
		{ 
			sourceFile = new(Path.Join(behaviorFolder.FullName, $"FNIS_{folder.Name}_{listName}_Behavior.hkx"));
			if (!sourceFile.Exists) 
			{
				logger.Warn($"FNIS Parser > Find > Animlist Behavior > {sourceFile.Name} > FAILED");
				return false; 
			}
		}	
		if (!stateMachineMap.TryGetValue(destPackFile.UniqueName, out stateFolderName!)) { return false; } //thread safe
		projectManager.TryActivatePackFile(destPackFile); 
		string nameWithoutExtension = Path.GetFileNameWithoutExtension(sourceFile.Name);
		string graphPath = $"{destPackFile.InputHandle.Directory?.Name}\\{nameWithoutExtension}.hkx";

		hkbBehaviorReferenceGenerator refGenerator = new() { name = nameWithoutExtension, variableBindingSet = null, userData = 0, behaviorName = graphPath };
		hkbStateMachineStateInfo stateInfo = new() {  name = "PN_StateInfo", enable = true, probability=1.0f, stateId = (graphPath.GetHashCode() & 0xfffffff), generator=refGenerator };
		hkbStateMachine rootState = destPackFile.GetPushedObjectAs<hkbStateMachine>(stateFolderName);
		lock (rootState)
		{
			rootState.states.Add(stateInfo);
		}
		return true;
	}

	private void ParseAnimlistFolder(DirectoryInfo folder, DirectoryInfo behaviorFolder, Project project, ProjectManager projectManager)
	{
		string listName = folder.Name; 
		FileInfo animListFile = new FileInfo(Path.Join(folder.FullName, $"FNIS_{folder.Name}_List.txt"));
		if (!animListFile.Exists) 
		{
			listName = behaviorFolder.Parent!.Name;
			animListFile = new FileInfo(Path.Join(folder.FullName, $"FNIS_{folder.Name}_{listName}_List.txt")); 
			if (!animListFile.Exists) 
			{
				if (behaviorFolder.Parent.Parent == null || !manualScanDirectories.TryGetValue(behaviorFolder.Parent.Name, out var scanNames)) { return; }
				foreach (string scanName in scanNames)
				{
					listName = scanName; 
					animListFile = animListFile = new FileInfo(Path.Join(folder.FullName, $"FNIS_{folder.Name}_{listName}_List.txt"));
					if (animListFile.Exists) 
					{ 
						break; 
					}
				}
				if (!animListFile.Exists) 
				{ 
					return; 
				}
			}
		}
		FNISAnimationList animList; 
#if DEBUG
		animList = FNISAnimationList.FromFile(animListFile);
#else
			try
			{
				animList = FNISAnimationList.FromFile(animListFile);
			}
			catch (Exception ex)
			{
				logger.Warn($"FNIS Parser > Serialize > Animlist > {animListFile.Name} > FAILED > {ex.ToString()}");
				return;
			}
#endif
		animList.BuildPatches(project, projectManager, patchNodeCreator);
		if (InjectGraphReference(listName, folder, behaviorFolder, project.BehaviorFile))
		{
			lock (ModInfos)
			{
				ModInfos.Add(animList.ModInfo);
			}
		}
	}
	public void SetOutputPath(DirectoryInfo outputPath)
	{
		outputDirectory = outputPath;
	}
}
