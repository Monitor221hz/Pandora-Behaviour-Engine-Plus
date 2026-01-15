using Pandora.Enums;
using Pandora.Models.Engine;
using Pandora.Services.Interfaces;
using System;
using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;

namespace Pandora.Services;

public class EngineOrchestrator : IDisposable
{
	private readonly IBehaviourEngine _engine;
	private readonly IEngineConfigurationService _configService;
	private readonly IEngineSharedState _state;
	private readonly IWindowStateService _windowService;
	private readonly CompositeDisposable _disposables = new();

	public EngineOrchestrator(
		IBehaviourEngine engine,
		IEngineConfigurationService configService,
		IEngineSharedState state,
		IWindowStateService windowService)
	{
		_engine = engine;
		_configService = configService;
		_state = state;
		_windowService = windowService;
	}

	public void Initialize()
	{
		_configService.CurrentFactoryChanged
			.DistinctUntilChanged()
			.Select(factory => Observable.FromAsync(() =>
				_engine.SwitchConfigurationAsync(factory.Create())))
			.Concat()
			.Subscribe()
			.DisposeWith(_disposables);


		_engine.StateChanged.Subscribe(s =>
		{
			_state.IsEngineRunning = s == EngineState.Running;
			_state.IsPreloading = s == EngineState.Preloading;
		}).DisposeWith(_disposables);

		_engine.StateChanged.Subscribe(HandleWindowState).DisposeWith(_disposables);
	}

	private void HandleWindowState(EngineState state)
	{
		switch (state)
		{
			case EngineState.Running:
				_windowService.SetVisualState(WindowVisualState.Running);
				break;
			case EngineState.Success:
				_windowService.SetVisualState(WindowVisualState.Idle);
				_windowService.FlashWindow();
				break;
			case EngineState.Error:
				_windowService.SetVisualState(WindowVisualState.Error);
				break;
			default: _windowService.SetVisualState(WindowVisualState.Idle);
				break;
		}
	}

	public void Dispose() => _disposables.Dispose();
}