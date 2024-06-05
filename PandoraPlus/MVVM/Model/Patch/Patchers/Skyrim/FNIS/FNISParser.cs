using HKX2;
using Pandora.Core;
using Pandora.Core.Patchers.Skyrim;
using Pandora.Patch.Patchers.Skyrim.AnimData;
using Pandora.Patch.Patchers.Skyrim.AnimSetData;
using Pandora.Patch.Patchers.Skyrim.Hkx;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Threading.Tasks.Sources;
using System.Xml.Linq;

namespace Pandora.Patch.Patchers.Skyrim.FNIS;
public class FNISParser
{
	private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

	private static readonly HashSet<string> animTypePrefixes = new HashSet<string>() { "b", "s", "so", "fu", "fuo", "+", "ofa", "pa", "km", "aa", "ch" };

	private static readonly Regex hkxRegex = new Regex("\\S*\\.hkx", RegexOptions.IgnoreCase);

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
		{"draugrproject~draugrbehavior", "#1998" },
		{"draugrskeletonproject~draugrbehavior", "#1998" },
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

	private static readonly Dictionary<string, string> linkedCharacterMap = new Dictionary<string, string>()
	{
		{"defaultmale", "defaultfemale" },
		{"defaultfemale", "defaultmale" },
		{"draugrskeletonproject", "draugrproject" },
		{"draugrproject", "draugrskeletonproject" },
		{"wolfproject", "dogproject" },
		{"dogproject", "wolfproject" }
	};

	private static readonly Dictionary<string, string> animListExcludeMap = new Dictionary<string, string>()
	{
		{"dogproject", "wolf" },
		{"wolfproject", "dog" }
	};

	private HashSet<string> parsedFolders = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

	public HashSet<FNISModInfo> ModInfos { get; private set; } = new HashSet<FNISModInfo>();

    public FNISParser(ProjectManager manager)
    {
		projectManager = manager;
    }
	private ProjectManager projectManager { get;  }
    public void ScanProjectAnimlist(Project project)
	{
		var animationsFolder = project.OutputAnimationDirectory;
		var behaviorFolder = project.OutputBehaviorDirectory;
		if (animationsFolder == null || behaviorFolder == null || !behaviorFolder.Exists) { return; }
		lock (parsedFolders)
		{
			if (parsedFolders.Contains(behaviorFolder.FullName)) { return; }

			parsedFolders.Add(animationsFolder.FullName);
			parsedFolders.Add(behaviorFolder.FullName);
		}
		var modFiles = behaviorFolder.GetFiles("FNIS*.hkx");

		if (modFiles.Length > 0) { projectManager.ActivatePackFile(project.BehaviorFile); }

		foreach (var modFile in modFiles)
		{
			InjectGraphReference(modFile, project.BehaviorFile);
		}
		if (!animationsFolder.Exists) { return; }
		var modAnimationFolders = animationsFolder.GetDirectories();

		if (modAnimationFolders.Length == 0) { return; }

		Parallel.ForEach(modAnimationFolders, folder => { ParseAnimlistFolder(folder, project, projectManager); });
	}
	private bool InjectGraphReference(FileInfo sourceFile, PackFileGraph destPackFile)
	{
		string stateFolderName;

		if (!stateMachineMap.TryGetValue(destPackFile.UniqueName, out stateFolderName!)) { return false; }

		destPackFile.MapNode(stateFolderName);

		string nameWithoutExtension = Path.GetFileNameWithoutExtension(sourceFile.Name);
		string refName = nameWithoutExtension.Replace(' ', '_');
		var stateInfoPath = string.Format("{0}/states", stateFolderName);
		var graphPath = $"{destPackFile.OutputHandle.Directory?.Name}\\{nameWithoutExtension}.hkx";
		var modInfo = new FNISModInfo(sourceFile);
		lock (ModInfos) ModInfos.Add(modInfo);
		var changeSet = new PackFileChangeSet(modInfo);

		PatchNodeCreator nodeMaker = new PatchNodeCreator(changeSet.Origin.Code);

		string behaviorRefName;
		var behaviorRef = nodeMaker.CreateBehaviorReferenceGenerator(refName, graphPath, out behaviorRefName);
		XElement behaviorRefElement = nodeMaker.TranslateToLinq<hkbBehaviorReferenceGenerator>(behaviorRef, behaviorRefName);

		string stateInfoName;
		var stateInfo = nodeMaker.CreateSimpleStateInfo(behaviorRef, out stateInfoName);
		XElement stateInfoElement = nodeMaker.TranslateToLinq<hkbStateMachineStateInfo>(stateInfo, stateInfoName);

		changeSet.AddChange(new AppendElementChange(PackFile.ROOT_CONTAINER_NAME, behaviorRefElement));
		changeSet.AddChange(new AppendElementChange(PackFile.ROOT_CONTAINER_NAME, stateInfoElement));
		changeSet.AddChange(new AppendTextChange(stateInfoPath, stateInfoName));

		destPackFile.Dispatcher.AddChangeSet(changeSet);
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

		

		List<PackFileCharacter> characterFiles = new List<PackFileCharacter> { project.CharacterFile };

		if (linkedCharacterMap.TryGetValue(project.Identifier, out var linkedProjectIdentifier) && projectManager.TryGetProject(linkedProjectIdentifier, out var linkedProject))
		{
			characterFiles.Add(linkedProject!.CharacterFile);
			
		}
		foreach(var characterFile in characterFiles) { projectManager.ActivatePackFile(characterFile); }
		foreach (var animlistFile in animlistFiles)
		{
			ParseAnimlist(animlistFile, characterFiles);
		}
	}
	private void ParseAnimlist(FileInfo file, List<PackFileCharacter> characterPackFiles)
	{


		List<PackFileChangeSet> changeSets = new List<PackFileChangeSet>();

		for(int i = 0; i < characterPackFiles.Count; i++) { changeSets.Add(new PackFileChangeSet(new FNISModInfo(file)));  }


		var parentFolder = file.Directory;
		if (parentFolder == null) { return; }


		var ancestorFolder = parentFolder.Parent;
		if (ancestorFolder == null) { return; }


		var root = Path.Combine(ancestorFolder.Name, parentFolder.Name);

		using (var readStream = file.OpenRead())
		{
			using (var reader = new StreamReader(readStream))
			{
				string? expectedLine; 
				while((expectedLine = reader.ReadLine()) != null)
				{
					if (string.IsNullOrWhiteSpace(expectedLine) || expectedLine[0] == '\'') { continue; }

					var match = hkxRegex.Match(expectedLine);
					if (!match.Success) { continue; }
					
					var animationPath = Path.Combine(root, match.Value);
					for (int i = 0; i < changeSets.Count; i++)
					{
						changeSets[i].AddChange(new AppendElementChange(characterPackFiles[i].AnimationNamesPath, new XElement("hkcstring", animationPath)));
					}
					
				}
			}
		}
		for (int i = 0; i< characterPackFiles.Count ; i++)
		{
			characterPackFiles[i].Dispatcher.AddChangeSet(changeSets[i]);
		}
		
	}


}
