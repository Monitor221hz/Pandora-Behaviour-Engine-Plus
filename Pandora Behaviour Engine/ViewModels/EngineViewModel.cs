// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using Avalonia.Controls;
using DynamicData;
using DynamicData.Binding;
using NLog;
using Pandora.API.Data;
using Pandora.API.DTOs;
using Pandora.API.Patch.Config;
using Pandora.API.Services;
using Pandora.Services;
using Pandora.Services.CreationEngine;
using Pandora.Utils;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using Splat;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace Pandora.ViewModels;

public partial class EngineViewModel : ViewModelBase, IActivatableViewModel
{
	private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

	private readonly IModService _modService;
	private readonly IPathResolver _pathResolver;
	private readonly IEngineConfigurationService _engineConfigService;
	private readonly ILoggingConfigurationService _logConfigService;
	private readonly IDiskDialogService _diskDialogService;
	private readonly IWindowStateService _windowStateService;
	private readonly IBehaviourEngine _engine;
	private readonly IGameDescriptor _gameDescriptor;

	private readonly bool _autoRun = false;
	private readonly bool _autoClose = false;

	[BindableDerivedList]
	private readonly ReadOnlyObservableCollection<ModInfoViewModel> _modViewModels;

	[Reactive]
	private bool _isPreloading;

	[Reactive]
	private bool _engineRunning;

	[Reactive]
	private bool _isOutputFolderCustomSet;

	[Reactive]
	private bool _isVisibleLinkOutputDirectory;

	[Reactive]
	private string _searchTerm = string.Empty;

	[Reactive]
	private string _outputDirectoryMessage = string.Empty;

	[ObservableAsProperty(ReadOnly = false)]
	private bool? _allSelected;

	[Reactive]
	private string _outputFolderUri;

	public LaunchOptions LaunchOptions { get; }
	public LogViewModel LogVM { get; }
	public ViewModelActivator Activator { get; } = new();
	public AboutDialogViewModel AboutDialog { get; }
	public SettingsViewModel Settings { get; }
	public DataGridOptionsViewModel DataGridOptions { get; } 

	public Interaction<AboutDialogViewModel, Unit> ShowAboutDialog { get; } = new();

	public ObservableCollection<IEngineConfigurationViewModel> EngineConfigurationViewModels { get; } = [];

	public EngineViewModel(
		IModService modService,
		IPathResolver pathResolver,
		IEngineConfigurationService engineConfigService,
		ILoggingConfigurationService logConfigService,
		IDiskDialogService diskDialogService,
		IWindowStateService windowStateService,
		IBehaviourEngine engine,
		IGameDescriptor gameDescriptor,
		LaunchOptions launchOptions,
		SettingsViewModel settingsViewModel,
		AboutDialogViewModel aboutDialogViewModel,
		LogViewModel logViewModel,
		DataGridOptionsViewModel dataGridOptionsViewModel
	)
	{
		Settings = settingsViewModel;
		AboutDialog = aboutDialogViewModel;
		LogVM = logViewModel;
		DataGridOptions = dataGridOptionsViewModel;

		_modService = modService;
		_pathResolver = pathResolver;
		_engineConfigService = engineConfigService;
		_logConfigService = logConfigService;
		_diskDialogService = diskDialogService;
		_windowStateService = windowStateService;
		_engine = engine;
		_gameDescriptor = gameDescriptor;

		LaunchOptions = launchOptions;

		_autoRun = LaunchOptions.AutoRun;
		_autoClose = LaunchOptions.AutoClose;

		_isOutputFolderCustomSet = !_pathResolver.GetOutputFolder().Equals(_pathResolver.GetGameDataFolder());

		if (!_isOutputFolderCustomSet)
		{
			_outputDirectoryMessage = $"Output dir not set. Files output to: {_pathResolver.GetGameDataFolder().FullName}";
		}

		_engineConfigService.Initialize(LaunchOptions.UseSkyrimDebug64);

		EngineConfigurationViewModels = ConfigurationMenuBuilder.BuildTree(
			_engineConfigService.GetAvailableConfigurations(),
			_engineConfigService
		);

		OutputFolderUri = _pathResolver.GetOutputFolder().FullName;

		_modService
		   .Connect()
		   .AutoRefresh(x => x.Priority)
		   .Filter(this.WhenAnyValue(x => x.SearchTerm).Select(ModViewModelExtensions.BuildNameFilter))
		   .Sort(SortExpressionComparer<ModInfoViewModel>.Ascending(x => x.Priority))
		   .Bind(out _modViewModels)
		   .Subscribe();

		this.WhenActivated(disposables =>
		{
			logger.UiInfo($"{_modViewModels.Count} mods loaded.");

			_engineConfigService.CurrentFactoryChanged
				.DistinctUntilChanged()
				.SelectMany(async factory =>
				{
					try
					{
						await _engine.SetConfigurationAsync(factory);
					}
					catch (Exception ex)
					{
						logger.Error(ex, "Failed to switch configuration");
					}
					return Unit.Default;
				})
				.Subscribe()
				.DisposeWith(disposables);

			_engine.StateObservable
				.ObserveOn(RxApp.MainThreadScheduler)
				.Subscribe(async state =>
				{
					EngineRunning = state == EngineState.Running;
					IsPreloading = state == EngineState.Preloading;

					switch (state)
					{
						case EngineState.Running:
							_windowStateService.SetVisualState(WindowVisualState.Running);
							break;

						case EngineState.Error:
							_windowStateService.SetVisualState(WindowVisualState.Error);
							logger.UiError("Launch failed. Existing output was not cleared, and current patch list will not be saved.");
							break;

						case EngineState.Idle:
							_windowStateService.SetVisualState(WindowVisualState.Idle);

							if (_autoClose)
								_windowStateService.Shutdown();
							break;
					}
				})
				.DisposeWith(disposables);

			_allSelectedHelper = _modService
				.Connect()
				.AutoRefresh(x => x.Active)
				.ToCollection()
				.Select(items => ModViewModelExtensions.AreAllNonPandoraModsSelected(items))
				.DistinctUntilChanged()
				.ToProperty(this, x => x.AllSelected)
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
		var activeMods = _modViewModels.GetSortedActiveMods();

		var result = await Task.Run(() => _engine.LaunchAsync(activeMods));

		await _modService.SaveSettingsAsync();

		logger.UiInfo(result.Message);
		logger.UiInfo($"Launch finished in {result.Duration.TotalSeconds:F2} seconds.");
	}

	[ReactiveCommand]
	private async Task PickGameDirectory()
	{
		var filterPattern = "*.exe";

		var file = await _diskDialogService.OpenFileAsync(
			$"Select {_gameDescriptor.Name} Executable",
			filterPattern);

		if (file == null || !file.Exists)
			return;

		var folder = file.Directory;
		if (folder is not null && !folder.Name.Equals("Data", StringComparison.OrdinalIgnoreCase))
		{
			var dataSub = new DirectoryInfo(Path.Combine(folder.FullName, "Data"));
			if (dataSub.Exists) folder = dataSub;
		}

		if (folder is null || !folder.Exists) return;

		bool isValidExecutable = _gameDescriptor.ExecutableNames.Any(
			exe => exe.Equals(file.Name, StringComparison.OrdinalIgnoreCase));

		if (isValidExecutable)
		{
			_pathResolver.SetGameDataFolder(folder);
			logger.UiInfo($"Game Data folder set to: {folder.FullName}");
		}
		else
		{
			logger.UiWarn(
				$"Warning: The selected executable ({file.Name}) does not appear to be for {_gameDescriptor.Name}."
			);
		}
	}

	[ReactiveCommand]
	private async Task PickOutputDirectory()
	{
		var currentOutput = _pathResolver.GetOutputFolder();

		var folder = await _diskDialogService.OpenFolderAsync("Select Output Directory", currentOutput);

		if (folder == null || !folder.Exists)
			return;

		_pathResolver.SetOutputFolder(folder);

		_isOutputFolderCustomSet = true;

		OutputFolderUri = folder.FullName;
		logger.UiInfo($"Output directory changed: {_pathResolver.GetOutputFolder().FullName}");
		_logConfigService.UpdateLogPath(folder.FullName);
	}

	[ReactiveCommand]
	private async Task ShowAboutDialogAsync() =>
		await ShowAboutDialog.Handle(AboutDialog);

	[ReactiveCommand]
	private void SortAscending(DataGridColumnHeader? header) =>
		DataGridUtils.ApplySort(header, c => c.Sort(ListSortDirection.Ascending));

	[ReactiveCommand]
	private void SortDescending(DataGridColumnHeader? header) =>
		DataGridUtils.ApplySort(header, c => c.Sort(ListSortDirection.Descending));

	[ReactiveCommand]
	private void ClearSort(DataGridColumnHeader? header) =>
		DataGridUtils.ApplySort(header, c => c.ClearSort());

	[ReactiveCommand]
	private void ToggleSelectAll(bool? isChecked)
	{
		if (isChecked is not bool check)
			return;

		_modViewModels.SetAllActive(check);
	}
}
