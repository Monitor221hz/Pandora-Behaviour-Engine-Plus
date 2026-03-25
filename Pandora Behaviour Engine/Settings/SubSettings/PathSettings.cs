// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using Pandora.Logging.Extensions;
using Pandora.Paths.Abstractions;
using Pandora.Paths.Validation;
using Pandora.Platform.CreationEngine;
using Pandora.Settings.DTOs;
using System;
using System.IO;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Pandora.Settings.SubSettings;

internal sealed class PathSettings(
	IGameLocator gameLocator,
	IGameDataValidator validator,
	IApplicationPaths appPaths,
	IUserPaths userPaths) : IPathSettings
{
	private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
	private GameSettings _gameSettings = null!;

	public IObservable<DirectoryInfo> GameDataChanged => userPaths.GameDataChanged;
	public IObservable<DirectoryInfo> OutputChanged => userPaths.OutputChanged;

	private readonly Subject<Unit> _saveRequestSubject = new();
	public IObservable<Unit> SaveRequired => _saveRequestSubject.AsObservable();

	public DirectoryInfo GameData => userPaths.GameData;
	public DirectoryInfo Output => userPaths.Output;

	public bool IsGameDataValid =>
		!GameData.FullName.Equals(appPaths.AssemblyDirectory.FullName, StringComparison.OrdinalIgnoreCase)
		&& validator.IsValid(GameData);

	public bool NeedsUserSelection => !IsGameDataValid;

	public void Initialize(GameSettings gameSettings)
	{
		_gameSettings = gameSettings;
		RefreshPaths();
	}

	private void RefreshPaths()
	{
		bool changed = false;
		var gameData = ResolveGameDataDirectory();
		userPaths.SetGameData(gameData);

		if (_gameSettings.GameDataPath != gameData.FullName)
		{
			_gameSettings.GameDataPath = gameData.FullName;
			changed = true;
		}

		var output = ResolveOutputDirectory(gameData);
		userPaths.SetOutput(output);

		if (_gameSettings.OutputPath != output.FullName)
		{
			_gameSettings.OutputPath = output.FullName;
			changed = true;
		}

		if (changed)
		{
			_saveRequestSubject.OnNext(Unit.Default);
		}
	}

	private DirectoryInfo ResolveGameDataDirectory()
	{
		if (!string.IsNullOrEmpty(_gameSettings.GameDataPath))
		{
			var configured = new DirectoryInfo(_gameSettings.GameDataPath);
			var normalized = validator.Normalize(configured);
			if (normalized != null) return normalized;
		}

		var located = gameLocator.TryLocateGameData();
		if (located != null)
		{
			var normalized = validator.Normalize(located);
			if (normalized != null) return normalized;
		}

		return appPaths.AssemblyDirectory;
	}

	private DirectoryInfo ResolveOutputDirectory(DirectoryInfo gameData)
	{
		if (!string.IsNullOrEmpty(_gameSettings.OutputPath))
		{
			var output = new DirectoryInfo(_gameSettings.OutputPath);
			if (output.Exists) return output;
		}
		return gameData;
	}

	public void SetGameDataFolder(DirectoryInfo directory)
	{
		var normalized = validator.Normalize(directory);
		if (normalized == null)
		{
			logger.UiWarn("Selected folder is not a valid game directory.");
			throw new InvalidOperationException("Invalid game data directory.");
		}

		userPaths.SetGameData(normalized);
		_gameSettings.GameDataPath = normalized.FullName;

		if (string.IsNullOrEmpty(_gameSettings.OutputPath) || _gameSettings.OutputPath == _gameSettings.GameDataPath)
		{
			userPaths.SetOutput(normalized);
			_gameSettings.OutputPath = normalized.FullName;
		}

		_saveRequestSubject.OnNext(Unit.Default);
	}

	public void SetOutputFolder(DirectoryInfo directory)
	{
		if (!directory.Exists) directory.Create();

		userPaths.SetOutput(directory);
		_gameSettings.OutputPath = directory.FullName;

		_saveRequestSubject.OnNext(Unit.Default);
	}
}