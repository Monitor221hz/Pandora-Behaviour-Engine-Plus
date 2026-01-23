using Pandora.Logging.Extensions;
using Pandora.Paths.Abstractions;
using Pandora.Paths.Configuration.DTOs;
using Pandora.Paths.Validation;
using Pandora.Platform.CreationEngine;
using System;
using System.IO;

namespace Pandora.Settings;

public class SettingsService(
	ISettingsRepository repository,
	IGameLocator gameLocator,
	IGameDataValidator validator,
	IGameDescriptor descriptor,
	IApplicationPaths appPaths,
	IUserPaths userPaths) : ISettingsService
{
	private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

	private PathsConfiguration _fullConfiguration = null!;
	private UserSettings _currentSettings = null!;

	public bool IsGameDataValid =>
		!userPaths.GameData.FullName.Equals(appPaths.AssemblyDirectory.FullName, StringComparison.OrdinalIgnoreCase)
		&& validator.IsValid(userPaths.GameData);

	public bool NeedsUserSelection => !IsGameDataValid;

	public void Initialize()
	{
		LoadConfiguration();
		DirectoryInfo gameDataDirectory = ResolveGameDataDirectory();
		userPaths.SetGameData(gameDataDirectory);

		DirectoryInfo outputDirectory = ResolveOutputDirectory(gameDataDirectory);
		userPaths.SetOutput(outputDirectory);

		SaveState();
	}

	private void LoadConfiguration()
	{
		_fullConfiguration = repository.Load() ?? [];

		if (_fullConfiguration.TryGetValue(descriptor.Id, out var loadedSettings))
		{
			_currentSettings = loadedSettings;
		}
		else
		{
			_currentSettings = new UserSettings();
			_fullConfiguration[descriptor.Id] = _currentSettings;
		}
	}

	private DirectoryInfo ResolveGameDataDirectory()
	{
		if (!string.IsNullOrEmpty(_currentSettings.GameDataPath))
		{
			var configuredDirectory = new DirectoryInfo(_currentSettings.GameDataPath);
			var normalizedDirectory = validator.Normalize(configuredDirectory);

			if (normalizedDirectory != null)
			{
				return normalizedDirectory;
			}
		}

		return ResolveViaLocators() ?? appPaths.AssemblyDirectory;
	}

	private DirectoryInfo ResolveOutputDirectory(DirectoryInfo gameDataDirectory)
	{
		if (!string.IsNullOrEmpty(_currentSettings.OutputPath))
		{
			var outputDirectory = new DirectoryInfo(_currentSettings.OutputPath);
			if (outputDirectory.Exists)
			{
				return outputDirectory;
			}
		}

		return gameDataDirectory;
	}

	private DirectoryInfo? ResolveViaLocators()
	{
		var locatedDirectory = gameLocator.TryLocateGameData();
		if (locatedDirectory == null)
		{
			return null;
		}

		var normalizedDirectory = validator.Normalize(locatedDirectory);
		if (normalizedDirectory != null)
		{
			_currentSettings.GameDataPath = normalizedDirectory.FullName;
		}

		return normalizedDirectory;
	}

	public void SetGameDataFolder(DirectoryInfo directory)
	{
		var normalizedDirectory = validator.Normalize(directory);
		if (normalizedDirectory == null)
		{
			logger.UiWarn("Selected folder is not a valid game directory.");
			throw new InvalidOperationException("Invalid game data directory selected.");
		}

		userPaths.SetGameData(normalizedDirectory);
		_currentSettings.GameDataPath = normalizedDirectory.FullName;

		if (string.IsNullOrEmpty(_currentSettings.OutputPath))
		{
			userPaths.SetOutput(normalizedDirectory);
		}

		SaveState();
	}

	public void SetOutputFolder(DirectoryInfo directory)
	{
		if (!directory.Exists)
		{
			directory.Create();
		}

		userPaths.SetOutput(directory);
		_currentSettings.OutputPath = directory.FullName;

		SaveState();
	}

	private void SaveState()
	{
		_fullConfiguration[descriptor.Id] = _currentSettings;
		repository.Save(_fullConfiguration);
	}
}