// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using GameFinder.RegistryUtils;
using GameFinder.StoreHandlers.GOG;
using GameFinder.StoreHandlers.Steam;
using GameFinder.StoreHandlers.Steam.Models.ValueTypes;
using NexusMods.Paths;
using NLog;
using Pandora.Logging;

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
	private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

	private delegate DirectoryInfo? PathProvider();

	private DirectoryInfo _assemblyDirectory = new(
		Path.GetDirectoryName(Process.GetCurrentProcess().MainModule?.FileName)
			?? throw new NullReferenceException("Main process not found.")
	);
	private DirectoryInfo _currentDirectory;
	private DirectoryInfo? _gameDirectory;
	private DirectoryInfo? _templateDirectory;
	private DirectoryInfo? _outputDirectory;

	private readonly uint[] steamAppIDs = new uint[]
	{
		489830, // SSE/AE (Steam)
		611670, // VR (Steam)
	};

	private readonly uint gogAppID = 711230643; // SSE/AE (GOG)

	private readonly IRegistry? _registry;
	private readonly IFileSystem _fileSystem;

	public SkyrimPathResolver(IRegistry? registry, IFileSystem fileSystem)
	{
		_fileSystem = fileSystem;
		_registry = registry;
		_currentDirectory = new(Environment.CurrentDirectory);
	}

	public DirectoryInfo GetGameDataFolder()
	{
		return _gameDirectory ?? ResolveGameDataDirectory();
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
			TryGetDataPathFromCommandLine,
			TryGetDataPathFromCurrentDirectory,
			() => TryGetDataPathFromGameFinder(), // add file dialog later
		];

		var resolvedPath = providers
			.Select(provider => provider())
			.FirstOrDefault(IsValidSkyrimDataDirectory);

		if (resolvedPath is not null)
		{
			Logger.Info($"Found valid Skyrim 'Data' directory at: {resolvedPath.FullName}");
			_gameDirectory = resolvedPath;
			return resolvedPath;
		}

		string msg =
			$"Could not find a valid Skyrim 'Data' directory. Current directory {Environment.CurrentDirectory}";
		EngineLoggerAdapter.AppendLine($"WARN: {msg}");
		Logger.Warn(msg);
		return _currentDirectory;
	}

	private DirectoryInfo ResolveTemplateDirectory()
	{
		_templateDirectory = new(
			Path.Join(_assemblyDirectory.FullName, "Pandora_Engine", "Skyrim", "Template")
		);
		return _templateDirectory;
	}

	private DirectoryInfo ResolveOutputDirectory()
	{
		throw new NotImplementedException();
	}

	/// <summary>
	/// Attempts to retrieve the Skyrim Special Edition installation path from command-line arguments
	/// specified in <see cref="LaunchOptions.Current.SkyrimGameDirectory"/>.
	/// </summary>
	/// <returns>
	/// A <see cref="DirectoryInfo"/> representing the "Data" directory within the specified path if provided;
	/// otherwise, null if no path is specified in the command-line arguments.
	/// </returns>
	private DirectoryInfo? TryGetDataPathFromCommandLine()
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
			return NormalizeToDataDirectory(new DirectoryInfo(game.Path.Directory));
		}
		if (_registry == null)
		{
			return null;
		}
		var gogHandler = new GOGHandler(_registry, _fileSystem);
		var gogGame = gogHandler.FindOneGameById(GOGGameId.From(711230643), out var gogErrors);
		if (gogGame == null || !gogGame.Path.DirectoryExists())
		{
			return null;
		}
		return NormalizeToDataDirectory(new DirectoryInfo(gogGame.Path.Directory));
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

		return File.Exists(exePath) || File.Exists(launcherPath);
	}

	private DirectoryInfo NormalizeToDataDirectory(DirectoryInfo directory)
	{
		if (directory.Name.Equals("Data", StringComparison.OrdinalIgnoreCase))
			return directory;

		return new DirectoryInfo(Path.Combine(directory.FullName, "Data"));
	}

	public DirectoryInfo GetTemplateFolder() => _templateDirectory ??= ResolveTemplateDirectory();

	public DirectoryInfo GetOutputFolder() => _outputDirectory ??= ResolveOutputDirectory();

	public DirectoryInfo GetOutputMeshFolder() =>
		new(Path.Join(GetOutputFolder().FullName, "meshes"));
}
