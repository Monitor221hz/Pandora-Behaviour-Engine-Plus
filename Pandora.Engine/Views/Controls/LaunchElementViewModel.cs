// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using Pandora.Skyrim.CLI;
using Pandora.Core.Logging.Extensions;
using Pandora.Core.Engine;
using Pandora.Core.Mods.Abstractions;
using Pandora.Platform.Windows;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using ReactiveUI.Primitives;
using Splat;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace Pandora.ViewModels;

public partial class LaunchElementViewModel : ViewModelBase, IActivatableViewModel
{
	private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

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
		IWindowStateService windowStateService
	)
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

		Logger.UiInfo(result.Message);
		Logger.UiInfo($"Launch finished in {result.Duration.TotalSeconds:F2} seconds.");

		await _modService.SaveSettingsAsync();

		if (_autoClose)
			_windowStateService.Shutdown();
	}
}