using HKX2E;
using Pandora.API.Patch;
using Pandora.Models.Patch.Skyrim64.Hkx.Packfile;
using Pandora.Patch.Patchers.Skyrim.FNIS;
using System.Collections.Generic;
#pragma warning disable IDE0005
using System;
#pragma warning restore IDE0005
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;

namespace Pandora.Models.Patch.Skyrim64.Format.FNIS;

public class FNISParser
{
	private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

	private static readonly HashSet<string> animTypePrefixes = ["b", "s", "so", "fu", "fuo", "+", "ofa", "pa", "km", "aa", "ch"];

	private static readonly Regex hkxRegex = new("\\S*\\.hkx", RegexOptions.IgnoreCase);

	private static readonly Regex animLineRegex = new(@"^([^('|\s)]+)\s*(-\S+)*\s*(\S+)\s+(\S+.hkx)(?:[^\S\r\n]+(\S+))*", RegexOptions.Compiled);

	private static readonly Dictionary<string, string> stateMachineMap = new()
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

	private static readonly Dictionary<string, string> animListExcludeMap = new()
	{
		{"dogproject", "wolf" },
		{"wolfproject", "dog" }
	};
	private static readonly Dictionary<string, string[]> manualScanDirectories = new()
	{
		{"canine", ["wolf", "dog"]},
	};

	private static readonly HashSet<string> humanoidProjectIdentifiers = new(StringComparer.OrdinalIgnoreCase)
	{
		"defaultmale",
		"defaultfemale"
	};
	private readonly HashSet<PackFile> parsedBehaviorFiles = [];
	private readonly HashSet<Project> skipAnimlistProjects = [];
	private readonly Dictionary<string, FNISAnimationList> animListFileMap = new(StringComparer.OrdinalIgnoreCase);

	private readonly HashSet<string> parsedFiles = new(StringComparer.OrdinalIgnoreCase) { };
	private DirectoryInfo outputDirectory;
	public HashSet<IModInfo> ModInfos { get; private set; } = [];

	public FNISParser(ProjectManager manager, DirectoryInfo outputPath)
	{
		outputDirectory = outputPath;
		projectManager = manager;
	}
	private ProjectManager projectManager;

	private void ProcessFNISAnimationsHumanoid(DirectoryInfo modFolder, DirectoryInfo behaviorFolder, Project project, ProjectManager projectManager)
	{
		if (TryFindAnimlistHumanoid(modFolder, project, out var animListFile) && TryFindGraphHumanoid(modFolder, behaviorFolder, out var graphFile))
		{
			ParseAnimlist(animListFile, project, projectManager);
			InjectGraphReference(graphFile, project.BehaviorFile);
		}
		else
		{
			logger.Warn($"FNIS Parser > Scan FNIS Animations > {modFolder.Name} in {project.Identifier} > FAILED");
		}
	}
	private bool ProcessFNISAnimationsCreature(string creatureName, DirectoryInfo modFolder, DirectoryInfo behaviorFolder, Project project, ProjectManager projectManager)
	{
		if (TryFindAnimlistCreature(creatureName, modFolder, project, out var animListFile) && TryFindGraphCreature(creatureName, modFolder, behaviorFolder, out var graphFile))
		{
			ParseAnimlist(animListFile, project, projectManager);
			InjectGraphReference(graphFile, project.BehaviorFile);
			return true;
		}
		else
		{
			return false;
		}
	}
	public void ScanProjectAnimations(Project project, DirectoryInfo absoluteOutputDirectory)
	{
		//lock (skipAnimlistProjects)
		//{
		//	if (skipAnimlistProjects.Contains(project))
		//	{
		//		return;
		//	}
		//	if (project.Sibling != null)
		//	{
		//		skipAnimlistProjects.Add(project.Sibling);
		//	}
		//}
		var animationsFolder = new DirectoryInfo(Path.Join(absoluteOutputDirectory.FullName, "animations"));
		if (!animationsFolder.Exists) { return; }
		var modAnimationFolders = animationsFolder.GetDirectories();

		var behaviorFolder = new DirectoryInfo(Path.Join(absoluteOutputDirectory.FullName, project.BehaviorFile.InputHandle.Directory!.Name));
		if (!behaviorFolder.Exists || behaviorFolder.Parent == null)
		{
			return;
		}
		if (modAnimationFolders.Length == 0) { return; }

		bool isHumanoidProject = humanoidProjectIdentifiers.Contains(project.Identifier);
		if (isHumanoidProject) 
		{
			Parallel.ForEach(modAnimationFolders, folder =>
			{
				ProcessFNISAnimationsHumanoid(folder, behaviorFolder, project, projectManager);
			});
			return;
		}
		// creatures only
		var pseudoProjectIdentifier = behaviorFolder.Parent.Name; 
		//if (manualScanDirectories.TryGetValue(pseudoProjectIdentifier, out var scanNames))
		//{
		//	foreach (var scanName in scanNames)
		//	{
		//		Parallel.ForEach(modAnimationFolders, folder =>
		//		{
		//			ProcessFNISAnimationsCreature(scanName, folder, behaviorFolder, project, projectManager);
		//		});
		//	}
		//	return;
		//}
		Parallel.ForEach(modAnimationFolders, folder => 
		{
			if (!ProcessFNISAnimationsCreature(pseudoProjectIdentifier, folder, behaviorFolder, project, projectManager) && 
				!ProcessFNISAnimationsCreature(project.Identifier.Replace("project", string.Empty), folder, behaviorFolder, project, projectManager))
			{
				logger.Warn($"FNIS Parser > {nameof(ProcessFNISAnimationsCreature)} > {pseudoProjectIdentifier}/{folder.Name} in {project.Identifier} > FAILED");
			};
		});
	}
	public void ScanProjectAnimlist(Project project)
	{
		var currentDirectory = new DirectoryInfo(Path.Join((BehaviourEngine.SkyrimGameDirectory ?? outputDirectory ?? BehaviourEngine.CurrentDirectory ?? BehaviourEngine.AssemblyDirectory).FullName, project.ProjectFile.RelativeOutputDirectoryPath));

		ScanProjectAnimations(project, currentDirectory);
	}
	private static bool TryFindGraphHumanoid(DirectoryInfo folder, DirectoryInfo behaviorFolder, [NotNullWhen(true)] out FileInfo? graphFile)
	{
		graphFile = new(Path.Join(behaviorFolder.FullName, $"FNIS_{folder.Name}_Behavior.hkx"));
		return graphFile.Exists;
	}
	private static bool TryFindGraphCreature(string creatureName, DirectoryInfo folder, DirectoryInfo behaviorFolder,[NotNullWhen(true)] out FileInfo? graphFile)
	{
		graphFile = new(Path.Join(behaviorFolder.FullName, $"FNIS_{creatureName}_Behavior.hkx"));
		if (!graphFile.Exists)
		{
			graphFile = new(Path.Join(behaviorFolder.FullName, $"FNIS_{folder.Name}_{creatureName}_Behavior.hkx"));
			if (!graphFile.Exists)
			{
				return false;
			}
		}
		return true; 
	}
	private static bool TryFindAnimlistHumanoid(DirectoryInfo folder, Project project, [NotNullWhen(true)] out FileInfo? animListFile)
	{
		animListFile = new FileInfo(Path.Join(folder.FullName, $"FNIS_{folder.Name}_List.txt"));
		return animListFile.Exists;
	}
	private static bool TryFindAnimlistCreature(string creatureName, DirectoryInfo folder, Project project,[NotNullWhen(true)] out FileInfo? animListFile)
	{
		string listName = folder.Name;
		animListFile = new FileInfo(Path.Join(folder.FullName, $"FNIS_{folder.Name}_{creatureName}_List.txt"));
		return animListFile.Exists;
		//if (!animListFile.Exists)
		//{
		//	if (!manualScanDirectories.TryGetValue(creatureName, out var scanNames)) { return false; }
		//	foreach (string scanName in scanNames)
		//	{
		//		listName = scanName;
		//		animListFile = animListFile = new FileInfo(Path.Join(folder.FullName, $"FNIS_{folder.Name}_{creatureName}_List.txt"));
		//		if (animListFile.Exists)
		//		{
		//			break;
		//		}
		//	}
		//	if (!animListFile.Exists)
		//	{
		//		logger.Warn($"FNIS Parser > Find > Animlist > {folder.Name} in {project.Identifier} > FAILED");
		//		return false;
		//	}
		//}
		//return true; 
	}
	private bool InjectGraphReference(FileInfo sourceFile, PackFileGraph destPackFile)
	{
		lock (parsedFiles)
		{
			if (!parsedFiles.Add(sourceFile.FullName))
			{
				return false; 
			}
		}
		string stateFolderName;
		if (!stateMachineMap.TryGetValue(destPackFile.UniqueName, out stateFolderName!)) { return false; } //thread safe
		projectManager.TryActivatePackFile(destPackFile);
		string nameWithoutExtension = Path.GetFileNameWithoutExtension(sourceFile.Name);
		string graphPath = $"{destPackFile.InputHandle.Directory?.Name}\\{nameWithoutExtension}.hkx";

		hkbBehaviorReferenceGenerator refGenerator = new() { name = nameWithoutExtension, variableBindingSet = null, userData = 0, behaviorName = graphPath };
		hkbStateMachineStateInfo stateInfo = new() { name = "PN_StateInfo", enable = true, probability = 1.0f, stateId = graphPath.GetHashCode() & 0xfffffff, generator = refGenerator };
		hkbStateMachine rootState = destPackFile.GetPushedObjectAs<hkbStateMachine>(stateFolderName);
		lock (rootState)
		{
			rootState.states.Add(stateInfo);
		}
		return true;
	}
	private void ParseAnimlist(FileInfo animListFile, Project project, ProjectManager projectManager)
	{
		lock (parsedFiles)
		{
			if (!parsedFiles.Add(animListFile.FullName))
			{
				return;
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
		animList.BuildAllBehaviors(project, projectManager);
		animList.BuildAllAnimations(project, projectManager);

		lock (ModInfos)
		{
			ModInfos.Add(animList.ModInfo);
		}
	}
	public void SetOutputPath(DirectoryInfo outputPath)
	{
		outputDirectory = outputPath;
	}
}
