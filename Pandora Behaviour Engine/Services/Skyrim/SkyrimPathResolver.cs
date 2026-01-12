using NLog;
using Pandora.API.Services;
using Pandora.Services.CreationEngine;
using Pandora.Utils;
using System;
using System.IO;
using System.Reactive.Subjects;

namespace Pandora.Services.Skyrim;

public sealed class SkyrimPathResolver : IPathResolver
{
	private const string ACTIVE_MODS_FILENAME = "ActiveMods.json";
	private const string PREVIOUS_OUTPUT_FILENAME = "PreviousOutput.txt";
	private const string PANDORA_ENGINE_FOLDERNAME = "Pandora_Engine";

	private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

	private readonly IGameLocator _gameLocator;
	private readonly IGameDescriptor _gameDescriptor;
	private readonly IOutputDirectoryProvider _outputProvider;
	private readonly IPathsConfigService _configService;
	private readonly IApplicationPaths _appPaths;

	private readonly BehaviorSubject<DirectoryInfo> _gameDataSubject;

	public IObservable<DirectoryInfo> GameDataFolderChanged => _gameDataSubject;

	private DirectoryInfo? _cachedGameDir;
	private DirectoryInfo? _cachedOutputDir;

	private DirectoryInfo? _cachedPandoraEngineDir;

	private readonly DirectoryInfo _currentDir;
	private readonly Lazy<DirectoryInfo> _templateDir;

	public SkyrimPathResolver(
		IGameLocator gameLocator,
		IGameDescriptor gameDescriptor,
		IOutputDirectoryProvider outputProvider,
		IPathsConfigService configService,
		IApplicationPaths appPaths)
	{
		_gameLocator = gameLocator;
		_gameDescriptor = gameDescriptor;
		_outputProvider = outputProvider;
		_configService = configService;
		_appPaths = appPaths;

		_currentDir = new DirectoryInfo(Environment.CurrentDirectory);

		_templateDir = new Lazy<DirectoryInfo>(ResolveTemplateDir);

		_gameDataSubject = new BehaviorSubject<DirectoryInfo>(GetGameDataFolder());
	}

	public DirectoryInfo GetGameDataFolder()
	{
		_cachedGameDir ??= ResolveGameData();
		return _cachedGameDir;
	}

	public void SetGameDataFolder(DirectoryInfo newFolder)
	{
		if (!newFolder.Exists) return;

		Logger.Info($"Game Data directory changed to: {newFolder.FullName}");

		_cachedGameDir = newFolder;
		_configService.UpdatePaths(_gameDescriptor.Id, _cachedGameDir, _cachedOutputDir);

		_gameDataSubject.OnNext(newFolder);
	}

	private DirectoryInfo ResolveGameData()
	{
		var dir = _gameLocator.TryLocateGameData();
		if (dir is not null)
		{
			Logger.Info($"Skyrim Data directory found: {dir.FullName}");
			return dir;
		}

		Logger.Warn("Skyrim Data directory not found. Falling back to assembly directory.");
		return _appPaths.AssemblyDirectory;
	}


	public DirectoryInfo GetOutputFolder()
	{
		_cachedOutputDir ??= ResolveOutput();
		return _cachedOutputDir;
	}

	public void SetOutputFolder(DirectoryInfo newFolder)
	{
		Logger.Info($"Output directory changed to: {newFolder.FullName}");

		_cachedOutputDir = newFolder;
		_configService.UpdatePaths(_gameDescriptor.Id, _cachedGameDir, _cachedOutputDir);

		if (!newFolder.Exists) 
			newFolder.Create();

		_cachedPandoraEngineDir = null;
	}

	private DirectoryInfo ResolveOutput() =>
		_outputProvider.TryGetOutputDirectory(_gameDescriptor.Id)
			?? GetGameDataFolder()
			?? _appPaths.AssemblyDirectory;


	public DirectoryInfo GetPandoraEngineFolder()
	{
		if (_cachedPandoraEngineDir == null)
		{
			var path = GetOutputFolder().FullName / PANDORA_ENGINE_FOLDERNAME;
			_cachedPandoraEngineDir = new DirectoryInfo(path);
			
			if (!_cachedPandoraEngineDir.Exists) 
				_cachedPandoraEngineDir.Create();
		}
		return _cachedPandoraEngineDir;
	}

	public FileInfo GetActiveModsFile() =>
		new(GetPandoraEngineFolder().FullName / ACTIVE_MODS_FILENAME);

	public FileInfo GetPreviousOutputFile() =>
		new(GetPandoraEngineFolder().FullName / PREVIOUS_OUTPUT_FILENAME);

	public DirectoryInfo GetOutputMeshFolder() =>
		new(GetOutputFolder().FullName / "meshes");


	public DirectoryInfo GetAssemblyFolder() => _appPaths.AssemblyDirectory;
	public DirectoryInfo GetCurrentFolder() => _currentDir;

	private DirectoryInfo ResolveTemplateDir() =>
		new(_appPaths.AssemblyDirectory.FullName / PANDORA_ENGINE_FOLDERNAME / "Skyrim" / "Template");

	public DirectoryInfo GetTemplateFolder() => _templateDir.Value;
}