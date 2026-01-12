using GameFinder.RegistryUtils;
using NexusMods.Paths;
using Pandora.Utils;
using System;
using System.IO;

namespace Pandora.Services.CreationEngine.Locators;

public sealed class RegistryGameLocator : IGameLocator
{
	private readonly IGameDescriptor _gameDescriptor;
	private readonly IRegistry? _registry;
	private readonly IFileSystem _fileSystem;

	public RegistryGameLocator(IGameDescriptor gameDescriptor, IRegistry? registry, IFileSystem fileSystem)
	{
		_gameDescriptor = gameDescriptor;
		_registry = registry;
		_fileSystem = fileSystem;
	}

	public DirectoryInfo? TryLocateGameData()
	{
		if (!OperatingSystem.IsWindows() || _registry is null)
			return null;

		var key = _registry.OpenBaseKey(RegistryHive.LocalMachine).OpenSubKey(_gameDescriptor.SubKey);
		if (key == null)
			return null;

		if (key.GetValue("Installed Path") is not string installPath)
			return null;

		var game = _fileSystem.FromUnsanitizedFullPath(installPath);
		if (!game.DirectoryExists())
			return null;

		var dirInfo = new DirectoryInfo(game.GetFullPath());

		return GamePathUtils.NormalizeToDataDirectory(dirInfo, _gameDescriptor);
	}
}

