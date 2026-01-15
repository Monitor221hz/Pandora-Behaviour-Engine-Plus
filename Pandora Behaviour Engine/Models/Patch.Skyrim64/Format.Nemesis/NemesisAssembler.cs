// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using NLog;
using Pandora.API.Patch;
using Pandora.API.Patch.IOManagers;
using Pandora.API.Patch.Skyrim64;
using Pandora.API.Patch.Skyrim64.AnimData;
using Pandora.API.Patch.Skyrim64.AnimSetData;
using Pandora.Models.Patch.Skyrim64.AnimData;
using Pandora.Models.Patch.Skyrim64.Format.Pandora;
using Pandora.Models.Patch.Skyrim64.Hkx.Packfile;
using Pandora.Paths.Contexts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using XmlCake.Linq.Expressions;

namespace Pandora.Models.Patch.Skyrim64.Format.Nemesis;

public class NemesisAssembler : IPatchAssembler
{
	private static readonly Logger Logger = LogManager.GetCurrentClassLogger(); //to do: move logger into inheritable base class

	private readonly IEnginePathContext _pathContext;

	private readonly PandoraBridgedAssembler _pandoraConverter;
	private readonly IMetaDataExporter<IPackFile> _exporter;

	private static readonly IXExpression replacePattern = new XSkipWrapExpression(
		new XStep(XmlNodeType.Comment, "CLOSE"),
		new XStep(XmlNodeType.Comment, "OPEN"),
		new XStep(XmlNodeType.Comment, "ORIGINAL"),
		new XStep(XmlNodeType.Comment, "CLOSE")
	);
	private static readonly IXExpression insertPattern = new XSkipWrapExpression(
		new XStep(XmlNodeType.Comment, "ORIGINAL"),
		new XStep(XmlNodeType.Comment, "OPEN"),
		new XStep(XmlNodeType.Comment, "CLOSE")
	);

	//private XPathLookup lookup = new XPathLookup();

	List<PackFile> packFiles = [];

	public IProjectManager ProjectManager { get; private set; }
	public IAnimDataManager AnimDataManager { get; private set; }
	public IAnimSetDataManager AnimSetDataManager { get; private set; }

	public NemesisAssembler(
	   IEnginePathContext pathContext,
	   IMetaDataExporter<IPackFile> exporter,
	   IProjectManager projectManager,
	   IAnimDataManager animDataManager,
	   IAnimSetDataManager animSetDataManager,
	   PandoraBridgedAssembler bridgedAssembler
   )
	{
		_pathContext = pathContext;
		_exporter = exporter;
		ProjectManager = projectManager;
		AnimDataManager = animDataManager;
		AnimSetDataManager = animSetDataManager;

		_pandoraConverter = bridgedAssembler;
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
		var projectLoadTask = Task.Run(ProjectManager.LoadTrackedProjects);
		var animSetDataTask = Task.Run(AnimSetDataManager.SplitAnimSetDataSingleFile);

		await Task.WhenAll(projectLoadTask, animSetDataTask);

		await Task.Run(() => AnimDataManager.SplitAnimDataSingleFile(ProjectManager));
	}

	public void AssemblePatch(IModInfo modInfo)
	{
		DirectoryInfo folder = modInfo.Folder;
		DirectoryInfo[] subFolders = folder.GetDirectories();

		foreach (DirectoryInfo subFolder in subFolders)
		{
			if (AssemblePackFilePatch(subFolder, modInfo))
				continue;
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
			if (subFolder.Name.StartsWith("plugin")) { }
		}
	}

	public void AssemblePatch(IModInfo modInfo, DirectoryInfo folder)
	{
		DirectoryInfo[] subFolders = folder.GetDirectories();
		foreach (DirectoryInfo subFolder in subFolders)
		{
			if (AssemblePackFilePatch(subFolder, modInfo))
				continue;
			if (subFolder.Name.StartsWith("animationsetdata"))
				AssembleAnimSetDataPatch(subFolder);
			if (subFolder.Name.StartsWith("animationdata"))
				AssembleAnimDataPatch(subFolder);
		}
	}

	public bool ApplyPatches() => true;

	public Task QueueNativePatchesAsync()
	{
		return Task.Run(() => _pandoraConverter.QueueNativePatches());
	}

	public Task LoadMetaDataAsync()
	{
		return Task.Run(() =>
		{
			_exporter.LoadMetaData();
		});
	}

	public Task SaveMetaDataAsync()
	{
		return Task.Run(() =>
		{
			_exporter.SaveMetaData(ProjectManager.ActivePackFiles);
		});
	}

	public async Task ApplyNativePatchesAsync(RunOrder order)
	{
		await Task.Run(() =>
		{
			_pandoraConverter.ApplyNativePatches(RuntimeMode.Serial, order);
		});
		await Task.Run(() =>
		{
			_pandoraConverter.ApplyNativePatches(RuntimeMode.Parallel, order);
		});
	}

	public async Task MergeAllAnimationDataAsync()
	{
		await Task.Run(() =>
		{
			AnimSetDataManager.MergeAnimSetDataSingleFile();
		});
		await Task.Run(() =>
		{
			AnimDataManager.MergeAnimDataSingleFile();
		});
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
		bool exportSuccess = _exporter.ExportParallel(ProjectManager.ActivePackFiles);
		var saveMetaDataTask = SaveMetaDataAsync();
		await allAnimDataTask;
		await saveMetaDataTask;
		Debug.WriteLine("Thread");
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

		if (
			!ProjectManager.TryGetProject(projectName, out var project)
			|| project.AnimData == null
			|| project.AnimData.BoundMotionDataProject == null
		)
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
			FileInfo motionDataFile = new(
				Path.Combine(clipDataFile.DirectoryName!, $"{clipDataBlock.ClipID}.txt")
			);
			if (
				!motionDataFile.Exists
				|| !ClipMotionDataBlock.TryReadBlock(motionDataFile, out var motionDataBlock)
			)
			{
				continue;
			}
			project.AnimData.AddClipData(clipDataBlock, motionDataBlock);
		}
	}

	public void AssembleAnimSetDataPatch(DirectoryInfo directoryInfo)
	{
		_pandoraConverter.AssembleAnimSetDataPatch(directoryInfo);
	}

	private bool AssemblePackFilePatch(DirectoryInfo folder, IProject project, IModInfo modInfo)
	{
		IPackFile targetPackFile;
		if (!ProjectManager.TryActivatePackFilePriority(folder.Name, project, out targetPackFile!))
		{
			return false;
		}
		lock (targetPackFile)
			targetPackFile.Dispatcher.AddChangeSet(
				NemesisParser.ParsePackFileChanges(targetPackFile, modInfo, folder)
			);
		return true;
	}

	private bool AssemblePackFilePatch(DirectoryInfo folder, IModInfo modInfo)
	{
		IPackFile targetPackFile;
		if (!ProjectManager.TryActivatePackFilePriority(folder.Name, out targetPackFile!))
		{
			IProject targetProject;
			if (!ProjectManager.TryLookupProjectFolder(folder.Name, out targetProject!))
			{
				return false;
			}

			DirectoryInfo[] subFolders = folder.GetDirectories();
			foreach (DirectoryInfo subFolder in subFolders)
			{
				AssemblePackFilePatch(subFolder, targetProject, modInfo);
			}
			return true;
		}
		lock (targetPackFile)
			targetPackFile.Dispatcher.AddChangeSet(
				NemesisParser.ParsePackFileChanges(targetPackFile, modInfo, folder)
			);
		return true;
	}

	public List<(FileInfo inFile, FileInfo outFile)> GetExportFiles()
	{
		List<(FileInfo inFile, FileInfo outFile)> exportFiles = [];
		foreach (PackFile packFile in ProjectManager.ActivePackFiles)
		{
			exportFiles.Add(
				(
					packFile.InputHandle,
					new FileInfo(Path.Join(_pathContext.CurrentFolder.FullName, packFile.InputHandle.Name))
				)
			);
		}
		return exportFiles;
	}
}
