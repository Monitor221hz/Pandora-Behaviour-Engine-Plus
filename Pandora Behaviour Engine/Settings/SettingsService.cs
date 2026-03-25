// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using Pandora.Platform.CreationEngine;
using Pandora.Settings.DTOs;
using Pandora.Settings.SubSettings;
using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;

namespace Pandora.Settings;

public sealed class SettingsService(
	ISettingsRepository repository,
	IThemeSettings themeSettings,
	IPathSettings pathSettings,
	IGameDescriptor descriptor) : ISettingsService, IDisposable
{
	private RootConfiguration _root = new();
	private readonly Lock _saveLock = new();
	private IDisposable? _saveSubscription;

	public IThemeSettings Theme => themeSettings;
	public IPathSettings Paths => pathSettings;

	public void Initialize()
	{
		_root = repository.Load() ?? new RootConfiguration();

		_root.App ??= new AppSettings();
		themeSettings.Initialize(_root.App);

		if (!_root.Games.TryGetValue(descriptor.Id, out var gameSettings))
		{
			gameSettings = new GameSettings();
			_root.Games[descriptor.Id] = gameSettings;
		}
		pathSettings.Initialize(gameSettings);

		var saveSignals = Observable.Merge(
			themeSettings.SaveRequired,
			pathSettings.SaveRequired
		);

		_saveSubscription = saveSignals
			.Throttle(TimeSpan.FromMilliseconds(500))
			.ObserveOn(TaskPoolScheduler.Default)
			.Subscribe(_ => Save());
	}

	private void Save()
	{
		lock (_saveLock)
		{
			repository.Save(_root);
		}
	}

	public void Dispose()
	{
		(themeSettings as IDisposable)?.Dispose();
		(pathSettings as IDisposable)?.Dispose();
		_saveSubscription?.Dispose();
	}
}