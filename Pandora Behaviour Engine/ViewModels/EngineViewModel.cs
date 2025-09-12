// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using DynamicData;
using DynamicData.Binding;
using Pandora.API.Patch.Engine.Config;
using Pandora.Data;
using Pandora.Logging;
using Pandora.Models;
using Pandora.Services;
using Pandora.Utils;
using Pandora.Views;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using Splat;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Pandora.ViewModels;

public partial class EngineViewModel : ViewModelBase, IActivatableViewModel
{
	private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

	private static readonly EngineConfigurationService _configService = new();

	private static DirectoryInfo DataOrCurrentDirectory => BehaviourEngine.SkyrimGameDirectory ?? BehaviourEngine.CurrentDirectory;

	[BindableDerivedList] 
	private readonly ReadOnlyObservableCollection<ModInfoViewModel> _modViewModels;

	private readonly bool _autoRun = false;
	private readonly bool _autoClose = false;
	private readonly SemaphoreSlim _preloadLock = new(1, 1);

	private Task _preloadTask = Task.CompletedTask;

	[Reactive] private bool _isPreloading;
	[Reactive] private bool _isOutputFolderCustomSet;
	[Reactive] private bool _isVisibleLinkOutputDirectory;
	[Reactive] private string _logText = string.Empty;
	[Reactive] private string _searchTerm = string.Empty;
	[Reactive] private string _outputDirectoryMessage = string.Empty;

	[ObservableAsProperty(ReadOnly = false)] private bool? _allSelected;
	[ObservableAsProperty(ReadOnly = false)] private bool _engineRunning;

	public static string OutputFolderUri => PandoraPaths.OutputPath.FullName;

	public BehaviourEngine Engine { get; private set; } = new BehaviourEngine();
	public DataGridOptionsViewModel DataGridOptions { get; } = new();
	public Interaction<AboutDialogViewModel, Unit> ShowAboutDialog { get; } = new();
	public SettingsViewModel SettingsApp { get; } = new();
	public ViewModelActivator Activator { get; } = new();

	public ObservableCollection<IEngineConfigurationViewModel> EngineConfigurationViewModels { get; } = [];
	public ObservableCollectionExtended<ModInfoViewModel> SourceMods { get; } = [];

	public EngineViewModel()
	{
		var startup = StartupService.Handle(LaunchOptions.Current);

		_autoRun = startup.AutoRun;
		_autoClose = startup.AutoClose;
		_isOutputFolderCustomSet = startup.IsCustomSet;
		_outputDirectoryMessage = startup.Message;

		_configService.Initialize(startup.UseSkyrimDebug64, SetEngineConfigCommand);

		EngineConfigurationViewModels = new ObservableCollection<IEngineConfigurationViewModel>(
			_configService.GetConfigurations());

		SourceMods.ToObservableChangeSet()
			.AutoRefresh(x => x.Priority)
			.Filter(this.WhenAnyValue(x => x.SearchTerm)
				.Throttle(TimeSpan.FromMilliseconds(200))
				.ObserveOn(RxApp.MainThreadScheduler)
				.Select(ModUtils.BuildNameFilter))
			.Sort(SortExpressionComparer<ModInfoViewModel>.Ascending(x => x.Priority))
			.Bind(out _modViewModels)
			.Subscribe();

		this.WhenActivated(disposables =>
		{
			EngineLoggerAdapter.LogObservable
				.ObserveOn(RxApp.MainThreadScheduler)
				.Subscribe(line => LogText = line, ex => logger.Error(ex))
				.DisposeWith(disposables);

			Observable.FromAsync(InitAsync)
				.Subscribe()
				.DisposeWith(disposables);

			InitSubscriptions(disposables);

			if (_autoRun) 
				LaunchEngineCommand.Execute().Subscribe().DisposeWith(disposables);

		});

		StartupService.LogPlugins();
	}

	private async Task PreloadEngineAsync()
	{
		await _preloadLock.WaitAsync().ConfigureAwait(false);
		try
		{
			if (IsPreloading)
			{
				await _preloadTask.ConfigureAwait(false);
				return;
			}

			IsPreloading = true;
			try
			{
				var factory = _configService.CurrentFactory;

				Engine = new BehaviourEngine()
					.SetConfiguration(factory.Config)
					.SetOutputPath(PandoraPaths.OutputPath);

				_preloadTask = Engine.PreloadAsync();
				await _preloadTask.ConfigureAwait(false);
			}
			finally
			{
				IsPreloading = false;
			}
		}
		catch (Exception ex)
		{
			logger.Error(ex, "Preload failed.");
			EngineLoggerAdapter.AppendLine("Error: Preload failed. See logs for details.");
		}
		finally
		{
			_preloadLock.Release();
		}
	}
	private async Task InitAsync()
	{
		try
		{
			var modProviders = new List<IModInfoProvider>
			{
				new NemesisModInfoProvider(),
				new PandoraModInfoProvider(),
			};
			var uniqueDirectories = new List<DirectoryInfo>
			{
				BehaviourEngine.AssemblyDirectory,
				BehaviourEngine.CurrentDirectory,
				DataOrCurrentDirectory,
			};

			var loadModsTask = ModLoader.LoadModsVMAsync(SourceMods, uniqueDirectories, modProviders);
			var preloadTask = PreloadEngineAsync();

			await Task.WhenAll(loadModsTask, preloadTask);
		}
		catch (Exception ex)
		{
			logger.Error(ex, "Failed to initialize load mods and preload engine.");
			EngineLoggerAdapter.AppendLine("Error: Failed to load initial data. Please check the logs.");
		}
	}

	private void InitSubscriptions(CompositeDisposable disposables)
	{
		_engineRunningHelper = LaunchEngineCommand.IsExecuting
			.ToProperty(this, x => x.EngineRunning)
			.DisposeWith(disposables);

		_allSelectedHelper = SourceMods
			.ToObservableChangeSet()
			.AutoRefresh(x => x.Active)
			.QueryWhenChanged(ModUtils.AreAllNonPandoraModsSelected)
			.DistinctUntilChanged()
			.ToProperty(this, x => x.AllSelected)
			.DisposeWith(disposables);

		LaunchEngineCommand.ThrownExceptions.Subscribe(ex => this.Log().Error(ex)).DisposeWith(disposables);
	}

	private async Task WaitForPreloadAsync()
	{
		EngineLoggerAdapter.Clear();
		EngineLoggerAdapter.AppendLine($"Engine launched with configuration: {Engine.Configuration.Name}. Do not exit before the launch is finished.");
		try
		{
			EngineLoggerAdapter.AppendLine("Waiting for preload to finish...");
			await _preloadTask.ConfigureAwait(false);
			EngineLoggerAdapter.AppendLine("Preload finished.");
		}
		catch (Exception ex)
		{
			logger.Error(ex, "Error while waiting for preload.");
			EngineLoggerAdapter.AppendLine("Error: Preload failed. See logs for details.");
			throw;
		}
	}

	private async Task<bool> ExecuteEngineAsync()
	{
		var activeMods = ModUtils.GetSortedActiveMods(SourceMods);
		bool success;
		try
		{
			success = await Engine.LaunchAsync(activeMods);
		}
		catch (Exception ex)
		{
			logger.Error(ex, "Error during engine launch");
			success = false;
		}

		return success;
	}

	private async Task HandleLaunchResultAsync(bool success, MainWindow? mainWindow)
	{
		if (success)
		{
			mainWindow?.SetVisualState(WindowVisualState.Idle);
			mainWindow?.FlashUntilFocused();

			await JsonModSettingsStore.SaveAsync(SourceMods, PandoraPaths.ActiveModsFile.FullName);

			if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime lifetime && _autoClose)
				lifetime.Shutdown();
		}
		else
		{
			mainWindow?.SetVisualState(WindowVisualState.Error);
			EngineLoggerAdapter.AppendLine("Launch aborted. Existing output was not cleared, and current patch list will not be saved.");
		}
	}

	[ReactiveCommand]
	private async Task LaunchEngine()
	{
		var mainWindow = (Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.MainWindow as MainWindow;
		mainWindow?.SetVisualState(WindowVisualState.Running);

		try
		{
			var timer = Stopwatch.StartNew();

			await WaitForPreloadAsync().ConfigureAwait(false);
			var success = await ExecuteEngineAsync().ConfigureAwait(false);

			timer.Stop();

			EngineLoggerAdapter.AppendLine(Engine.GetMessages(success));
			EngineLoggerAdapter.AppendLine($"Launch finished in {timer.Elapsed.TotalSeconds:F2} seconds.");
			await HandleLaunchResultAsync(success, mainWindow);

			_ = PreloadEngineAsync();
		}
		finally
		{
			mainWindow?.SetVisualState(WindowVisualState.Idle);
		}
	}

	[ReactiveCommand]
	private async Task SetEngineConfig(IEngineConfigurationFactory? factory)
	{
		if (factory is null || factory == _configService.CurrentFactory) return;

		try
		{
			await _preloadTask.ConfigureAwait(false);
			_configService.SetCurrentFactory(factory);
			await PreloadEngineAsync().ConfigureAwait(false);
		}
		catch (Exception ex)
		{
			logger.Error(ex, "Failed to set engine config.");
			EngineLoggerAdapter.AppendLine("Error: Failed to apply configuration. See logs for details.");
		}
	}

	[ReactiveCommand]
	private async Task ShowAboutDialogAsync() =>
		await ShowAboutDialog.Handle(new AboutDialogViewModel());

	[ReactiveCommand]
	private void SortAscending(DataGridColumnHeader? header) =>
		ModUtils.ApplySortToColumn(header, c => c.Sort(ListSortDirection.Ascending));

	[ReactiveCommand]
	private void SortDescending(DataGridColumnHeader? header) =>
		ModUtils.ApplySortToColumn(header, c => c.Sort(ListSortDirection.Descending));

	[ReactiveCommand]
	private void ClearSort(DataGridColumnHeader? header) =>
		ModUtils.ApplySortToColumn(header, c => c.ClearSort());

	[ReactiveCommand]
	private void ToggleSelectAll(bool? isChecked)
	{
		if (isChecked is not bool check) return;

		ModUtils.SetModsActiveState(SourceMods, check);
	}
}