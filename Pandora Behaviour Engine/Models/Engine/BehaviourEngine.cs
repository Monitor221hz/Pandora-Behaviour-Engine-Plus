using Pandora.API.Data;
using Pandora.API.DTOs;
using Pandora.API.Patch;
using Pandora.API.Patch.Engine.Config;
using Pandora.API.Services;
using Pandora.Logging.Extensions;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Pandora.Models.Engine;

public sealed class BehaviourEngine : IBehaviourEngine
{
	private readonly IEngineRunner _runner;
	private readonly IEngineStateMachine _state;
	private readonly SemaphoreSlim _lock = new(1, 1);

	public IEngineConfiguration Configuration { get; private set; }

	private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

	public BehaviourEngine(
		IEngineRunner runner, 
		IEngineStateMachine state,
		IEngineConfiguration initialConfig)
	{
		_runner = runner;
		_state = state;
		Configuration = initialConfig;
	}

	public EngineState State => _state.Current;
	public IObservable<EngineState> StateChanged => _state.Changes;

	public async Task InitializeAsync()
	{
		await _lock.WaitAsync();
		try
		{
			await ExecutePreloadInternalAsync();
		}
		finally
		{
			_lock.Release();
		}
	}

	public async Task<EngineResult> RunAsync(IReadOnlyList<IModInfo> mods)
	{
		if (_state.Current == EngineState.Running)
			return EngineResult.Fail("Engine is already running.");

		logger.UiClear();
		logger.UiInfo($"Engine launched with configuration: {Configuration.Name}. Do not exit before the launch is finished.");
		bool isWaitingForPreload = _lock.CurrentCount == 0;

		if (isWaitingForPreload)
		{
			logger.UiInfo("Waiting for preload to finish...");
		}
		await _lock.WaitAsync();

		try
		{
			if (isWaitingForPreload)
			{
				logger.UiInfo("Preload finished.");
			}

			if (_state.Current == EngineState.Running)
				return EngineResult.Fail("Engine is busy.");

			_state.Transition(EngineState.Running);

			var result = await _runner.RunAsync(mods);

			_state.Transition(result.IsSuccess ? EngineState.Success : EngineState.Error);

			_ = Task.Run(RunPostLaunchPreload);

			return result;
		}
		catch (Exception ex)
		{
			_state.Transition(EngineState.Error);
			_ = Task.Run(RunPostLaunchPreload);
			return EngineResult.Fail($"Critical error: {ex.Message}");
		}
		finally
		{
			_lock.Release();
		}
	}

	public async Task SwitchConfigurationAsync(IEngineConfiguration config)
	{
		await _lock.WaitAsync();
		try
		{
			if (_state.Current == EngineState.Running)
				throw new InvalidOperationException("Cannot switch config while running.");

			Configuration = config;

			await _runner.SwitchConfigurationAsync(config);

			await ExecutePreloadInternalAsync();
		}
		finally
		{
			_lock.Release();
		}
	}

	private async Task ExecutePreloadInternalAsync()
	{
		try
		{
			_state.Transition(EngineState.Preloading);
			await _runner.PreloadAsync();
			_state.Transition(EngineState.Ready);
		}
		catch
		{
			_state.Transition(EngineState.Error);
			throw;
		}
	}

	private async Task RunPostLaunchPreload()
	{
		await _lock.WaitAsync();
		try
		{
			if (_state.Current == EngineState.Running) return;

			await ExecutePreloadInternalAsync();
		}
		catch (Exception)
		{
			_state.Transition(EngineState.Error);
		}
		finally
		{
			_lock.Release();
		}
	}

	public void Dispose()
	{
		_lock.Dispose();
	}

}
