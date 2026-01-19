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
	private readonly IApplicationPaths _appPaths;
	private readonly IUserPaths _userPaths;
	private readonly IGameDescriptor _descriptor;

	private PathsConfiguration _fullConfiguration = new();
	private UserSettings _currentSettings = new();

	public bool IsGameDataValid =>
		!_userPaths.GameData.FullName.Equals(_appPaths.AssemblyDirectory.FullName, StringComparison.OrdinalIgnoreCase)
		&& _validator.IsValid(_userPaths.GameData);

	public bool NeedsUserSelection => !IsGameDataValid;

	public SettingsService(
		ISettingsRepository repository,
		IGameLocator gameLocator,
		IGameDataValidator validator,
		IApplicationPaths appPaths,
		IUserPaths userPaths,
		IGameDescriptor descriptor)
	{
		_repository = repository;
		_gameLocator = gameLocator;
		_validator = validator;
		_appPaths = appPaths;
		_userPaths = userPaths;
		_descriptor = descriptor;
	}

	public void Initialize()
	{
		_fullConfiguration = _repository.Load();

		if (!_fullConfiguration.TryGetValue(_descriptor.Id, out _currentSettings!))
		{
			_currentSettings = new UserSettings();
			_fullConfiguration[_descriptor.Id] = _currentSettings;
		}

		// 1. GAME DATA
		DirectoryInfo resolvedGameData;

		if (!string.IsNullOrEmpty(_currentSettings.GameDataPath))
		{
			var configured = new DirectoryInfo(_currentSettings.GameDataPath);
			var normalized = _validator.Normalize(configured);

			if (normalized != null)
			{
				resolvedGameData = normalized;
			}
			else
			{
				resolvedGameData = ResolveViaLocators();
			}
		}
		else
		{
			resolvedGameData = ResolveViaLocators();
		}
		_userPaths.SetGameData(resolvedGameData);

		// 2. OUTPUT
		DirectoryInfo resolvedOutput;

		if (!string.IsNullOrEmpty(_currentSettings.OutputPath)
			&& new DirectoryInfo(_currentSettings.OutputPath).Exists)
		{
			resolvedOutput = new DirectoryInfo(_currentSettings.OutputPath);
		}
		else
		{
			resolvedOutput = resolvedGameData;
		}
		_userPaths.SetOutput(resolvedOutput);
	}

	private DirectoryInfo ResolveViaLocators()
	{
		var located = _gameLocator.TryLocateGameData();

		if (located != null)
		{
			var normalized = _validator.Normalize(located);
			if (normalized != null)
			{
				_currentSettings.GameDataPath = normalized.FullName;
				SaveState();
				return normalized;
			}
		}

		// fallback
		return _appPaths.AssemblyDirectory;
	}


	public void SetGameDataFolder(DirectoryInfo dir)
	{
		var normalized = _validator.Normalize(dir);
		if (normalized is null)
		{
			logger.UiWarn("Selected folder is not a valid game directory.");
			return;
		}

		_userPaths.SetGameData(normalized);
		_currentSettings.GameDataPath = normalized.FullName;

		if (string.IsNullOrEmpty(_currentSettings.OutputPath))
		{
			_userPaths.SetOutput(normalized);
		}

		SaveState();
	}


	public void SetOutputFolder(DirectoryInfo dir)
	{
		if (!dir.Exists) dir.Create();

		_userPaths.SetOutput(dir);

		_currentSettings.OutputPath = dir.FullName;

		SaveState();
	}

	private void SaveState()
	{
		_fullConfiguration[_descriptor.Id] = _currentSettings;

		_repository.Save(_fullConfiguration);
	}
}