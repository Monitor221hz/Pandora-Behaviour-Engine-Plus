// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using NLog;
using Pandora.API.Patch;
using Pandora.API.Patch.Skyrim64;
using Pandora.Models.Patch.Skyrim64.Format.FNIS;
using Pandora.Models.Patch.Skyrim64.Hkx.Packfile;
using Pandora.Paths.Abstractions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandora.Models.Patch.Skyrim64;

public class ProjectManager : IProjectManager
{
	private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

	private readonly Dictionary<string, IProject> _projectMap = new Dictionary<string, IProject>(
		StringComparer.OrdinalIgnoreCase
	);
	private readonly Dictionary<string, IProject> _fileProjectMap = new Dictionary<string, IProject>(
		StringComparer.OrdinalIgnoreCase
	);
	private readonly Dictionary<string, IProject> _folderProjectMap = new Dictionary<string, IProject>(
		StringComparer.OrdinalIgnoreCase
	);
	private Dictionary<string, List<IProject>> _linkedProjectMap = new Dictionary<
		string,
		List<IProject>
	>(StringComparer.OrdinalIgnoreCase);

	private const string VANILLA_PROJECTPATHS_FILENAME = "vanilla_projectpaths.txt";

	private readonly IEnginePathsFacade _pathContext;
	private IPackFileCache _packFileCache = new PackFileConcurrentCache();

	private readonly IFNISParser _fnisParser;

	private const bool CompleteExportSuccess = true;

	public HashSet<IPackFile> ActivePackFiles { get; private set; } = [];

	public ProjectManager(IEnginePathsFacade skyrimPathResolver)
	{
		_pathContext = skyrimPathResolver;
		_fnisParser = new FNISParser(_pathContext, this);
	}

	public void GetExportInfo(StringBuilder builder)
	{
		if (CompleteExportSuccess)
		{
			return;
		}
		builder.AppendLine();

		foreach (var failedPackFile in ActivePackFiles.Where(pf => !pf.ExportSuccess))
		{
			builder.AppendLine(
				$"FATAL ERROR: Could not export {failedPackFile.UniqueName}. Check Engine.log for more information."
			);
		}
	}

	public void GetAnimationInfo(StringBuilder builder)
	{
		var projects = _projectMap.Values;
		uint totalAnimationCount = 0;

		builder.AppendLine();
		foreach (var project in projects)
		{
			Debug.Assert(project.CharacterPackFile is not null, "Project must have a character pack file.");
			var animCount = project.CharacterPackFile.NewAnimationCount;
			if (animCount == 0)
			{
				continue;
			}
			totalAnimationCount += animCount;
			builder.AppendLine($"{animCount} animations added to {project.Identifier}.");
		}
		builder.AppendLine();
		builder.AppendLine($"{totalAnimationCount} total animations added.");
	}

	public void GetFNISInfo(StringBuilder builder)
	{
		uint fnisModCount = 0;
		builder.AppendLine();
		foreach (IModInfo modInfo in _fnisParser.ModInfos)
		{
			fnisModCount++;
			builder.AppendLine($"FNIS Mod {fnisModCount} : {modInfo.Code}");
			Logger.Info($"FNIS Mod {fnisModCount} : {modInfo.Code}");
		}
	}

	public bool TryGetProject(string name, [NotNullWhen(true)] out IProject? project) =>
		_projectMap.TryGetValue(name, out project);

	public bool ProjectExists(string name) => _projectMap.ContainsKey(name);

	public void LoadTrackedProjects()
	{
		FileInfo projectList = new(
			Path.Join(_pathContext.TemplateFolder.FullName, VANILLA_PROJECTPATHS_FILENAME)
		);
		string? expectedLine = null;
		List<string> projectPaths = [];

		try
		{
			using (var readStream = projectList.OpenRead())
			{
				using (var streamReader = new StreamReader(readStream))
				{
					while ((expectedLine = streamReader.ReadLine()) != null)
					{
						if (string.IsNullOrWhiteSpace(expectedLine))
							continue;
						projectPaths.Add(expectedLine);
					}
				}
			}
			LoadProjects(projectPaths);
			Logger.Info($"Loaded {projectPaths.Count} tracked projects from {projectList.Name}");
		}
		catch (FileNotFoundException ex)
		{
			Logger.Error(ex, $"Project list file not found: {projectList.Name}");
		}
		catch (IOException ex)
		{
			Logger.Error(ex, $"I/O error while reading project list file: {projectList.Name}");
		}
		catch (Exception ex)
		{
			Logger.Fatal(
				ex,
				$"Unexpected error while loading tracked projects from {projectList.Name}"
			);
			throw;
		}
	}

	public void LoadProjects(List<string> projectPaths)
	{
		Dictionary<string, IProject> directoryProjectPaths = [];
		foreach (var projectPath in projectPaths)
		{
			var project = LoadProject(projectPath);
			if (project == null || !project.Valid)
				continue;
			ExtractProject(project);
			if (project.ProjectDirectory == null)
				continue;
			var projectFolderPath = project.ProjectDirectory.FullName;
			if (directoryProjectPaths.TryGetValue(projectFolderPath, out var existingProject))
			{
				existingProject.Sibling = project;
				project.Sibling = existingProject;
				project.CharacterPackFile.AttachUniqueAnimationLock(
					project.Sibling.CharacterPackFile
				);
			}
			else
			{
				directoryProjectPaths.Add(projectFolderPath, project);
			}
		}
	}

	public void LoadProjectsParallel(List<string> projectPaths)
	{
		_packFileCache = new PackFileConcurrentCache();
		ConcurrentDictionary<string, IProject> directoryProjectPaths = new();
		Partitioner<string> partitioner = Partitioner.Create(projectPaths, true);
		ParallelOptions options = new() { MaxDegreeOfParallelism = Environment.ProcessorCount / 2 };
		Parallel.ForEach(
			partitioner,
			options,
			projectPath =>
			{
				var project = LoadProject(projectPath);
				if (project == null || !project.Valid)
					return;
				ExtractProject(project);
				if (project.ProjectDirectory == null)
					return;
				var projectFolderPath = project.ProjectDirectory.FullName;
				if (directoryProjectPaths.TryGetValue(projectFolderPath, out var existingProject))
				{
					existingProject.Sibling = project;
					project.Sibling = existingProject;
					Debug.Assert(project.CharacterPackFile is not null,
						"Project must have a character pack file.");
					Debug.Assert(project.Sibling.CharacterPackFile is not null,
						"Sibling project must have a character pack file.");
					project.CharacterPackFile.AttachUniqueAnimationLock(
						project.Sibling.CharacterPackFile
					);
				}
				else
				{
					directoryProjectPaths.TryAdd(projectFolderPath, project);
				}
			}
		);
	}

	public IProject? LoadProject(string projectFilePath)
	{
		if (string.IsNullOrWhiteSpace(projectFilePath))
			return null;

		lock (_projectMap)
		{
			var project = Project.Load(
				new FileInfo(
					Path.Join(_pathContext.TemplateFolder.FullName, projectFilePath)
				),
				_packFileCache
			);

			_projectMap.Add(project.Identifier, project);

			//lock (project) ExtractProject(project);
			return project;
		}
	}

	public IProject? LoadProjectHeader(string projectFilePath)
	{
		if (string.IsNullOrEmpty(projectFilePath))
			return null;
		lock (_projectMap)
		{
			var project = new Project(
				_packFileCache.LoadPackFile(
					new FileInfo(
						Path.Join(_pathContext.GameDataFolder.FullName, projectFilePath)
					)
				)
			);

			_projectMap.Add(project.Identifier, project);

			return project;
		}
	}

	public bool TryLoadOutputPackFile<T>(IPackFile packFile, [NotNullWhen(true)] out T? outPackFile)
		where T : class, IPackFile
	{
		var fileInfo = packFile.GetOutputHandle(_pathContext.GameDataFolder);
		if (!fileInfo.Exists)
		{
			outPackFile = default;
			return false;
		}
		outPackFile = (_packFileCache.LoadPackFile(fileInfo) as T)!;
		return true;
	}

	public bool TryLoadOutputPackFile<T>(
		IPackFile packFile,
		string extension,
		[NotNullWhen(true)] out T? outPackFile
	)
		where T : class, IPackFile
	{
		var fileInfo = new FileInfo(
			Path.ChangeExtension(
				(packFile.GetOutputHandle(_pathContext.GameDataFolder)).FullName,
				extension
			)
		);
		if (!fileInfo.Exists)
		{
			outPackFile = default;
			return false;
		}
		outPackFile = (_packFileCache.LoadPackFile(fileInfo) as T)!;
		return true;
	}

	private void ExtractProject(IProject project)
	{
		lock (_fileProjectMap)
		{
			List<string> fileNames = project.MapFiles(_packFileCache);
			foreach (string file in fileNames)
			{
				_fileProjectMap.TryAdd(file, project);
			}
		}
		lock (_folderProjectMap)
		{
			_folderProjectMap.TryAdd(project.ProjectDirectory!.Name, project);
		}
	}

	private bool TryLookupNestedPackFile(string name, out IPackFile? packFile)
	{
		packFile = null;
		string[] sections = name.Split('~');
		if (!_projectMap.TryGetValue(sections[0], out var project))
		{
			return false;
		}
		return project.TryLookupPackFile(name, out packFile);
	}

	private IPackFile LookupNestedPackFile(string name)
	{
		string[] sections = name.Split('~');

		var targetProject = _projectMap[sections[0]];
		return targetProject.LookupPackFile(sections[1]);
	}

	private bool ContainsNestedPackFile(string name)
	{
		string[] sections = name.Split('~');

		if (!_projectMap.TryGetValue(sections[0], out var targetProject))
			return false;

		return targetProject.ContainsPackFile(sections[1]);
	}

	public bool ProjectLoaded(string name) => _projectMap.ContainsKey(name);

	public IProject LookupProject(string name) => _projectMap[name];

	public bool TryLookupPackFile(string projectName, string packFileName, out IPackFile? packFile)
	{
		packFile = null;
		if (!_projectMap.TryGetValue(projectName, out var project))
		{
			return false;
		}
		return project.TryLookupPackFile(packFileName, out packFile);
	}

	public bool TryLookupPackFileEx(string name, string packFileName, out IPackFile? packFile)
	{
		packFile = TryLookupPackFile(name, packFileName, out var exPackFile)
			? exPackFile as IPackFile
			: null;
		return packFile != null;
	}

	public bool TryLookupPackFile(string name, out IPackFile? packFile)
	{
		name = name.ToLower();
		packFile = null;
		if (name.Contains('~'))
		{
			return TryLookupNestedPackFile(name, out packFile);
		}
		if (!_fileProjectMap.TryGetValue(name, out var project))
		{
			return false;
		}

		return project.TryLookupPackFile(name, out packFile);
	}

	public bool TryLookupProjectFolder(string folderName, out IProject? project)
	{
		return _folderProjectMap.TryGetValue(folderName, out project);
	}

	public bool TryLookupProjectFolderEx(string folderName, out IProject? project)
	{
		project = TryLookupProjectFolder(folderName, out var exProject)
			? exProject as IProject
			: null;
		return project != null;
	}

	public bool TryActivatePackFilePriority(string name, IProject project, out IPackFile? packFile)
	{
		packFile = null;
		if (!project.TryLookupPackFile(name, out packFile))
		{
			return false;
		}
		lock (ActivePackFiles)
			lock (packFile)
			{
				if (ActivePackFiles.Contains(packFile))
				{
					return true;
				}
				packFile.Activate();
				packFile.PopPriorityXmlAsObjects();
				ActivePackFiles.Add(packFile);
			}
		return true;
	}

	public bool TryActivatePackFilePriority(string name, out IPackFile? packFile)
	{
		packFile = null;
		if (!TryLookupPackFile(name, out packFile!))
		{
			return false;
		}
		lock (ActivePackFiles)
			lock (packFile)
			{
				if (ActivePackFiles.Contains(packFile))
				{
					return true;
				}
				packFile.Activate();
				packFile.PopPriorityXmlAsObjects();
				ActivePackFiles.Add(packFile);
			}
		return true;
	}

	public bool TryActivatePackFile(IPackFile packFile)
	{
		lock (ActivePackFiles)
			lock (packFile)
			{
				if (ActivePackFiles.Contains(packFile))
					return true;
				packFile.Activate();
				ActivePackFiles.Add(packFile);
			}
		return false;
	}

	public bool ApplyPatches()
	{
		Parallel.ForEach(
			ActivePackFiles,
			packFile =>
			{
				packFile.ApplyChanges();
				//packFile.Map.Save(Path.Join(Environment.CurrentDirectory, packFile.InputHandle.Name));
			}
		);
		//foreach (PackFile packFile in ActivePackFiles)
		//{
		//	packFile.ApplyChanges();
		//	packFile.Map.Save(Path.Join(Environment.CurrentDirectory, packFile.InputHandle.Name));
		//	packFile.Export();
		//}
		return true;
	}

	public bool ApplyPatchesParallel()
	{
		Parallel.ForEach(
			ActivePackFiles,
			packFile =>
			{
				packFile.ApplyChanges();
			}
		);
		try
		{
			Parallel.ForEach(
				_projectMap.Values,
				project =>
				{
					_fnisParser.ScanProjectAnimlist(project);
				}
			);
			var allAltAnim = new ConcurrentBag<AlternateAnimation>(
				_projectMap.Values.AsParallel().SelectMany(p =>
				{
					_fnisParser.ScanProjectAnimlist(p);
					return p.AlternateAnimations;
				})
			);
			var builder = new AltAnimToOarBuilder(allAltAnim, _pathContext);
			builder.Build();
			builder.PushAAVars(this);
		}
		catch (Exception ex)
		{
			Logger.Error($"FNIS Parser > Scan > Failed > {ex.Message}");
		}
		Parallel.ForEach(
			ActivePackFiles,
			packFile =>
			{
				packFile.PushXmlAsObjects();
			}
		);

		return true;
	}

	public DirectoryInfo GetOutputDirectory() => _pathContext.OutputFolder;
}