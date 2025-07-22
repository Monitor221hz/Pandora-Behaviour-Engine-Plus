using Pandora.API.Patch;
using Pandora.API.Patch.IOManagers;
using Pandora.Models.Patch.IO.Skyrim64;
using Pandora.Models.Patch.Skyrim64.AnimData;
using Pandora.Models.Patch.Skyrim64.AnimSetData;
using Pandora.Models.Patch.Skyrim64.Format.Nemesis;
using Pandora.Models.Patch.Skyrim64.Hkx.Changes;
using Pandora.Models.Patch.Skyrim64.Hkx.Packfile;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace Pandora.Models.Patch.Skyrim64.Format.Pandora;

public class PandoraAssembler
{
	private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

	private readonly DirectoryInfo templateFolder = new(Environment.CurrentDirectory + "\\Pandora_Engine\\Skyrim\\Template");

	private readonly DirectoryInfo defaultOutputMeshFolder = new($"{Environment.CurrentDirectory}\\meshes");

	public ProjectManager ProjectManager { get; private set; }
	public AnimDataManager AnimDataManager { get; private set; }
	public AnimSetDataManager AnimSetDataManager { get; private set; }

	private PandoraNativePatchManager nativeManager = new();

	private IMetaDataExporter<PackFile> exporter = new PackFileExporter();
	public PandoraAssembler(IMetaDataExporter<PackFile> exporter)
	{
		this.exporter = exporter;
		ProjectManager = new ProjectManager(templateFolder, defaultOutputMeshFolder);
		AnimSetDataManager = new AnimSetDataManager(templateFolder, defaultOutputMeshFolder);
		AnimDataManager = new AnimDataManager(templateFolder, defaultOutputMeshFolder);
	}
	public PandoraAssembler(IMetaDataExporter<PackFile> exporter, NemesisAssembler nemesisAssembler)
	{
		this.exporter = exporter;
		ProjectManager = nemesisAssembler.ProjectManager;
		AnimSetDataManager = nemesisAssembler.AnimSetDataManager;
		AnimDataManager = nemesisAssembler.AnimDataManager;
	}
	public PandoraAssembler(ProjectManager projManager, AnimSetDataManager animSDManager, AnimDataManager animDManager)
	{
		ProjectManager = projManager;
		AnimSetDataManager = animSDManager;
		AnimDataManager = animDManager;
	}

	public void SetOutputPath(DirectoryInfo baseOutputDirectory)
	{
		var outputMeshDirectory = new DirectoryInfo(Path.Join(baseOutputDirectory.FullName, "meshes"));
		ProjectManager.SetOutputPath(baseOutputDirectory);
		AnimDataManager.SetOutputPath(outputMeshDirectory);
		AnimSetDataManager.SetOutputPath(outputMeshDirectory);
	}



	public bool AssemblePackFilePatch(FileInfo file, IModInfo modInfo)
	{

		var name = Path.GetFileNameWithoutExtension(file.Name);
		PackFile targetPackFile;
		if (!ProjectManager.TryActivatePackFilePriority(name, out targetPackFile!)) { return false; }

		var changeSet = new PackFileChangeSet(modInfo);

		XElement container;
		using (FileStream stream = file.OpenRead())
		{
			container = XElement.Load(stream);
		}
		var editContainer = container;

		if (editContainer == null) { return false; }

		PandoraParser.ParseEdits(editContainer, targetPackFile, changeSet);

		targetPackFile.Dispatcher.AddChangeSet(changeSet);
		return true;
	}
	public void AssemblePatch(IModInfo modInfo)
	{
		var patchFolder = new DirectoryInfo(Path.Join(modInfo.Folder.FullName, "patches"));
		if (patchFolder.Exists)
		{
			foreach (var file in patchFolder.GetFiles("*.xml"))
			{
				AssemblePackFilePatch(file, modInfo);
			}
		}

		var pluginFolder = new DirectoryInfo(Path.Join(modInfo.Folder.FullName, "native"));
		if (pluginFolder.Exists)
		{
			foreach (var folder in pluginFolder.GetDirectories())
			{
				nativeManager.LoadAssembly(folder);
			}
		}

	}
	public void ApplyNativePatches(RuntimeMode mode, RunOrder order) { nativeManager.ApplyPatches(ProjectManager, mode, order); }
	public void QueueNativePatches() { nativeManager.QueuePatches(); }
	public void AssembleAnimDataPatch(DirectoryInfo folder)
	{
		var files = folder.GetFiles();
		foreach (var file in files)
		{
			if (!file.Exists || !ProjectManager.TryGetProject(Path.GetFileNameWithoutExtension(file.Name.ToLower()), out Project? targetProject)) continue;

			using (var readStream = file.OpenRead())
			{
				using (var reader = new StreamReader(readStream))
				{
					string? expectedLine;
					while ((expectedLine = reader.ReadLine()) != null)
					{
						if (string.IsNullOrWhiteSpace(expectedLine)) continue;
						targetProject!.AnimData?.AddDummyClipData(expectedLine);
					}
				}
			}
		}
	}
	public void AssembleAnimSetDataPatch(DirectoryInfo directoryInfo) //not exactly Nemesis format but this format is just simpler
	{
		ProjectAnimSetData? targetAnimSetData;

		foreach (DirectoryInfo subDirInfo in directoryInfo.GetDirectories())
		{
			if (!AnimSetDataManager.AnimSetDataMap.TryGetValue(subDirInfo.Name, out targetAnimSetData)) continue;
			var patchFiles = subDirInfo.GetFiles();

			foreach (var patchFile in patchFiles)
			{

				if (!targetAnimSetData.AnimSetsByName.TryGetValue(patchFile.Name, out AnimSet? targetAnimSet)) continue;

				using (var readStream = patchFile.OpenRead())
				{

					using (var reader = new StreamReader(readStream))
					{

						string? expectedPath;
						while ((expectedPath = reader.ReadLine()) != null)
						{
							if (string.IsNullOrWhiteSpace(expectedPath)) continue;

							string animationName = Path.GetFileNameWithoutExtension(expectedPath);
							string folder = Path.GetDirectoryName(expectedPath)!;
							var animInfo = SetCachedAnimInfo.Encode(folder, animationName);
							targetAnimSet.AddAnimInfo(animInfo);
						}

					}
				}
			}
		}
		foreach (FileInfo patchFile in directoryInfo.GetFiles("*.txt"))
		{
			if (!AnimSetDataManager.AnimSetDataMap.TryGetValue(Path.GetFileNameWithoutExtension(patchFile.Name), out targetAnimSetData)) continue;
			List<SetCachedAnimInfo> animInfos = [];
			using (var readStream = patchFile.OpenRead())
			{
				using (var reader = new StreamReader(readStream))
				{
					string? expectedPath;
					while ((expectedPath = reader.ReadLine()) != null)
					{
						if (string.IsNullOrWhiteSpace(expectedPath)) continue;

						string animationName = Path.GetFileNameWithoutExtension(expectedPath);
						string folder = Path.GetDirectoryName(expectedPath)!;
						var animInfo = SetCachedAnimInfo.Encode(folder, animationName);
						animInfos.Add(animInfo);
					}
				}
			}
			foreach (var animSet in targetAnimSetData.AnimSets)
			{
				foreach (var animInfo in animInfos)
				{
					animSet.AddAnimInfo(animInfo);
				}
			}
			break;
		}



	}
}
