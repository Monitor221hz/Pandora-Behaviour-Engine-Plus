// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

﻿using Pandora.Configuration;
using Pandora.Mods.Abstractions;
using Pandora.Paths.Abstractions;
using Pandora.Platform.Windows;
using Pandora.ViewModels;
using System;
using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;

namespace Pandora.Models.Engine;

public sealed class EngineOrchestrator(
	IModService mods,
	IUserPaths userPaths,
	IBehaviourEngine engine,
	IEngineConfigurationService configService,
	IEngineSharedState state,
	IWindowStateService windowService) : IDisposable
{
	private readonly CompositeDisposable _disposables = [];

	public void Initialize()
	{
		userPaths.GameDataChanged
			.Skip(1)
			.SelectMany(_ => Observable.FromAsync(mods.RefreshModsAsync))
			.Subscribe()
			.DisposeWith(_disposables);

		configService.CurrentFactoryChanged
			.DistinctUntilChanged()
			.Select(factory => Observable.FromAsync(() =>
				engine.SwitchConfigurationAsync(factory.Create())))
			.Concat()
			.Subscribe()
			.DisposeWith(_disposables);


		engine.StateChanged.Subscribe(s =>
		{
			state.IsEngineRunning = s == EngineState.Running;
			state.IsPreloading = s == EngineState.Preloading;
		}).DisposeWith(_disposables);

		engine.StateChanged.Subscribe(HandleWindowState).DisposeWith(_disposables);
	}

	private void HandleWindowState(EngineState state)
	{
		switch (state)
		{
			case EngineState.Running:
				windowService.SetVisualState(WindowVisualState.Running);
				break;
			case EngineState.Success:
				windowService.SetVisualState(WindowVisualState.Idle);
				windowService.FlashWindow();
				break;
			case EngineState.Error:
				windowService.SetVisualState(WindowVisualState.Error);
				break;
			default: windowService.SetVisualState(WindowVisualState.Idle);
				break;
		}
	}

	public void Dispose()
	{
		_disposables.Dispose();
		GC.SuppressFinalize(this);
	}
}