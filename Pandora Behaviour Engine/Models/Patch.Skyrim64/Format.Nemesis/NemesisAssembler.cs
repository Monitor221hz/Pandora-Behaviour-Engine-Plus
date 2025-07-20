using NLog;
using Pandora.API.Patch;
using Pandora.API.Patch.IOManagers;
using Pandora.Models.Patch.IO.Skyrim64;
using Pandora.Models.Patch.Skyrim64.AnimData;
using Pandora.Models.Patch.Skyrim64.AnimSetData;
using Pandora.Models.Patch.Skyrim64.Format.Pandora;
using Pandora.Models.Patch.Skyrim64.Hkx.Packfile;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using XmlCake.Linq.Expressions;

namespace Pandora.Models.Patch.Skyrim64.Format.Nemesis;

public class NemesisAssembler
{
	private static readonly Logger Logger = LogManager.GetCurrentClassLogger(); //to do: move logger into inheritable base class


	private IXExpression replacePattern = new XSkipWrapExpression(new XStep(XmlNodeType.Comment, "CLOSE"), new XStep(XmlNodeType.Comment, "OPEN"), new XStep(XmlNodeType.Comment, "ORIGINAL"), new XStep(XmlNodeType.Comment, "CLOSE"));
	private IXExpression insertPattern = new XSkipWrapExpression(new XStep(XmlNodeType.Comment, "ORIGINAL"), new XStep(XmlNodeType.Comment, "OPEN"), new XStep(XmlNodeType.Comment, "CLOSE"));

	//private XPathLookup lookup = new XPathLookup();

	List<PackFile> packFiles = [];

	private static readonly DirectoryInfo engineFolder = new(Environment.CurrentDirectory + "\\Pandora_Engine");

	private static readonly DirectoryInfo templateFolder = new(Path.Combine(Environment.CurrentDirectory, "Pandora_Engine\\Skyrim\\Template"));

	private static readonly DirectoryInfo defaultOutputMeshFolder = new(Path.Join(Environment.CurrentDirectory, "meshes"));

	private static readonly DirectoryInfo currentDirectory = new(Environment.CurrentDirectory);
	public ProjectManager ProjectManager { get; private set; }
	public AnimDataManager AnimDataManager { get; private set; }
	public AnimSetDataManager AnimSetDataManager { get; private set; }

	private readonly PandoraBridgedAssembler pandoraConverter;

	private IMetaDataExporter<PackFile> exporter = new PackFileExporter();
	public NemesisAssembler()
	{
		ProjectManager = new ProjectManager(templateFolder, currentDirectory);
		AnimSetDataManager = new AnimSetDataManager(templateFolder, defaultOutputMeshFolder);
		AnimDataManager = new AnimDataManager(templateFolder, defaultOutputMeshFolder);

		pandoraConverter = new PandoraBridgedAssembler(ProjectManager, AnimSetDataManager, AnimDataManager);
	}
	public NemesisAssembler(IMetaDataExporter<PackFile> ioManager)
	{
		exporter = ioManager;
		ProjectManager = new ProjectManager(templateFolder, currentDirectory);
		AnimSetDataManager = new AnimSetDataManager(templateFolder, defaultOutputMeshFolder);
		AnimDataManager = new AnimDataManager(templateFolder, defaultOutputMeshFolder);

		pandoraConverter = new PandoraBridgedAssembler(ProjectManager, AnimSetDataManager, AnimDataManager);
	}
	public NemesisAssembler(IMetaDataExporter<PackFile> ioManager, ProjectManager projManager, AnimSetDataManager animSDManager, AnimDataManager animDManager)
	{
		exporter = ioManager;
		ProjectManager = projManager;
		AnimSetDataManager = animSDManager;
		AnimDataManager = animDManager;
		pandoraConverter = new PandoraBridgedAssembler(ProjectManager, AnimSetDataManager, AnimDataManager);
	}

	public void SetOutputPath(DirectoryInfo baseOutputDirectory)
	{
		var outputMeshDirectory = new DirectoryInfo(Path.Join(baseOutputDirectory.FullName, "meshes"));
		ProjectManager.SetOutputPath(baseOutputDirectory);
		AnimDataManager.SetOutputPath(outputMeshDirectory);
		AnimSetDataManager.SetOutputPath(outputMeshDirectory);
	}

	public void LoadResources()
	{
		throw new NotImplementedException();


	}
	public void GetPostMessages(StringBuilder builder)
	{
		ProjectManager.GetFNISInfo(builder);
		ProjectManager.GetAnimationInfo(builder);
		ProjectManager.GetExportInfo(builder);
	}
	public async Task LoadResourcesAsync()
	{
		var animSetDataTask = Task.Run(() => { AnimSetDataManager.SplitAnimSetDataSingleFile(); });
		await Task.Run(ProjectManager.LoadTrackedProjects);
		await Task.Run(() => { AnimDataManager.SplitAnimationDataSingleFile(ProjectManager); });
		await animSetDataTask;

	}

	public void AssemblePatch(IModInfo modInfo)
	{
		DirectoryInfo folder = modInfo.Folder;
		DirectoryInfo[] subFolders = folder.GetDirectories();

		foreach (DirectoryInfo subFolder in subFolders)
		{
			if (AssemblePackFilePatch(subFolder, modInfo)) continue;
			if (subFolder.Name.StartsWith("animationsetdata"))
			{
				AssembleAnimSetDataPatch(subFolder);
				continue;
			}
			if (subFolder.Name.StartsWith("animationdata"))
			{
				AssembleAnimDataPatch(subFolder);
				continue;
			}
			if (subFolder.Name.StartsWith("plugin"))
			{

			}

		}
	}
	public void AssemblePatch(IModInfo modInfo, DirectoryInfo folder)
	{
		DirectoryInfo[] subFolders = folder.GetDirectories();
		foreach (DirectoryInfo subFolder in subFolders)
		{
			if (AssemblePackFilePatch(subFolder, modInfo)) continue;
			if (subFolder.Name.StartsWith("animationsetdata")) AssembleAnimSetDataPatch(subFolder);
			if (subFolder.Name.StartsWith("animationdata")) AssembleAnimDataPatch(subFolder);

		}
	}
	public bool ApplyPatches() => true;
	public Task QueueNativePatchesAsync()
	{
		return Task.Run(() => pandoraConverter.QueueNativePatches());
	}
	public Task LoadMetaDataAsync()
	{
		return Task.Run(() => { exporter.LoadMetaData(); });
	}
	public Task SaveMetaDataAsync()
	{
		return Task.Run(() => { exporter.SaveMetaData(ProjectManager.ActivePackFiles); });
	}
	public async Task ApplyNativePatchesAsync(RunOrder order)
	{
		await Task.Run(() => { pandoraConverter.ApplyNativePatches(RuntimeMode.Serial, order); });
		await Task.Run(() => { pandoraConverter.ApplyNativePatches(RuntimeMode.Parallel, order); });
	}
	public async Task MergeAllAnimationDataAsync()
	{
		await Task.Run(() => { AnimSetDataManager.MergeAnimSetDataSingleFile(); });
		await Task.Run(() => { AnimDataManager.MergeAnimDataSingleFile(); });
	}
	public async Task<bool> ApplyPatchesAsync()
	{
		var queueNativeTask = QueueNativePatchesAsync();
		var loadMetaDataTask = LoadMetaDataAsync();
		await queueNativeTask;
		await ApplyNativePatchesAsync(RunOrder.PreLaunch);
		var mainTask = ProjectManager.ApplyPatchesParallel();
		await ApplyNativePatchesAsync(RunOrder.PostLaunch);
		var allAnimDataTask = MergeAllAnimationDataAsync();
		await loadMetaDataTask;
		bool exportSuccess = exporter.ExportParallel(ProjectManager.ActivePackFiles);
		var saveMetaDataTask = SaveMetaDataAsync();
		await allAnimDataTask;
		await saveMetaDataTask;
		return mainTask && exportSuccess;
	}

	public void AssembleAnimDataPatch(DirectoryInfo folder)
	{
		foreach (var subFolder in folder.GetDirectories())
		{
			AssembleProjectAnimDataPatch(subFolder);
		}
	}
	public void AssembleProjectAnimDataPatch(DirectoryInfo folder)
	{

		var projectName = folder.Name.Split('~')[0];

		if (!ProjectManager.TryGetProject(projectName, out var project) ||
			project.AnimData == null ||
			project.AnimData.BoundMotionDataProject == null)
		{
			return;
		}

		var clipDataFiles = folder.GetFiles("*~*.txt");
		foreach (var clipDataFile in clipDataFiles)
		{
			if (!ClipDataBlock.TryReadBlock(clipDataFile, out var clipDataBlock))
			{
				continue;
			}
			FileInfo motionDataFile = new(Path.Combine(clipDataFile.DirectoryName!, $"{clipDataBlock.ClipID}.txt"));
			if (!motionDataFile.Exists || !ClipMotionDataBlock.TryReadBlock(motionDataFile, out var motionDataBlock))
			{
				continue;
			}
			project.AnimData.AddClipData(clipDataBlock, motionDataBlock);
		}

	}
	public void AssembleAnimSetDataPatch(DirectoryInfo directoryInfo)
	{
		pandoraConverter.AssembleAnimSetDataPatch(directoryInfo);
	}

	private bool AssemblePackFilePatch(DirectoryInfo folder, Project project, IModInfo modInfo)
	{
		PackFile targetPackFile;
		if (!ProjectManager.TryActivatePackFilePriority(folder.Name, project, out targetPackFile!))
		{
			return false;
		}
		lock (targetPackFile) targetPackFile.Dispatcher.AddChangeSet(NemesisParser.ParsePackFileChanges(targetPackFile, modInfo, folder));
		return true;
	}
	private bool AssemblePackFilePatch(DirectoryInfo folder, IModInfo modInfo)
	{
		PackFile targetPackFile;
		if (!ProjectManager.TryActivatePackFilePriority(folder.Name, out targetPackFile!))
		{
			Project targetProject;
			if (!ProjectManager.TryLookupProjectFolder(folder.Name, out targetProject!)) { return false; }


			DirectoryInfo[] subFolders = folder.GetDirectories();
			foreach (DirectoryInfo subFolder in subFolders)
			{
				AssemblePackFilePatch(subFolder, targetProject, modInfo);
			}
			return true;
		}
		lock (targetPackFile) targetPackFile.Dispatcher.AddChangeSet(NemesisParser.ParsePackFileChanges(targetPackFile, modInfo, folder));
		return true;
	}

	public List<(FileInfo inFile, FileInfo outFile)> GetExportFiles()
	{
		List<(FileInfo inFile, FileInfo outFile)> exportFiles = [];
		foreach (PackFile packFile in ProjectManager.ActivePackFiles)
		{
			exportFiles.Add((packFile.InputHandle, new FileInfo(Path.Join(Environment.CurrentDirectory, packFile.InputHandle.Name))));
		}
		return exportFiles;
	}
}
