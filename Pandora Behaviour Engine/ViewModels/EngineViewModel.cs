using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using DynamicData;
using DynamicData.Binding;
using Pandora.API.Patch.Engine.Config;
using Pandora.Models;
using Pandora.Models.Patch.Configs;
using Pandora.Services;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandora.ViewModels;

public partial class EngineViewModel : ViewModelBase, IActivatableViewModel
{
	private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

	public BehaviourEngine Engine { get; private set; } = new BehaviourEngine();
	public ViewModelActivator Activator { get; }
	public Interaction<AboutDialogViewModel, Unit> ShowAboutDialog { get; }
	public ObservableCollectionExtended<ModInfoViewModel> SourceMods { get; }
	public ObservableCollection<IEngineConfigurationViewModel> EngineConfigurationViewModels { get; } = [];

	private readonly HashSet<string> startupArguments = new(StringComparer.OrdinalIgnoreCase);
	private readonly ModService _modService;
	private readonly EngineConfigurationService _configService;
	private readonly StringBuilder _logBuilder = new();

	private readonly DirectoryInfo launchDirectory = BehaviourEngine.AssemblyDirectory;
	private DirectoryInfo currentDirectory = BehaviourEngine.SkyrimGameDirectory ?? BehaviourEngine.CurrentDirectory;

	private Task preloadTask;
	private bool closeOnFinish = false;
	private bool autoRun = false;

	[Reactive] private string _logText = string.Empty;
	[Reactive] private string _searchTerm = string.Empty;

	[ObservableAsProperty(ReadOnly = false)] private bool? _allSelected;
	[ObservableAsProperty(ReadOnly = false)] private bool _engineRunning;

	[BindableDerivedList] private readonly ReadOnlyObservableCollection<ModInfoViewModel> _modViewModels;

	public EngineViewModel()
	{
		startupArguments = Environment.GetCommandLineArgs().ToHashSet(StringComparer.OrdinalIgnoreCase);

		_modService = new ModService(Path.Combine(launchDirectory.FullName, "Pandora_Engine", "ActiveMods.txt"));
		_configService = new EngineConfigurationService();

		EngineConfigurationViewModels = new ObservableCollection<IEngineConfigurationViewModel>(
			_configService.GetInitialConfigurations(SetEngineConfigCommand));

		ShowAboutDialog = new Interaction<AboutDialogViewModel, Unit>();

		SourceMods = [];
		SourceMods.ToObservableChangeSet()
			.AutoRefresh(x => x.Priority)
			.Filter(this.WhenAnyValue(x => x.SearchTerm)
				.Throttle(TimeSpan.FromMilliseconds(200))
				.ObserveOn(RxApp.MainThreadScheduler)
				.Select(BuildFilter))
			.Sort(SortExpressionComparer<ModInfoViewModel>.Ascending(x => x.Priority))
			.Bind(out _modViewModels)
			.Subscribe();

		Activator = new ViewModelActivator();
		this.WhenActivated(disposables =>
		{
			_engineRunningHelper = LaunchEngineCommand.IsExecuting
				.ToProperty(this, x => x.EngineRunning)
				.DisposeWith(disposables);

			_allSelectedHelper = SourceMods
				.ToObservableChangeSet()
				.AutoRefresh(x => x.Active)
				.QueryWhenChanged(AllSelectedCheckBoxHelper)
				.DistinctUntilChanged()
				.ToProperty(this, x => x.AllSelected)
				.DisposeWith(disposables);

			Observable
				.Interval(TimeSpan.FromMilliseconds(200))
				.ObserveOn(RxApp.MainThreadScheduler)
				.Subscribe(_ => LogText = _logBuilder.ToString())
				.DisposeWith(disposables);

			Observable.Return(Unit.Default)
				.Where(_ => autoRun)
				.ObserveOn(RxApp.MainThreadScheduler)
				.InvokeCommand(this, x => x.LaunchEngineCommand)
				.DisposeWith(disposables);
		});

		ReadStartupArguments();

		preloadTask = Task.Run(Engine.PreloadAsync);
		Engine.SetOutputPath(currentDirectory);
	}

	public async Task LoadAsync()
	{
		SourceMods.Clear();

		var modInfoList = await _modService.LoadModsAsync(launchDirectory, BehaviourEngine.CurrentDirectory, currentDirectory);

		var pandoraMod = modInfoList.FirstOrDefault(m => string.Equals(m.Code, "pandora", StringComparison.OrdinalIgnoreCase));
		if (pandoraMod is not null)
		{
			pandoraMod.Active = true;
		}
		else
		{
			_logBuilder.AppendLine("FATAL ERROR: Pandora Base does not exist. Ensure the engine was installed properly and data is not corrupted.");
		}

		SourceMods.AddRange(modInfoList.Select(m => new ModInfoViewModel(m)));

		ModService.AssignModPrioritiesFromViewModels(SourceMods);

		if (BehaviourEngine.EngineConfigurations.Count > 0)
		{
			_logBuilder.AppendLine("Plugins loaded.");
		}
		_logBuilder.AppendLine("Mods loaded.");
	}

	private void ReadStartupArguments()
	{
		if (startupArguments.Remove("-skyrimDebug64"))
		{
			var debugConfig = EngineConfigurationService.FlattenConfigurations(EngineConfigurationViewModels)
				.OfType<EngineConfigurationViewModel>()
				.Select(vm => vm.Factory)
				.FirstOrDefault(f => f is ConstEngineConfigurationFactory<SkyrimDebugConfiguration>);
			_configService.SetCurrentFactory(debugConfig);
			Engine = new BehaviourEngine(debugConfig.Config);
		}
		if (startupArguments.Remove("-autoClose")) closeOnFinish = true;
		foreach (var arg in startupArguments)
		{
			if (startupArguments.Remove("-o:"))
			{
				var path = arg.Substring(3).Trim();
				if (Directory.Exists(path)) currentDirectory = new DirectoryInfo(path);
			}

		}
		if (startupArguments.Remove("-autoRun")) { autoRun = true; closeOnFinish = true; }
	}

	private Func<ModInfoViewModel, bool> BuildFilter(string searchText) =>
		mod => string.IsNullOrEmpty(searchText) || mod.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase);

	private bool? AllSelectedCheckBoxHelper(IReadOnlyCollection<ModInfoViewModel> query)
	{
		if (query.Count == 0) return false;

		var selectedCount = query.Count(x => x.Active) - 1;

		return selectedCount switch
		{
			0 => false,
			var count when count == query.Count - 1 => true,
			_ => null
		};
	}

	[ReactiveCommand]
	private void ToggleSelectAll(bool? isChecked)
	{
		if (isChecked is not bool check)
			return;

		foreach (var modVM in SourceMods)
		{
			if (!string.Equals(modVM.Code, "pandora", StringComparison.OrdinalIgnoreCase))
			{
				modVM.Active = check;
			}
		}
	}
	[ReactiveCommand]
	private async Task CopyText()
	{
		await DoSetClipboardTextAsync(LogText);
	}
	private async Task DoSetClipboardTextAsync(string? text)
	{
		if (Application.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop ||
			desktop.MainWindow?.Clipboard is not { } provider)
			throw new NullReferenceException("Missing Clipboard instance.");

		await provider.SetTextAsync(text);
	}
	[ReactiveCommand]
	private async Task ShowAboutDialogAsync() =>
		await ShowAboutDialog.Handle(new AboutDialogViewModel());

	[ReactiveCommand(CanExecute = nameof(EngineRunning))]
	private async void SetEngineConfig(IEngineConfigurationFactory? config)
	{
		if (config == null) return;
		_configService.SetCurrentFactory(config);
		await preloadTask;
		var newConfig = _configService.GetCurrentFactory()?.Config;
		Engine = newConfig != null ? new BehaviourEngine(newConfig) : Engine;
		Engine.SetOutputPath(currentDirectory);
		preloadTask = Engine.PreloadAsync();
	}

	[ReactiveCommand(CanExecute = nameof(EngineRunning))]
	private async Task LaunchEngine()
	{
		_logBuilder.Clear();

		_logBuilder.AppendLine($"Engine launched with configuration: {Engine.Configuration.Name}. Do not exit before the launch is finished.");
		_logBuilder.AppendLine("Waiting for preload to finish...");

		Stopwatch timer = Stopwatch.StartNew();
		await preloadTask;
		_logBuilder.AppendLine("Preload finished.");

		var activeMods = ModService.GetActiveModsByPriority(SourceMods);
		bool success = await Task.Run(() => Engine.LaunchAsync(activeMods));

		timer.Stop();

		_logBuilder.AppendLine(Engine.GetMessages(success));

		if (success)
		{
			_logBuilder.AppendLine($"Launch finished in {timer.Elapsed.TotalSeconds:F2} seconds.");
			_modService.SaveActiveMods(activeMods);

			if (closeOnFinish && Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime lifetime)
			{
				lifetime.Shutdown();
			}
		}
		else
		{
			_logBuilder.AppendLine("Launch aborted. Existing output was not cleared, and current patch list will not be saved.");
		}
		var newConfig = _configService.GetCurrentFactory().Config;
		Engine = newConfig != null ? new BehaviourEngine(newConfig) : new BehaviourEngine();

		Engine.SetOutputPath(currentDirectory);
		preloadTask = Task.Run(Engine.PreloadAsync);
	}
}
