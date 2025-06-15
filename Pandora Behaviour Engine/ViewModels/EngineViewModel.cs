using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Styling;
using DynamicData;
using DynamicData.Binding;
using Pandora.API.Patch.Engine.Config;
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

	public BehaviourEngine Engine { get; private set; } = new();
	public ViewModelActivator Activator { get; } = new();
	public Interaction<AboutDialogViewModel, Unit> ShowAboutDialog { get; } = new();
	public ObservableCollectionExtended<ModInfoViewModel> SourceMods { get; } = [];
	public ObservableCollection<IEngineConfigurationViewModel> EngineConfigurationViewModels { get; } = [];

	private readonly HashSet<string> startupArguments = new(StringComparer.OrdinalIgnoreCase);

	private readonly ModService _modService;
	private readonly EngineConfigurationService _configService;

	private readonly DirectoryInfo launchDirectory = BehaviourEngine.AssemblyDirectory;
	private DirectoryInfo currentDirectory = BehaviourEngine.SkyrimGameDirectory ?? BehaviourEngine.CurrentDirectory;

	public string CurrentDirectoryInfo => currentDirectory.ToString();

	private Task preloadTask;

	private bool closeOnFinish = false;
	private bool autoRun = false;

	[Reactive] private string _logText = string.Empty;
	[Reactive] private string _searchTerm = string.Empty;
	[Reactive] private DataGridGridLinesVisibility _gridLinesVisibility = (DataGridGridLinesVisibility)Properties.GUISettings.Default.GridLinesVisibility;
	[Reactive] private bool _isCompactRowHeight = Properties.GUISettings.Default.IsCompactRowHeight;
	[Reactive] private bool _isOutputFolderCustomSet;
	[Reactive] private bool _isPreloading;
	[Reactive] private bool? _themeToggleState = Properties.GUISettings.Default.AppTheme switch { 0 => true, 1 => false, _ => true };

	[ObservableAsProperty(ReadOnly = false)] private bool? _allSelected;
	[ObservableAsProperty(ReadOnly = false)] private bool _engineRunning;

	[BindableDerivedList] private readonly ReadOnlyObservableCollection<ModInfoViewModel> _modViewModels;

	public EngineViewModel()
	{
		startupArguments = Environment.GetCommandLineArgs().ToHashSet(StringComparer.OrdinalIgnoreCase);

		_modService = new ModService(Path.Combine(launchDirectory.FullName, "Pandora_Engine", "ActiveMods.json"));
		_configService = new EngineConfigurationService();

		EngineConfigurationViewModels = new ObservableCollection<IEngineConfigurationViewModel>(
			_configService.GetInitialConfigurations(SetEngineConfigCommand));

		SourceMods.ToObservableChangeSet()
			.AutoRefresh(x => x.Priority)
			.Filter(this.WhenAnyValue(x => x.SearchTerm)
				.Throttle(TimeSpan.FromMilliseconds(200))
				.ObserveOn(RxApp.MainThreadScheduler)
				.Select(ModUtils.BuildFilter))
			.Sort(SortExpressionComparer<ModInfoViewModel>.Ascending(x => x.Priority))
			.Bind(out _modViewModels)
			.Subscribe();

		ReadStartupArguments();

		this.WhenActivated(disposables =>
		{
			RxApp.MainThreadScheduler.Schedule(PreloadEngine);

			Observable.FromAsync(async () =>
			{
				try
				{
					await LoadAsync();
					EngineLogger.AppendLine("Mods loaded.\n\nWaiting for preload to finish...");
					await preloadTask;
					EngineLogger.AppendLine("Preload finished.");
				}
				catch (Exception ex)
				{
					logger.Error(ex, "Error during activation loading/preload");
					EngineLogger.AppendLine($"Error: {ex.Message}");
				}
			}).Subscribe().DisposeWith(disposables);

			this.WhenAnyValue(x => x.IsCompactRowHeight)
				.Skip(1)
				.Subscribe(value =>
				{
					Properties.GUISettings.Default.IsCompactRowHeight = value;
					Properties.GUISettings.Default.Save();
				}).DisposeWith(disposables);

			this.WhenAnyValue(x => x.GridLinesVisibility)
				.Skip(1)
				.Subscribe(value =>
				{
					Properties.GUISettings.Default.GridLinesVisibility = (int)value;
					Properties.GUISettings.Default.Save();
				}).DisposeWith(disposables);

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

			EngineLogger.LogObservable
				.ObserveOn(RxApp.MainThreadScheduler)
				.DistinctUntilChanged()
				.Subscribe(text => LogText = text)
				.DisposeWith(disposables);

			LaunchEngineCommand.ThrownExceptions.Subscribe(ex => this.Log().Error(ex)).DisposeWith(disposables);

			if (autoRun) RxApp.MainThreadScheduler.Schedule(() =>
				LaunchEngineCommand.Execute().Subscribe().DisposeWith(disposables));

		});

	}
	private void PreloadEngine()
	{
		var newFactory = _configService.GetCurrentFactory();

		Engine = newFactory is not null ? new BehaviourEngine(newFactory.Config) : new BehaviourEngine();
		Engine.SetOutputPath(currentDirectory);

		IsPreloading = true;

		preloadTask = Task.Run(async () =>
		{
			try
			{
				await Engine.PreloadAsync();
			}
			catch (Exception ex)
			{
				logger.Error(ex, "Error during preload");
			}
			finally
			{
				RxApp.MainThreadScheduler.Schedule(() => IsPreloading = false);
			}
		});
	}

	private void ReadStartupArguments()
	{
		foreach (var arg in startupArguments)
			ProcessArgument(arg);
	}

	private void ProcessArgument(string arg)
	{
		switch (arg.ToLowerInvariant())
		{
			case "-autoclose":
				closeOnFinish = true;
				break;
			case "-autorun":
				autoRun = true;
				closeOnFinish = true;
				break;
			case "-skyrimdebug64":
				var debugConfig = EngineConfigurationService.FlattenConfigurations(EngineConfigurationViewModels)
					.OfType<EngineConfigurationViewModel>()
					.Select(vm => vm.Factory)
					.FirstOrDefault(f => f is ConstEngineConfigurationFactory<SkyrimDebugConfiguration>);
				_configService.SetCurrentFactory(debugConfig);
				Engine = new BehaviourEngine(debugConfig?.Config);
				break;
			default:
				if (arg.StartsWith("-o:"))
				{
					currentDirectory = new DirectoryInfo(arg[3..]);
					IsOutputFolderCustomSet = true;
				}
				break;
		}
	}
	public async Task LoadAsync()
	{
		SourceMods.Clear();

		var searchDirectories = new List<DirectoryInfo>
		{
			launchDirectory,
			BehaviourEngine.CurrentDirectory,
			currentDirectory
		};

		var mods = await _modService.LoadModsAsync(searchDirectories.ToArray());
		var viewModelsMods = await _modService.PrepareModViewModelsAsync(mods);
		SourceMods.AddRange(viewModelsMods);

		if (BehaviourEngine.EngineConfigurations.Count > 0)
		{
			EngineLogger.AppendLine("Plugins loaded.");
		}
	}

	[ReactiveCommand]
	private async Task SetEngineConfig(IEngineConfigurationFactory? config)
	{
		if (config is null) return;
		_configService.SetCurrentFactory(config);
		await preloadTask;
		PreloadEngine();
	}

	[ReactiveCommand]
	private async Task LaunchEngine()
	{
		EngineLogger.Clear();

		EngineLogger.AppendLine($"Engine launched with configuration: {Engine.Configuration.Name}. Do not exit before the launch is finished.");
		EngineLogger.AppendLine("Waiting for preload to finish...");

		Stopwatch timer = Stopwatch.StartNew();
		await preloadTask;
		EngineLogger.AppendLine("\nPreload finished.");

		var activeMods = ModUtils.GetActiveModsByPriority(SourceMods);
		bool success = await Task.Run(() => Engine.LaunchAsync(activeMods));

		timer.Stop();

		EngineLogger.AppendLine(Engine.GetMessages(success));

		if (success)
		{
			EngineLogger.AppendLine($"Launch finished in {timer.Elapsed.TotalSeconds:F2} seconds.");
			await _modService.SaveActiveModsAsync(SourceMods);

			if (closeOnFinish && Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime lifetime)
			{
				lifetime.Shutdown();
			}
		}
		else
		{
			EngineLogger.AppendLine("Launch aborted. Existing output was not cleared, and current patch list will not be saved.");
		}
		PreloadEngine();
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

	[ReactiveCommand]
	private void ToggleTheme(bool? isChecked)
	{
		ThemeToggleState = isChecked;

		switch (isChecked)
		{
			case true:
				Application.Current!.RequestedThemeVariant = ThemeVariant.Light;
				Properties.GUISettings.Default.AppTheme = 0;
				break;
			case false:
				Application.Current!.RequestedThemeVariant = ThemeVariant.Dark;
				Properties.GUISettings.Default.AppTheme = 1;
				break;
		}
		Properties.GUISettings.Default.Save();
	}
}