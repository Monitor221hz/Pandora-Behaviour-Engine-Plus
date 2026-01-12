// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using Pandora.API.Patch;
using Pandora.API.Patch.IOManagers;
using Pandora.API.Patch.Skyrim64;
using Pandora.API.Patch.Skyrim64.AnimData;
using Pandora.API.Patch.Skyrim64.AnimSetData;
using Pandora.API.Utils;
using Pandora.Models.Patch.IO.Skyrim64;
using Pandora.Models.Patch.Skyrim64.AnimData;
using Pandora.Models.Patch.Skyrim64.AnimSetData;
using Pandora.Models.Patch.Skyrim64.Hkx.Changes;
using Pandora.Models.Patch.Skyrim64.Hkx.Packfile;
using Pandora.Utils;

namespace Pandora.Models.Patch.Skyrim64.Format.Pandora;

public class PandoraAssembler
{
	private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

	private readonly DirectoryInfo templateFolder = new(
		Path.Join(
			BehaviourEngine.AssemblyDirectory.FullName,
			"Pandora_Engine",
			"Skyrim",
			"Template"
		)
	);
	private readonly PandoraNativePatchManager nativeManager = new();
	private readonly IPathResolver _pathResolver;
	private readonly IMetaDataExporter<IPackFile> _packFileExporter;

	public IProjectManager ProjectManager { get; private set; }
	public IAnimDataManager AnimDataManager { get; private set; }
	public IAnimSetDataManager AnimSetDataManager { get; private set; }

	public PandoraAssembler(
		IPathResolver pathResolver,
		IMetaDataExporter<IPackFile> exporter,
		IProjectManager projectManager,
		IAnimDataManager animDataManager,
		IAnimSetDataManager animSetDataManager
	)
	{
		_pathResolver = pathResolver;
		_packFileExporter = exporter;
		ProjectManager = projectManager;
		AnimSetDataManager = animSetDataManager;
		AnimDataManager = animDataManager;
	}

	public PandoraAssembler(
		IPathResolver pathResolver,
		IMetaDataExporter<IPackFile> exporter,
		IPatchAssembler nemesisAssembler
	)
	{
		_pathResolver = pathResolver;
		_packFileExporter = exporter;
		ProjectManager = nemesisAssembler.ProjectManager;
		AnimSetDataManager = nemesisAssembler.AnimSetDataManager;
		AnimDataManager = nemesisAssembler.AnimDataManager;
	}

	public PandoraAssembler(
		IPathResolver pathResolver,
		IMetaDataExporter<IPackFile> exporter,
		IProjectManager projManager,
		IAnimSetDataManager animSDManager,
		IAnimDataManager animDManager
	)
	{
		_pathResolver = pathResolver;
		_packFileExporter = exporter;
		ProjectManager = projManager;
		AnimSetDataManager = animSDManager;
		AnimDataManager = animDManager;
	}

	public bool AssemblePackFilePatch(FileInfo file, IModInfo modInfo)
	{
		var name = Path.GetFileNameWithoutExtension(file.Name);
		IPackFile targetPackFile;
		if (!ProjectManager.TryActivatePackFilePriority(name, out targetPackFile!))
		{
			return false;
		}

		var changeSet = new PackFileChangeSet(modInfo);

		XElement container;
		using (FileStream stream = file.OpenRead())
		{
			container = XElement.Load(stream);
		}
		var editContainer = container;

		if (editContainer == null)
		{
			return false;
		}

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

	public void ApplyNativePatches(RuntimeMode mode, RunOrder order)
	{
		nativeManager.ApplyPatches(ProjectManager, mode, order);
	}

	public void QueueNativePatches()
	{
		nativeManager.QueuePatches();
	}

	public void AssembleAnimDataPatch(DirectoryInfo folder)
	{
		var files = folder.GetFiles();
		foreach (var file in files)
		{
			if (
				!file.Exists
				|| !ProjectManager.TryGetProject(
					Path.GetFileNameWithoutExtension(file.Name.ToLower()),
					out IProject? targetProject
				)
			)
				continue;

			using (var readStream = file.OpenRead())
			{
				using (var reader = new StreamReader(readStream))
				{
					string? expectedLine;
					while ((expectedLine = reader.ReadLine()) != null)
					{
						if (string.IsNullOrWhiteSpace(expectedLine))
							continue;
						targetProject!.AnimData?.AddDummyClipData(expectedLine);
					}
				}
			}
		}
	}

	public void AssembleAnimSetDataPatch(DirectoryInfo directoryInfo) //not exactly Nemesis format but this format is just simpler
	{
		IProjectAnimSetData? targetAnimSetData;

		foreach (DirectoryInfo subDirInfo in directoryInfo.GetDirectories())
		{
			if (
				!AnimSetDataManager.AnimSetDataMap.TryGetValue(
					subDirInfo.Name,
					out targetAnimSetData
				)
			)
				continue;
			var patchFiles = subDirInfo.GetFiles();

			foreach (var patchFile in patchFiles)
			{
				if (
					!targetAnimSetData.AnimSetsByName.TryGetValue(
						patchFile.Name,
						out IAnimSet? targetAnimSet
					)
				)
					continue;

				using (var readStream = patchFile.OpenRead())
				{
					using (var reader = new StreamReader(readStream))
					{
						string? expectedPath;
						while ((expectedPath = reader.ReadLine()) != null)
						{
							if (string.IsNullOrWhiteSpace(expectedPath))
								continue;

							string animationName = Path.GetFileNameWithoutExtension(expectedPath);
							string folder = Path.GetDirectoryName(expectedPath)!;
							var animInfo = new SetCachedAnimInfo().Encode(folder, animationName);
							targetAnimSet.AddAnimInfo(animInfo);
						}
					}
				}
			}
		}
		foreach (FileInfo patchFile in directoryInfo.GetFiles("*.txt"))
		{
			if (
				!AnimSetDataManager.AnimSetDataMap.TryGetValue(
					Path.GetFileNameWithoutExtension(patchFile.Name),
					out targetAnimSetData
				)
			)
				continue;
			List<ISetCachedAnimInfo> animInfos = [];
			using (var readStream = patchFile.OpenRead())
			{
				using (var reader = new StreamReader(readStream))
				{
					string? expectedPath;
					while ((expectedPath = reader.ReadLine()) != null)
					{
						if (string.IsNullOrWhiteSpace(expectedPath))
							continue;

						string animationName = Path.GetFileNameWithoutExtension(expectedPath);
						string folder = Path.GetDirectoryName(expectedPath)!;
						var animInfo = new SetCachedAnimInfo().Encode(folder, animationName);
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
