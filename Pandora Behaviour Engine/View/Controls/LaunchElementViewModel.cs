using Pandora.DTOs;
using Pandora.Enums;
using Pandora.Logging.Extensions;
using Pandora.Services.Interfaces;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using Splat;
using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace Pandora.ViewModels;

public partial class LaunchElementViewModel : ViewModelBase, IActivatableViewModel
{
	private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

	private readonly IEngineSessionState _state;
	public IEngineSessionState State => _state;

	private readonly IModService _modService;
	private readonly IBehaviourEngine _engine;
	private readonly IWindowStateService _windowStateService;
	private readonly IEngineConfigurationService _engineConfigService;

	private readonly bool _autoClose = false;
	private readonly bool _autoRun = false;

	public ViewModelActivator Activator { get; } = new();

	public LaunchElementViewModel(
		IEngineSessionState state,
		IModService modService,
		IBehaviourEngine engine,
		IEngineConfigurationService engineConfigService,
		IWindowStateService windowStateService)
	{
		_state = state;
		_engine = engine;
		_modService = modService;
		_windowStateService = windowStateService;
		_engineConfigService = engineConfigService;

		this.WhenActivated(disposables =>
		{
			_engineConfigService.CurrentFactoryChanged
				.DistinctUntilChanged()
				.Select(async factory =>
				{
					try
					{
						await _engine.SwitchConfigurationAsync(factory.Create());
					}
					catch (Exception ex)
					{
						logger.Error(ex, "Failed to switch configuration");
					}
					return Unit.Default;
				})
				.Concat()
				.Subscribe()
				.DisposeWith(disposables);

			_engine.StateChanged
				.ObserveOn(RxApp.MainThreadScheduler)
				.Subscribe(state =>
				{
					State.IsEngineRunning = state == EngineState.Running;
					State.IsPreloading = state == EngineState.Preloading;

					bool isReady = state == EngineState.Ready || state == EngineState.Success || state == EngineState.Error;

					switch (state)
					{
						case EngineState.Preloading:
							break;

						case EngineState.Ready:
							_windowStateService.SetVisualState(WindowVisualState.Idle);
							break;

						case EngineState.Running:
							_windowStateService.SetVisualState(WindowVisualState.Running);
							break;

						case EngineState.Success:
							_windowStateService.SetVisualState(WindowVisualState.Idle);
							_windowStateService.FlashWindow();

							if (_autoClose)
								_windowStateService.Shutdown();
							break;

						case EngineState.Error:
							_windowStateService.SetVisualState(WindowVisualState.Error);
							logger.UiError("Launch failed. Existing output was not cleared, and current patch list will not be saved.");
							break;
					}
				})
				.DisposeWith(disposables);

			LaunchEngineCommand
				.ThrownExceptions.Subscribe(ex => this.Log().Error(ex))
				.DisposeWith(disposables);

			if (_autoRun)
				LaunchEngineCommand.Execute().Subscribe().DisposeWith(disposables);
		});

	}

	[ReactiveCommand]
	private async Task LaunchEngine()
	{
		var activeMods = _modService.GetActiveMods();


		var result = await Task.Run(() => _engine.RunAsync(activeMods));

		await _modService.SaveSettingsAsync();

		logger.UiInfo(result.Message);
		logger.UiInfo($"Launch finished in {result.Duration.TotalSeconds:F2} seconds.");
	}
}
