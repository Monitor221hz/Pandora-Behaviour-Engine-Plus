using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using DynamicData;
using DynamicData.Binding;
using FluentAvalonia.UI.Controls;
using Pandora.API.Patch;
using Pandora.API.Patch.Engine.Config;
using Pandora.Core;
using Pandora.Core.Engine.Configs;
using Pandora.MVVM.Data;
using Pandora.Services;
using Pandora.Views;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization; 
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;


namespace Pandora.ViewModels;

public partial class EngineViewModel : ViewModelBase, IActivatableViewModel
{
    public ViewModelActivator Activator { get; }

    private readonly HashSet<string> startupArguments = new(StringComparer.OrdinalIgnoreCase);

    private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

    private bool closeOnFinish = false;
    private bool autoRun = false;

    public BehaviourEngine Engine { get; private set; } = new BehaviourEngine();

    public ModService _modService { get; private set; }

    private readonly StringBuilder _logBuilder = new();

    [Reactive] private bool _engineRunning = false;
    [Reactive] private bool _menuEnabled = false;
    [Reactive] private string _logText = string.Empty;
    [Reactive] private string _searchTerm = string.Empty;

    [Reactive] private ObservableCollection<IEngineConfigurationViewModel> _engineConfigurationViewModels = [];

    [BindableDerivedList] private readonly ReadOnlyObservableCollection<ModInfoViewModel> _modViewModels;
    public ObservableCollectionExtended<ModInfoViewModel> SourceMods { get; }

    [ObservableAsProperty(ReadOnly = false)] private bool? _allSelected;

    private IObservable<bool> _canLaunchEngine;

    private DirectoryInfo launchDirectory = BehaviourEngine.AssemblyDirectory;
    private DirectoryInfo currentDirectory = BehaviourEngine.SkyrimGameDirectory ?? BehaviourEngine.CurrentDirectory;

    private Task preloadTask;

    private IEngineConfigurationFactory engineConfigurationFactory;

    private static readonly char[] menuPathSeparators = ['/', '\\'];

    public EngineViewModel()
    {
        startupArguments = Environment.GetCommandLineArgs().ToHashSet(StringComparer.OrdinalIgnoreCase);

        _modService = new ModService(Path.Combine(launchDirectory.FullName, "Pandora_Engine", "ActiveMods.txt"));

        SourceMods = new ObservableCollectionExtended<ModInfoViewModel>();
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
            _canLaunchEngine = this
                .WhenAnyValue(x => x.EngineRunning)
                .Select(running => !running);

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
        });

        SetupCultureInfo();
        ReadStartupArguments();

        preloadTask = Task.Run(Engine.PreloadAsync);
        Engine.SetOutputPath(currentDirectory);

        if (autoRun) LaunchEngineCommand.Execute(Unit.Default);
    }
    private Func<ModInfoViewModel, bool> BuildFilter(string searchText) =>
        mod => string.IsNullOrEmpty(searchText) || mod.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase);

    private static void SetupCultureInfo()
    {
        CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");

        CultureInfo.DefaultThreadCurrentCulture = culture;
        CultureInfo.DefaultThreadCurrentUICulture = culture;
        CultureInfo.CurrentCulture = culture;
    }

    private void SetupExternalConfigurationPlugin(IEngineConfigurationPlugin injection)
    {
        if (string.IsNullOrEmpty(injection.MenuPath))
        {
            EngineConfigurationViewModels.Add(new EngineConfigurationViewModel(injection.Factory, SetEngineConfigCommand));
            return;
        }

        string[] pathSegments = injection.MenuPath.Split(menuPathSeparators);
        EngineConfigurationViewModelContainer? container = null;
        int index = 0;
        container = EngineConfigurationViewModels
            .Where(vm => vm.Name.Equals(pathSegments[index], StringComparison.OrdinalIgnoreCase)).FirstOrDefault() as EngineConfigurationViewModelContainer;
        if (container == null)
        {
            container = new(pathSegments[index]);
            EngineConfigurationViewModels.Add(container);
        }
        index++;
        while (pathSegments.Length > index)
        {
            if (container.NestedViewModels
                .Where(vm => vm.Name.Equals(pathSegments[index], StringComparison.OrdinalIgnoreCase)).FirstOrDefault() is not EngineConfigurationViewModelContainer tempContainer)
            {
                tempContainer = new EngineConfigurationViewModelContainer(pathSegments[index]);
                container.NestedViewModels.Add(tempContainer);
            }
            container = tempContainer;
            index++;
        }
        container.NestedViewModels.Add(new EngineConfigurationViewModel(injection.Factory, SetEngineConfigCommand));
    }
    private async Task SetupConfigurationOptions()
    {
        engineConfigurationFactory = new EngineConfigurationViewModel(new ConstEngineConfigurationFactory<SkyrimConfiguration>("Normal"), SetEngineConfigCommand);

        EngineConfigurationViewModels.Add(
        new EngineConfigurationViewModelContainer("Skyrim 64",
            new EngineConfigurationViewModelContainer("Behavior",
                new EngineConfigurationViewModelContainer("Patch",
                    (EngineConfigurationViewModel)engineConfigurationFactory,
                    new EngineConfigurationViewModel(new ConstEngineConfigurationFactory<SkyrimDebugConfiguration>("Debug"), SetEngineConfigCommand)
                )
                    //,
                    //new EngineConfigurationViewModelContainer("Convert"

                    //),
                    //new EngineConfigurationViewModelContainer("Validate"
                    //)
                    )
                )
        );

        foreach (var configPlugin in BehaviourEngine.EngineConfigurations)
        {
            SetupExternalConfigurationPlugin(configPlugin);
        }
        if (BehaviourEngine.EngineConfigurations.Count > 0)
        {
            _logBuilder.AppendLine("Plugins loaded.");
        }
        MenuEnabled = true;
        //EngineConfigs.Add(new EngineConfigurationViewModel<SkyrimConfiguration>("Skyrim SE/AE", SetEngineConfigCommand));
        //EngineConfigs.Add(new EngineConfigurationViewModel<SkyrimDebugConfiguration>("Skyrim SE/AE Debug", SetEngineConfigCommand));
    }
    public async Task LoadAsync()
    {
        SourceMods.Clear();
        
        var pluginsTask = SetupConfigurationOptions();
        var modInfoList = await _modService.LoadModsAsync(launchDirectory, currentDirectory);

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

        _modService.AssignModPrioritiesFromViewModels(SourceMods);

        await pluginsTask;
        _logBuilder.AppendLine("Mods loaded.");
    }

    private void ReadStartupArguments()
    {
        if (startupArguments.Remove("-skyrimDebug64"))
        {
            engineConfigurationFactory = new EngineConfigurationViewModel(new ConstEngineConfigurationFactory<SkyrimDebugConfiguration>("Debug"), SetEngineConfigCommand);
            Engine = new BehaviourEngine(engineConfigurationFactory.Config);
        }
        if (startupArguments.Remove("-autoClose"))
        {
            closeOnFinish = true;
        }
        foreach (var arg in startupArguments)
        {
            if (arg.StartsWith("-o:", StringComparison.OrdinalIgnoreCase))
            {
                var argArr = arg.AsSpan();
                var pathArr = argArr.Slice(3);
                var path = pathArr.Trim().ToString();
                currentDirectory = new DirectoryInfo(path);
                continue;
            }
        }
        if (startupArguments.Remove("-autorun"))
        {
            closeOnFinish = true;
            autoRun = true;
        }
    }

    private bool? AllSelectedCheckBoxHelper(IReadOnlyCollection<ModInfoViewModel> query)
    {
        var filtered = query.Where(x => !string.Equals(x.Code, "pandora", StringComparison.OrdinalIgnoreCase)).ToList();

        if (query.Count == 0)
            return false;

        var selectedCount = filtered.Count(x => x.Active);

        return selectedCount switch
        {
            0 => false,
            var count when count == filtered.Count => true,
            _ => null
        };
    }


    [ReactiveCommand(CanExecute = nameof(_canLaunchEngine))]
    private async void SetEngineConfig(IEngineConfigurationFactory? config)
    {
        if (config == null) return;
        engineConfigurationFactory = config;
        await preloadTask;
        var newConfig = engineConfigurationFactory.Config;
        Engine = newConfig != null ? new BehaviourEngine(newConfig) : Engine;
        Engine.SetOutputPath(currentDirectory);
        preloadTask = Engine.PreloadAsync();
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

    [ReactiveCommand(CanExecute = nameof(_canLaunchEngine))]
    private async void LaunchEngine(object? parameter)
    {
        lock (_canLaunchEngine)
        {
            EngineRunning = true;
        }

        _logBuilder.Clear();

        var configInfoMessage = $"Engine launched with configuration: {Engine.Configuration.Name}. Do not exit before the launch is finished.";
        _logBuilder.AppendLine(configInfoMessage);

        _logBuilder.AppendLine("Waiting for preload to finish.");
        Stopwatch timer = Stopwatch.StartNew();
        await preloadTask;
        _logBuilder.AppendLine("Preload finished.");
        
        List<IModInfo> activeMods = _modService.GetActiveModsByPriority(SourceMods);

        bool success = false;
        await Task.Run(async () => { success = await Engine.LaunchAsync(activeMods); });

        timer.Stop();

        logger.Info(configInfoMessage);
        _logBuilder.AppendLine(Engine.GetMessages(success));

        if (!success)
        {
            _logBuilder.AppendLine($"Launch aborted. Existing output was not cleared, and current patch list will not be saved.");
        }
        else
        {
            _logBuilder.AppendLine($"Launch finished in {Math.Round(timer.ElapsedMilliseconds / 1000.0, 2)} seconds");
            await Task.Run(() => { _modService.SaveActiveMods(activeMods); });

            if (closeOnFinish)
            {
                if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime lifetime) lifetime.Shutdown();
            }
        }
        _logBuilder.AppendLine(string.Empty);
        var newConfig = engineConfigurationFactory.Config;
        Engine = newConfig != null ? new BehaviourEngine(newConfig) : new BehaviourEngine();

        Engine.SetOutputPath(currentDirectory);
        preloadTask = Task.Run(Engine.PreloadAsync);

        lock (_canLaunchEngine)
        {
            EngineRunning = false;
        }
    }
}
