using Pandora.Logging.Extensions;
using Pandora.Paths.Abstractions;
using Pandora.Paths.Configuration.DTOs;
using Pandora.Paths.Validation;
using Pandora.Platform.CreationEngine;
using System;
using System.IO;

namespace Pandora.Services.Settings;

public class SettingsService : ISettingsService
{
	private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

	private readonly ISettingsRepository _repository;
	private readonly IGameLocator _gameLocator;
	private readonly IGameDataValidator _validator;
	private readonly IGameDescriptor _descriptor;
	private readonly IApplicationPaths _appPaths;
	private readonly IUserPaths _userPaths;

	private PathsConfiguration _fullConfiguration;
	private UserSettings _currentSettings;

	public bool IsGameDataValid =>
		!_userPaths.GameData.FullName.Equals(_appPaths.AssemblyDirectory.FullName, StringComparison.OrdinalIgnoreCase)
		&& _validator.IsValid(_userPaths.GameData);

	public bool NeedsUserSelection => !IsGameDataValid;

	public SettingsService(
		ISettingsRepository repository,
		IGameLocator gameLocator,
		IGameDataValidator validator,
		IGameDescriptor descriptor,
		IApplicationPaths appPaths,
		IUserPaths userPaths)
	{
		_repository = repository;
		_gameLocator = gameLocator;
		_validator = validator;
		_descriptor = descriptor;
		_appPaths = appPaths;
		_userPaths = userPaths;
	}

	public void Initialize()
	{
		LoadConfiguration();
		DirectoryInfo gameDataDirectory = ResolveGameDataDirectory();
		_userPaths.SetGameData(gameDataDirectory);

		DirectoryInfo outputDirectory = ResolveOutputDirectory(gameDataDirectory);
		_userPaths.SetOutput(outputDirectory);

		SaveState();
	}

	private void LoadConfiguration()
	{
		_fullConfiguration = _repository.Load() ?? new PathsConfiguration();

		if (!_fullConfiguration.TryGetValue(_descriptor.Id, out _currentSettings))
		{
			_currentSettings = new UserSettings();
			_fullConfiguration[_descriptor.Id] = _currentSettings;
		}
	}

	private DirectoryInfo ResolveGameDataDirectory()
	{
		if (!string.IsNullOrEmpty(_currentSettings.GameDataPath))
		{
			var configuredDirectory = new DirectoryInfo(_currentSettings.GameDataPath);
			var normalizedDirectory = _validator.Normalize(configuredDirectory);

			if (normalizedDirectory != null)
			{
				return normalizedDirectory;
			}
		}

		return ResolveViaLocators() ?? _appPaths.AssemblyDirectory;
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
		var locatedDirectory = _gameLocator.TryLocateGameData();
		if (locatedDirectory == null)
		{
			return null;
		}

		var normalizedDirectory = _validator.Normalize(locatedDirectory);
		if (normalizedDirectory != null)
		{
			_currentSettings.GameDataPath = normalizedDirectory.FullName;
		}

		return normalizedDirectory;
	}

	public void SetGameDataFolder(DirectoryInfo directory)
	{
		var normalizedDirectory = _validator.Normalize(directory);
		if (normalizedDirectory == null)
		{
			logger.UiWarn("Selected folder is not a valid game directory.");
			throw new InvalidOperationException("Invalid game data directory selected.");
		}

		_userPaths.SetGameData(normalizedDirectory);
		_currentSettings.GameDataPath = normalizedDirectory.FullName;

		if (string.IsNullOrEmpty(_currentSettings.OutputPath))
		{
			_userPaths.SetOutput(normalizedDirectory);
		}

		SaveState();
	}

	public void SetOutputFolder(DirectoryInfo directory)
	{
		if (!directory.Exists)
		{
			directory.Create();
		}

		_userPaths.SetOutput(directory);
		_currentSettings.OutputPath = directory.FullName;

		SaveState();
	}

	private void SaveState()
	{
		_fullConfiguration[_descriptor.Id] = _currentSettings;
		_repository.Save(_fullConfiguration);
	}
}