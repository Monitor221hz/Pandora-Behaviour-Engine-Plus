// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using GameFinder.Common;
using GameFinder.RegistryUtils;
using GameFinder.StoreHandlers.GOG;
using GameFinder.StoreHandlers.Steam;
using GameFinder.StoreHandlers.Steam.Models.ValueTypes;
using NexusMods.Paths;
using NLog;
using Pandora.API.Utils;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace Pandora.Utils.Skyrim;

/// <summary>
/// Resolves the installation path of Skyrim Special Edition, prioritizing a manually specified path via command-line arguments.
/// If not provided, it queries the Windows Registry for the installation path.
/// </summary>
/// <returns>
/// Returns the "Data" directory within the installation folder if found; otherwise <see cref="Environment.CurrentDirectory"/>.
/// </returns>
public sealed class SkyrimPathResolver : IPathResolver
{
	private const string ACTIVE_MODS_FILENAME = "ActiveMods.json";
	private const string PATH_CONFIG_FILENAME = "Paths.json";
	private const string PREVIOUS_OUTPUT_FILENAME = "PreviousOutput.txt";
	private const string PANDORA_ENGINE_FOLDERNAME = "Pandora_Engine";

	private readonly AHandler<SteamGame, AppId> _steamHandler;
	private readonly AHandler<GOGGame, GOGGameId> _gogHandler;

	private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

	private delegate DirectoryInfo? PathProvider();

	private DirectoryInfo _assemblyDirectory = new(
		Path.GetDirectoryName(Process.GetCurrentProcess().MainModule?.FileName)
			?? throw new NullReferenceException("Main process not found.")
	);
	private DirectoryInfo _currentDirectory;

	private Lazy<DirectoryInfo> _gameDirectory;
	private Lazy<DirectoryInfo> _outputDirectory;

	private readonly Lazy<DirectoryInfo> _templateDirectory;
	private readonly Lazy<DirectoryInfo> _outputMeshDirectory;
	private readonly Lazy<DirectoryInfo> _pandoraEngineDirectory;

	private readonly Lazy<FileInfo> _activeModsFile;
	private readonly Lazy<FileInfo> _previousOutputFile;
	private readonly Lazy<FileInfo> _pathsConfigFile;

	private Lazy<SkyrimPathsConfiguration?> _pathsConfiguration;

	private readonly JsonSerializerOptions _jsonSerializerOptions;

	private readonly uint[] steamAppIDs = new uint[]
	{
		489830, // SSE/AE (Steam)
		611670, // VR (Steam)
	};

	private readonly uint gogAppID = 711230643; // SSE/AE (GOG)

	private readonly IRegistry? _registry;
	private readonly IFileSystem _fileSystem;

	public SkyrimPathResolver(
		IRegistry? registry,
		IFileSystem fileSystem,
		AHandler<SteamGame, AppId> steamHandler,
		AHandler<GOGGame, GOGGameId> gogHandler
	)
	{
		_steamHandler = steamHandler;
		_gogHandler = gogHandler;
		_fileSystem = fileSystem;
		_registry = registry;
		_currentDirectory = new(Environment.CurrentDirectory);
		_pathsConfiguration = new(() => ResolvePathsConfiguration());
		_gameDirectory = new(() => ResolveGameDataDirectory());
		_templateDirectory = new(() => ResolveTemplateDirectory());
		_outputDirectory = new(() => ResolveOutputDirectory());
		_outputMeshDirectory = new(() => ResolveOutputMeshDirectory());
		_pandoraEngineDirectory = new(() => ResolvePandoraEngineDirectory());
		_activeModsFile = new(() => ResolveActiveModsFile());
		_previousOutputFile = new(() => ResolvePreviousOutputFile());
		_pathsConfigFile = new(() => ResolvePathsConfigFile());

		_jsonSerializerOptions = new JsonSerializerOptions() { WriteIndented = true };
		_jsonSerializerOptions.Converters.Add(Converters.DirectoryInfoJsonConverter.Instance);
		_jsonSerializerOptions.Converters.Add(Converters.FileInfoJsonConverter.Instance);
	}

	public void SetGameDataFolder(DirectoryInfo gameDataFolder)
	{
		_gameDirectory = new(() => gameDataFolder);
		if (_pathsConfiguration.Value == null)
		{
			_pathsConfiguration = new(() => new SkyrimPathsConfiguration());
		}
		_pathsConfiguration.Value!.GameDataDirectory = gameDataFolder;
		_gameDirectory = new(() => ResolveGameDataDirectory());
	}

	public void SetOutputFolder(DirectoryInfo outputFolder)
	{
		_outputDirectory = new(() => outputFolder);
		if (_pathsConfiguration.Value == null)
		{
			_pathsConfiguration = new(() => new SkyrimPathsConfiguration());
		}
		_pathsConfiguration.Value!.OutputDirectory = outputFolder;
		_outputDirectory = new(() => ResolveOutputDirectory());
	}

	public void SavePathsConfiguration()
	{
		if (_pathsConfiguration.Value == null)
		{
			return;
		}
		var configFile = _pathsConfigFile.Value;
		using var stream = configFile.Create();
		JsonSerializer.Serialize<SkyrimPathsConfiguration>(
			stream,
			_pathsConfiguration.Value!,
			_jsonSerializerOptions
		);
		_pathsConfiguration = new(() => ResolvePathsConfiguration());
	}

	public DirectoryInfo ResolvePandoraEngineDirectory()
	{
		var dir = new DirectoryInfo(
			Path.Combine(GetGameDataFolder().FullName, PANDORA_ENGINE_FOLDERNAME)
		);
		dir.Create();
		return dir;
	}

	public DirectoryInfo GetGameDataFolder()
	{
		return _gameDirectory.Value;
	}

	private FileInfo ResolvePreviousOutputFile()
	{
		return new FileInfo(Path.Join(GetPandoraEngineFolder().FullName, PREVIOUS_OUTPUT_FILENAME));
	}

	private FileInfo ResolveActiveModsFile()
	{
		return new FileInfo(Path.Join(GetPandoraEngineFolder().FullName, ACTIVE_MODS_FILENAME));
	}

	private DirectoryInfo ResolveOutputMeshDirectory()
	{
		return new DirectoryInfo(Path.Join(GetOutputFolder().FullName, "meshes"));
	}

	private FileInfo ResolvePathsConfigFile()
	{
		return new FileInfo(
			Path.Join(GetAssemblyFolder().FullName, PANDORA_ENGINE_FOLDERNAME, PATH_CONFIG_FILENAME)
		);
	}

	/// <summary>
	/// Resolves the installation path of Skyrim Special Edition by checking command-line arguments first,
	/// then falling back to the Windows Registry if no path is specified.
	/// </summary>
	/// <returns>
	/// A <see cref="DirectoryInfo"/> representing the "Data" directory of the Skyrim Special Edition installation,
	/// otherwise <see cref="Environment.CurrentDirectory"/>.
	/// </returns>
	private DirectoryInfo ResolveGameDataDirectory()
	{
		PathProvider[] providers =
		[
			TryGetDataPathFromPathsConfiguration,
			TryGetDataPathFromCommandLine,
			TryGetDataPathFromGameFinder, // add file dialog after this; file dialog must not fail!
			TryGetDataPathFromCurrentDirectory,
		];

		var resolvedPath = providers
			.Select(provider => provider())
			.FirstOrDefault(IsValidSkyrimDataDirectory);

		if (resolvedPath is not null)
		{
			Logger.Info($"Found valid Skyrim 'Data' directory at: {resolvedPath.FullName}");
			return resolvedPath;
		}

		string msg =
			$"Could not find a valid Skyrim 'Data' directory. Using directory {_assemblyDirectory.FullName}";
		Logger.UiWarn($"WARN: {msg}");

		Logger.Warn(msg);
		return _assemblyDirectory!;
	}

	private DirectoryInfo ResolveTemplateDirectory()
	{
		return new(Path.Join(_assemblyDirectory.FullName, "Pandora_Engine", "Skyrim", "Template"));
	}

	private DirectoryInfo ResolveOutputDirectory()
	{
		return LaunchOptions.Current?.OutputDirectory
			?? TryGetOutputPathFromPathsConfiguration()
			?? GetGameDataFolder()
			?? GetAssemblyFolder();
	}

	private DirectoryInfo? TryGetDataPathFromPathsConfiguration()
	{
		return _pathsConfiguration.Value?.GameDataDirectory;
	}

	private DirectoryInfo? TryGetOutputPathFromPathsConfiguration()
	{
		return _pathsConfiguration.Value?.OutputDirectory;
	}

	/// <summary>
	/// Attempts to retrieve the Skyrim Special Edition installation path from command-line arguments
	/// specified in <see cref="LaunchOptions.Current.SkyrimGameDirectory"/>.
	/// </summary>
	/// <returns>
	/// A <see cref="DirectoryInfo"/> representing the "Data" directory within the specified path if provided;
	/// otherwise, null if no path is specified in the command-line arguments.
	/// </returns>
	private static DirectoryInfo? TryGetDataPathFromCommandLine()
	{
		var gameDir = LaunchOptions.Current?.SkyrimGameDirectory;
		if (gameDir is not null)
		{
			Logger.Debug(
				"Attempting Skyrim path from command line args (--tesv): {0}",
				gameDir.FullName
			);
			var abstractGameDir = new DirectoryInfo(gameDir.FullName);
			return NormalizeToDataDirectory(abstractGameDir);
		}
		Logger.Debug("No Skyrim path in command line args.");
		return null;
	}

	/// <summary>
	/// Try to get path from current working directory.
	/// This is needed in case the path of the current directory coincides with the directory to Data.
	/// Such cases are set via MO2/Vortex using Start In.
	/// </summary>
	/// <returns>
	/// A <see cref="DirectoryInfo"/> representing the "Data" directory within the installation folder if found;
	/// otherwise <see cref="Environment.CurrentDirectory"/>.
	/// </returns>
	private DirectoryInfo? TryGetDataPathFromCurrentDirectory()
	{
		Logger.Debug(
			"Attempting Skyrim path from current directory: {0}",
			_currentDirectory.FullName
		);
		return NormalizeToDataDirectory(_currentDirectory);
	}

	private DirectoryInfo? TryGetDataPathFromGameFinder()
	{
		SteamHandler steamHandler = new SteamHandler(_fileSystem, _registry);
		foreach (var appId in steamAppIDs)
		{
			var game = steamHandler.FindOneGameById(AppId.From(appId), out var steamErrors);
			if (game == null || !game.Path.DirectoryExists())
				continue;
			return NormalizeToDataDirectory(new DirectoryInfo(game.Path.GetFullPath()));
		}
		if (_registry == null)
		{
			return null;
		}
		var gogHandler = new GOGHandler(_registry, _fileSystem);
		var gogGame = gogHandler.FindOneGameById(GOGGameId.From(gogAppID), out var gogErrors);
		if (gogGame == null || !gogGame.Path.DirectoryExists())
		{
			return null;
		}
		return NormalizeToDataDirectory(new DirectoryInfo(gogGame.Path.GetFullPath()));
	}

	private SkyrimPathsConfiguration? ResolvePathsConfiguration()
	{
		var configFile = _pathsConfigFile.Value;
		if (!configFile.Exists)
			return null;
		using var stream = configFile.OpenRead();
		return JsonSerializer.Deserialize<SkyrimPathsConfiguration>(stream, _jsonSerializerOptions);
	}

	private bool IsValidSkyrimDataDirectory([NotNullWhen(true)] DirectoryInfo? dataDirectory)
	{
		if (dataDirectory is null || !dataDirectory.Exists)
			return false;

		if (!dataDirectory.Name.Equals("Data", StringComparison.OrdinalIgnoreCase))
			return false;

		var gameRoot = dataDirectory.Parent;
		if (gameRoot is null || !gameRoot.Exists)
			return false;

		var exePath = Path.Combine(gameRoot.FullName, "SkyrimSE.exe");
		var launcherPath = Path.Combine(gameRoot.FullName, "SkyrimSELauncher.exe");
		var vrPath = Path.Combine(gameRoot.FullName, "SkyrimVR.exe");

		return File.Exists(exePath) || File.Exists(launcherPath) || File.Exists(vrPath);
	}

	private static DirectoryInfo NormalizeToDataDirectory(DirectoryInfo directory)
	{
		if (directory.Name.Equals("Data", StringComparison.OrdinalIgnoreCase))
			return directory;

		return new DirectoryInfo(Path.Combine(directory.FullName, "Data"));
	}

	public DirectoryInfo GetCurrentFolder() => _currentDirectory;

	public DirectoryInfo GetTemplateFolder() => _templateDirectory.Value;

	public DirectoryInfo GetOutputFolder() => _outputDirectory.Value;

	public DirectoryInfo GetOutputMeshFolder() => _outputMeshDirectory.Value;

	public DirectoryInfo GetPandoraEngineFolder() => _pandoraEngineDirectory.Value;

	public DirectoryInfo GetAssemblyFolder() => _assemblyDirectory;

	public FileInfo GetActiveModsFile() => _activeModsFile.Value;

	public FileInfo GetPreviousOutputFile() => _previousOutputFile.Value;
}
