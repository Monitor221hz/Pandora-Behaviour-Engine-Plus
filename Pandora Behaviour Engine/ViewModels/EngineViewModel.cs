using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using DynamicData;
using DynamicData.Binding;
using Pandora.API.Patch.Engine.Config;
using Pandora.Data;
using Pandora.Models;
using Pandora.Models.Patch.Configs;
using Pandora.Services;
using Pandora.Utils;
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
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace Pandora.ViewModels;

public partial class EngineViewModel : ViewModelBase, IActivatableViewModel
{
	private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
	private static readonly IReadOnlyList<IModInfoProvider> DefaultModProviders = new List<IModInfoProvider>
	{
		new NemesisModInfoProvider(),
		new PandoraModInfoProvider()
	}.AsReadOnly();

	private readonly ModService _modService;
	private readonly JsonModSettingsStore _settingsStore;
	private readonly EngineConfigurationService _configService;
	private readonly DirectoryInfo launchDirectory = BehaviourEngine.AssemblyDirectory;

	private bool autoRun = false;
	private bool closeOnFinish = false;
	private Task _preloadTask = Task.CompletedTask;
	private DirectoryInfo currentDirectory = BehaviourEngine.SkyrimGameDirectory ?? BehaviourEngine.CurrentDirectory;

	[Reactive] private bool _isPreloading;
	[Reactive] private bool _isOutputFolderCustomSet;
	[Reactive] private bool _isVisibleLinkOutputDirectory;
	[Reactive] private string _logText = string.Empty;
	[Reactive] private string _searchTerm = string.Empty;
	[Reactive] private string _outputDirectoryMessage = string.Empty;

	[ObservableAsProperty(ReadOnly = false)] private bool? _allSelected;
	[ObservableAsProperty(ReadOnly = false)] private bool _engineRunning;

	[BindableDerivedList] private readonly ReadOnlyObservableCollection<ModInfoViewModel> _modViewModels;

	public string CurrentDirectoryInfo => currentDirectory.ToString();
	public UIOptionsViewModel UIOptions { get; } = new();
	public ViewModelActivator Activator { get; } = new();
	public Interaction<AboutDialogViewModel, Unit> ShowAboutDialog { get; } = new();
	public ObservableCollectionExtended<ModInfoViewModel> SourceMods { get; } = [];
	public ObservableCollection<IEngineConfigurationViewModel> EngineConfigurationViewModels { get; } = [];
	public BehaviourEngine Engine { get; private set; } = new();

	public EngineViewModel()
	{
		var rawArgs = Environment.GetCommandLineArgs().Skip(1).ToArray();
		var options = LaunchOptions.Parse(rawArgs);

		_settingsStore = new JsonModSettingsStore(Path.Combine(launchDirectory.FullName, AppConstants.ActiveModsPath));
		_modService = new ModService(_settingsStore, DefaultModProviders);
		_configService = new EngineConfigurationService();

		EngineConfigurationViewModels = new ObservableCollection<IEngineConfigurationViewModel>(
			_configService.GetInitialConfigurations(SetEngineConfigCommand));

		SourceMods.ToObservableChangeSet()
			.AutoRefresh(x => x.Priority)
			.Filter(this.WhenAnyValue(x => x.SearchTerm)
				.Throttle(AppConstants.SearchThrottle)
				.ObserveOn(RxApp.MainThreadScheduler)
				.Select(ModUtils.BuildFilter))
			.Sort(SortExpressionComparer<ModInfoViewModel>.Ascending(x => x.Priority))
			.Bind(out _modViewModels)
			.Subscribe();

		this.WhenActivated(disposables =>
		{
			HandleStartupArguments(options);
			InitializeSubscriptions(disposables);

			RxApp.MainThreadScheduler.Schedule(async () =>
            {
                await LoadModsAsync();
                await PreloadEngineAsync();

                if (autoRun) await LaunchEngineCommand.Execute();
            });
		});

		if (BehaviourEngine.EngineConfigurations.Count > 0)
		{
			EngineLoggerAdapter.AppendLine("Plugins loaded.");
		}
	}

	private void HandleStartupArguments(LaunchOptions options)
	{
		if (options.OutputDirectory != null && options.OutputDirectory.Exists)
		{
			currentDirectory = options.OutputDirectory;
			IsOutputFolderCustomSet = true;
		}
		else
		{
			bool fromMO2 = ProcessUtils.IsLaunchedFromModOrganizer();

			IsVisibleLinkOutputDirectory = fromMO2;
			OutputDirectoryMessage = fromMO2
				? "Output folder not set via command line arguments (-o). If you use configured output folder via MO2 (Create files in mod ...), then ignore this."
				: "Output folder is not set. Set it either via argument (-o). While the files will be generated in:";
		}

		autoRun = options.AutoRun;
		closeOnFinish = options.AutoClose;

		if (options.UseSkyrimDebug64)
		{
			var debugFactory = _configService.GetFactoryByType<SkyrimDebugConfiguration>();
			if (debugFactory != null)
			{
				_configService.SetCurrentFactory(debugFactory);
			}
		}
	}
	private void InitializeSubscriptions(CompositeDisposable disposables)
	{
		_engineRunningHelper = LaunchEngineCommand.IsExecuting
			.ToProperty(this, x => x.EngineRunning)
			.DisposeWith(disposables);

		_allSelectedHelper = SourceMods
			.ToObservableChangeSet()
			.AutoRefresh(x => x.Active)
			.QueryWhenChanged(ModUtils.IsAllSelectedExceptPandora)
			.DistinctUntilChanged()
			.ToProperty(this, x => x.AllSelected)
			.DisposeWith(disposables);

		EngineLoggerAdapter.LogObservable
			.ObserveOn(RxApp.MainThreadScheduler)
			.Subscribe(text => LogText = text, ex => logger.Error(ex))
			.DisposeWith(disposables);

		LaunchEngineCommand.ThrownExceptions.Subscribe(ex => this.Log().Error(ex)).DisposeWith(disposables);
	}

	private async Task PreloadEngineAsync()
	{
		if (IsPreloading)
		{
			await _preloadTask;
			return;
		}

		IsPreloading = true;
		try
		{
			var factory = _configService.GetCurrentFactory();
			Engine = factory is not null ? new BehaviourEngine(factory.Config) : new BehaviourEngine();
			Engine.SetOutputPath(currentDirectory);

			_preloadTask = Engine.PreloadAsync();
			await _preloadTask;
		}
		finally
		{
			IsPreloading = false;
		}
	}
	private async Task LoadModsAsync()
	{
		List<DirectoryInfo> searchDirectories =
		[
			launchDirectory,
			BehaviourEngine.CurrentDirectory,
			currentDirectory
		];
		DirectoryInfo[] uniqueDirectories = [.. searchDirectories
			.Where(d => d != null)
			.DistinctBy(d => d.FullName, StringComparer.OrdinalIgnoreCase)];
		var mods = await _modService.LoadModsAsync([.. uniqueDirectories]);

		RxApp.MainThreadScheduler.Schedule(() =>
		{
			SourceMods.Clear();
			SourceMods.AddRange(mods);
			EngineLoggerAdapter.AppendLine($"Mods loaded.");
		});
	}

	private async Task WaitForPreloadAsync()
	{
		EngineLoggerAdapter.Clear();
		EngineLoggerAdapter.AppendLine($"Engine launched with configuration: {Engine.Configuration.Name}. Do not exit before the launch is finished.");
		EngineLoggerAdapter.AppendLine("Waiting for preload to finish...");
		await _preloadTask;
		EngineLoggerAdapter.AppendLine("Preload finished.");
	}
	private async Task<bool> ExecuteEngineAsync()
	{
		var activeMods = ModUtils.GetActiveModsByPriority(SourceMods);
		bool success;

		try
		{
			success = await Task.Run(() => Engine.LaunchAsync(activeMods));
		}
		catch (Exception ex)
		{
			logger.Error(ex, "Error during engine launch");
			success = false;
		}

		return success;
	}

	private async Task HandleLaunchResultAsync(bool success)
	{
		if (success)
		{
			await _modService.SaveActiveModsAsync(SourceMods);
			if (closeOnFinish && Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime lifetime)
			{
				lifetime.Shutdown();
			}
		}
		else
		{
			EngineLoggerAdapter.AppendLine("Launch aborted. Existing output was not cleared, and current patch list will not be saved.");
		}
	}

	[ReactiveCommand]
	private async Task LaunchEngine()
	{
		var timer = Stopwatch.StartNew();
		await WaitForPreloadAsync();
		var success = await ExecuteEngineAsync();
		timer.Stop();
		EngineLoggerAdapter.AppendLine(Engine.GetMessages(success));
		EngineLoggerAdapter.AppendLine($"Launch finished in {timer.Elapsed.TotalSeconds:F2} seconds.");
		await HandleLaunchResultAsync(success);
		_ = PreloadEngineAsync();
	}

	[ReactiveCommand]
	private async Task SetEngineConfig(IEngineConfigurationFactory? factory)
	{
		if (factory is null || factory == _configService.GetCurrentFactory()) return;

		await _preloadTask;
		_configService.SetCurrentFactory(factory);
		await PreloadEngineAsync();
	}

	[ReactiveCommand]
	private async Task ShowAboutDialogAsync() =>
		await ShowAboutDialog.Handle(new AboutDialogViewModel());

	[ReactiveCommand]
	private async Task CopyTextAsync(string? text) =>
		await AvaloniaServices.DoSetClipboardTextAsync(text);

	[ReactiveCommand]
	private async Task OpenUrlAsync(string url) =>
		await AvaloniaServices.DoLaunchUriAsync(url);

	[ReactiveCommand]
	private void SortAscending(DataGridColumnHeader? header) =>
		ModUtils.ApplySort(header, c => c.Sort(ListSortDirection.Ascending));

	[ReactiveCommand]
	private void SortDescending(DataGridColumnHeader? header) =>
		ModUtils.ApplySort(header, c => c.Sort(ListSortDirection.Descending));

	[ReactiveCommand]
	private void ClearSort(DataGridColumnHeader? header) =>
		ModUtils.ApplySort(header, c => c.ClearSort());

	[ReactiveCommand]
	private void ToggleSelectAll(bool? isChecked)
	{
		if (isChecked is not bool check) return;

		ModUtils.SetAllModActiveStates(SourceMods, check);
	}
}