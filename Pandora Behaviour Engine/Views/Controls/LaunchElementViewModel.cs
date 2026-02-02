// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

﻿using Pandora.CLI;
using Pandora.Logging.Extensions;
using Pandora.Models.Engine;
using Pandora.Mods.Abstractions;
using Pandora.Platform.Windows;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using Splat;
using System;
using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace Pandora.ViewModels;

public partial class LaunchElementViewModel : ViewModelBase, IActivatableViewModel
{
	private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

	public IEngineSharedState State { get; }

	private readonly IModService _modService;
	private readonly IBehaviourEngine _engine;
	private readonly IWindowStateService _windowStateService;

	private readonly bool _autoClose = false;
	private readonly bool _autoRun = false;

	public ViewModelActivator Activator { get; } = new();

	public LaunchElementViewModel(
		LaunchOptions options,
		IEngineSharedState state,
		IModService modService,
		IBehaviourEngine engine,
		IWindowStateService windowStateService)
	{
		State = state;
		_engine = engine;
		_modService = modService;
		_windowStateService = windowStateService;

		_autoClose = options.AutoClose;
		_autoRun = options.AutoRun;

		this.WhenActivated(disposables =>
		{
			LaunchEngineCommand
				.ThrownExceptions.Subscribe(ex => this.Log().Error(ex))
				.DisposeWith(disposables);

			if (_autoRun)
				LaunchEngineCommand.Execute().Subscribe();
		});

	}

	[ReactiveCommand]
	private async Task LaunchEngine()
	{
		var activeMods = _modService.GetActiveMods();

		var result = await Task.Run(() => _engine.RunAsync(activeMods));

		logger.UiInfo(result.Message);
		logger.UiInfo($"Launch finished in {result.Duration.TotalSeconds:F2} seconds.");

		await _modService.SaveSettingsAsync();

		if (_autoClose) 
			_windowStateService.Shutdown();
	}
}
