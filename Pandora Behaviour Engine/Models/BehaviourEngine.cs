using Microsoft.Extensions.DependencyInjection;
using Pandora.API.Data;
using Pandora.API.DTOs;
using Pandora.API.Patch;
using Pandora.API.Patch.Config;
using Pandora.API.Patch.Engine.Config;
using Pandora.API.Services;
using Pandora.Logging;
using Pandora.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;

namespace Pandora.Models;

public sealed class BehaviourEngine : IBehaviourEngine, IDisposable
{
	private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

	private readonly IPathResolver _pathResolver;
	private readonly IServiceScopeFactory _scopeFactory;
	private IServiceScope? _preparedScope;

	private readonly SemaphoreSlim _lifecycleGate = new(1, 1);

	private Task? _preloadTask;

	private EngineState _state = EngineState.Idle;
	private readonly BehaviorSubject<EngineState> _stateSubject;

	public IEngineConfiguration Configuration { get; private set; }
	public EngineState CurrentState => _state;
	public IObservable<EngineState> StateObservable => _stateSubject;

	public BehaviourEngine(IPathResolver pathResolver, IEngineConfiguration config, IServiceScopeFactory scopeFactory)
	{
		_pathResolver = pathResolver;
		Configuration = config;
		_scopeFactory = scopeFactory;
		_stateSubject = new BehaviorSubject<EngineState>(_state);
	}

	public Task InitializeAsync() => StartPreloadAsync();

	public async Task<EngineLaunchResult> LaunchAsync(List<IModInfo> mods)
	{
		await _lifecycleGate.WaitAsync();
		var timer = Stopwatch.StartNew();

		IServiceScope? scope = _preparedScope;
		_preparedScope = null;

		try
		{
			logger.UiClear();
			logger.UiInfo(
				$"Engine launched with configuration: {Configuration.Name}. Do not exit before the launch is finished."
			);

			logger.UiInfo("Waiting for preload to finish...");
			await AwaitPreloadIfRunningAsync();
			logger.UiInfo("Preload finished.");

			if (scope is null)
			{
				scope = _scopeFactory.CreateScope();

				var tempPatcher = (IPatcher)scope.ServiceProvider.GetRequiredService(Configuration.PatcherType);

				await tempPatcher.PreloadAsync();
			}

			var patcher = (IPatcher)scope.ServiceProvider.GetRequiredService(Configuration.PatcherType);

			TransitionTo(EngineState.Running);

			patcher.SetTarget(mods);

			EnsureOutputFolder();

			if (!await patcher.UpdateAsync())
			{
				TransitionTo(EngineState.Error);
				return Fail(timer, "Update phase failed.");
			}

			var success = await patcher.RunAsync();

			TransitionTo(success ? EngineState.Idle : EngineState.Error);

			return new EngineLaunchResult(
				success,
				timer.Elapsed,
				success ? patcher.GetPostRunMessages()
						: patcher.GetFailureMessages()
			);
		}
		catch (Exception ex)
		{
			TransitionTo(EngineState.Error);
			return Fail(timer, ex.Message);
		}
		finally
		{
			scope?.Dispose();
			_lifecycleGate.Release();
			_ = StartPreloadAsync();
		}
	}

	public async Task SetConfigurationAsync(IEngineConfigurationFactory factory)
	{
		await _lifecycleGate.WaitAsync();
		try
		{
			if (_state == EngineState.Running)
				throw new InvalidOperationException("Cannot switch config while running.");

			var newConfig = factory.Create();

			if (Configuration.GetType() == newConfig.GetType())
			{
				return;
			}

			Configuration = newConfig;

			DisposePreparedScope();

			_ = StartPreloadAsync();
		}
		finally
		{
			_lifecycleGate.Release();
		}
	}

	private Task StartPreloadAsync()
	{
		if (_preloadTask != null && !_preloadTask.IsCompleted) return _preloadTask;

		var scope = _scopeFactory.CreateScope();

		_preparedScope = scope;

		_preloadTask = Task.Run(async () =>
		{
			try
			{
				var patcher = (IPatcher)scope.ServiceProvider.GetRequiredService(Configuration.PatcherType);

				TransitionTo(EngineState.Preloading);
				await patcher.PreloadAsync();
				TransitionTo(EngineState.Idle);
			}
			catch (Exception)
			{
				TransitionTo(EngineState.Error);
				DisposePreparedScope();
				throw;
			}
		});

		return _preloadTask;
	}

	private void DisposePreparedScope()
	{
		_preparedScope?.Dispose();
		_preparedScope = null;
	}

	private async Task AwaitPreloadIfRunningAsync()
	{
		if (_preloadTask != null && !_preloadTask.IsCompleted)
		{
			try { await _preloadTask; } catch { }
		}
	}

	private void TransitionTo(EngineState next)
	{
		if (_state == next) return;
		_state = next;
		_stateSubject.OnNext(next);
	}

	private void EnsureOutputFolder()
	{
		var output = _pathResolver.GetOutputFolder();
		if (!output.Exists) output.Create();
	}

	private static EngineLaunchResult Fail(Stopwatch timer, string message)
	{
		timer.Stop();
		return new EngineLaunchResult(false, timer.Elapsed, message);
	}

	public void Dispose()
	{
		DisposePreparedScope();
		_lifecycleGate.Dispose();
		_stateSubject.Dispose();
	}
}