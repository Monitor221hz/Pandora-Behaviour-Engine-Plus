using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using DynamicData;
using DynamicData.Binding;
using FluentAvalonia.UI.Controls;
using Pandora.API.Patch;
using Pandora.API.Patch.Engine.Config;
using Pandora.Command;
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
    private readonly NemesisModInfoProvider nemesisModInfoProvider = new NemesisModInfoProvider();
    private readonly PandoraModInfoProvider pandoraModInfoProvider = new PandoraModInfoProvider();

    private HashSet<string> startupArguments = new(StringComparer.OrdinalIgnoreCase);

    private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

    private bool closeOnFinish = false;
    private bool autoRun = false;

    public BehaviourEngine Engine { get; private set; } = new BehaviourEngine();

    public LogService _logService { get; private set; } = new LogService();

    public ObservableCollectionExtended<ModInfoViewModel> SourceMods { get; }

    public ViewModelActivator Activator { get; }

    [Reactive] private bool _engineRunning = false;
    [Reactive] private bool _menuEnabled = false;
    [Reactive] private string _logText = string.Empty;
    [Reactive] private string _searchTerm = string.Empty;

    [Reactive] private List<IModInfo> mods = [];
    [Reactive] private ObservableCollection<IEngineConfigurationViewModel> _engineConfigurationViewModels = [];

    [BindableDerivedList] private readonly ReadOnlyObservableCollection<ModInfoViewModel> _modViewModels;

    [ObservableAsProperty(ReadOnly = false)] private bool? _allSelected;

    private IObservable<bool> _canLaunchEngine;

    
    private FileInfo activeModConfig;

    private Dictionary<string, IModInfo> modsByCode = [];

    private bool modInfoCache = false;


    private DirectoryInfo launchDirectory = BehaviourEngine.AssemblyDirectory;
    private DirectoryInfo currentDirectory = BehaviourEngine.SkyrimGameDirectory ?? BehaviourEngine.CurrentDirectory;


    private Task preloadTask;


    private IEngineConfigurationFactory engineConfigurationFactory;

    private static readonly char[] menuPathSeparators = ['/', '\\'];

    public EngineViewModel()
    {
        startupArguments = Environment.GetCommandLineArgs().ToHashSet(StringComparer.OrdinalIgnoreCase);

        SourceMods = new ObservableCollectionExtended<ModInfoViewModel>();

        SourceMods.ToObservableChangeSet()
            .Filter(this.WhenAnyValue(x => x.SearchTerm)
                .Throttle(TimeSpan.FromMilliseconds(200))
                .Select(BuildFilter))
            .Sort(SortExpressionComparer<ModInfoViewModel>.Ascending(m => m.Priority))
            .ObserveOn(RxApp.MainThreadScheduler)
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
                .Subscribe(_ => LogText = _logService.LogText)
                .DisposeWith(disposables);
        });

        SetupCultureInfo();
        ReadStartupArguments();

        activeModConfig = new FileInfo(Path.Combine(launchDirectory.FullName, "Pandora_Engine", "ActiveMods.txt"));

        preloadTask = Task.Run(Engine.PreloadAsync);
        Engine.SetOutputPath(currentDirectory);

        if (autoRun) LaunchEngineCommand.Execute(Unit.Default);
    }

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
            .Where(vm => vm.Name.Equals(pathSegments[index], StringComparison.OrdinalIgnoreCase)).FirstOrDefault()
            as EngineConfigurationViewModelContainer;
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
            await _logService.WriteLineAsync("Plugins loaded.");
        }
        MenuEnabled = true;
        //EngineConfigs.Add(new EngineConfigurationViewModel<SkyrimConfiguration>("Skyrim SE/AE", SetEngineConfigCommand));
        //EngineConfigs.Add(new EngineConfigurationViewModel<SkyrimDebugConfiguration>("Skyrim SE/AE Debug", SetEngineConfigCommand));
    }
    public async Task LoadAsync()
    {
        SourceMods.Clear();
        Mods.Clear();
        modsByCode.Clear();
        var pluginsTask = SetupConfigurationOptions();
        List<IModInfo> modInfoList;
        {
            HashSet<IModInfo> modInfos = new();

            //Program folder
            LoadModFolder(modInfos, await nemesisModInfoProvider?.GetInstalledMods(launchDirectory + "\\Nemesis_Engine\\mod")!);
            LoadModFolder(modInfos, await pandoraModInfoProvider?.GetInstalledMods(launchDirectory + "\\Pandora_Engine\\mod")!);
            //Working folder, or Skyrim\Data folder
            LoadModFolder(modInfos, await nemesisModInfoProvider?.GetInstalledMods(BehaviourEngine.CurrentDirectory + "\\Nemesis_Engine\\mod")!);
            LoadModFolder(modInfos, await pandoraModInfoProvider?.GetInstalledMods(BehaviourEngine.CurrentDirectory + "\\Pandora_Engine\\mod")!);
            //Current (defaults to Working folder) or Output (set via -o) folder
            LoadModFolder(modInfos, await nemesisModInfoProvider?.GetInstalledMods(currentDirectory + "\\Nemesis_Engine\\mod")!);
            LoadModFolder(modInfos, await pandoraModInfoProvider?.GetInstalledMods(currentDirectory + "\\Pandora_Engine\\mod")!);

            modInfoList = [.. modInfos];
        }
        modInfoList.ForEach(a => modsByCode.Add(a.Code, a));

        modInfoCache = LoadActiveMods(modInfoList);

        modInfoList = modInfoList.OrderBy(m => m.Code == "pandora").ThenBy(m => m.Priority == 0).ThenBy(m => m.Priority).ThenBy(m => m.Name).ToList();

        //var stopwatch = Stopwatch.StartNew();
        Mods.AddRange(modInfoList);
        SourceMods.AddRange(modInfoList.Select(m => new ModInfoViewModel(m)));
        //stopwatch.Stop();
        //await _logService.WriteLineAsync($"Создание ViewModels заняло {stopwatch.ElapsedMilliseconds} мс");

        AssignModPrioritiesFromViewModels(SourceMods);

        await pluginsTask;
        await _logService.WriteLineAsync("Mods loaded.");
    }

    private static void LoadModFolder(HashSet<IModInfo> modInfos, List<IModInfo> mods)
    {
        if (mods == null) return;

        foreach (var mod in mods)
        {
            modInfos.Add(mod);
        }
    }

    private Func<ModInfoViewModel, bool> BuildFilter(string searchText) => 
        mod => string.IsNullOrEmpty(searchText) || mod.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase);
    

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
    private bool LoadActiveMods(List<IModInfo> loadedMods)
    {
        if (!activeModConfig.Exists) return false;
        foreach (var mod in loadedMods)
        {
            if (mod == null) continue;
            mod.Active = false;
        }
        using (var readStream = activeModConfig.OpenRead())
        {
            using (var streamReader = new StreamReader(readStream))
            {
                string? expectedLine;
                uint priority = 0;
                while ((expectedLine = streamReader.ReadLine()) != null)
                {
                    if (!modsByCode.TryGetValue(expectedLine, out IModInfo? modInfo)) continue;
                    priority++;
                    modInfo.Priority = priority;
                    modInfo.Active = true;
                }
            }


        }
        return true;
    }
    private void SaveActiveMods(List<IModInfo> activeMods)
    {
        activeModConfig.Directory?.Create();
        using (var writeStream = activeModConfig.Create())
        {
            using (StreamWriter streamWriter = new StreamWriter(writeStream))
            {
                foreach (var modInfo in activeMods)
                {
                    streamWriter.WriteLine(modInfo.Code);
                }
            }
        }
    }
    public void AssignModPrioritiesFromViewModels(IEnumerable<ModInfoViewModel> modViewModels)
    {
        uint priority = 0;
        foreach (var modViewModel in modViewModels)
        {
            priority++;
            modViewModel.Priority = priority;
        }
    }
    private List<IModInfo> AssignModPriorities(List<IModInfo> mods)
    {
        uint priority = 0;
        foreach (var mod in mods)
        {
            priority++;
            mod.Priority = priority;
        }

        return mods;
    }

    private List<IModInfo> GetActiveModsByPriority() => Mods.Where(m => m.Active).OrderBy(m => m.Code == "pandora").ThenBy(m => m.Priority == 0).ThenBy(m => m.Priority).ToList();

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
        if (isChecked is not bool check) return;

        foreach (var mod in SourceMods)
        {
            mod.Active = check;
        }
    }

    private bool? AllSelectedCheckBoxHelper(IReadOnlyCollection<ModInfoViewModel> query)
    {
        if (query.Count == 0)
            return false;

        var selectedCount = query.Count(x => x.Active);

        return selectedCount switch
        {
            0 => false,
            var count when count == query.Count => true,
            _ => null
        };
    }

    [ReactiveCommand(CanExecute = nameof(_canLaunchEngine))]
    private async void LaunchEngine(object? parameter)
    {
        lock (_canLaunchEngine)
        {
            EngineRunning = true;
        }

        await _logService.ClearAsync();

        var configInfoMessage = $"Engine launched with configuration: {Engine.Configuration.Name}. Do not exit before the launch is finished.";
        await _logService.WriteLineAsync(configInfoMessage);

        await _logService.WriteLineAsync("Waiting for preload to finish.");
        Stopwatch timer = Stopwatch.StartNew();
        await preloadTask;
        await _logService.WriteLineAsync("Preload finished.");
        //AssignModPrioritiesFromViewModels(SourceMods);
        List<IModInfo> activeMods = GetActiveModsByPriority();

        IModInfo? baseModInfo = Mods.Where(m => m.Code == "pandora").FirstOrDefault();

        if (baseModInfo == null)
        {
            await _logService.WriteLineAsync("FATAL ERROR: Pandora Base does not exist. Ensure the engine was installed properly and data is not corrupted.");
            return;
        }
        if (!baseModInfo.Active)
        {
            baseModInfo.Active = true;
            activeMods.Add(baseModInfo);
        }
        baseModInfo.Priority = uint.MaxValue;

        bool success = false;
        await Task.Run(async () => { success = await Engine.LaunchAsync(activeMods); });


        timer.Stop();

        logger.Info(configInfoMessage);
        await _logService.WriteLineAsync(Engine.GetMessages(success));

        if (!success)
        {
            await _logService.WriteLineAsync($"Launch aborted. Existing output was not cleared, and current patch list will not be saved.");
        }
        else
        {
            await _logService.WriteLineAsync($"Launch finished in {Math.Round(timer.ElapsedMilliseconds / 1000.0, 2)} seconds");
            await Task.Run(() => { SaveActiveMods(activeMods); });

            if (closeOnFinish)
            {
                if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime lifetime) lifetime.Shutdown();
            }
        }
        await _logService.WriteLineAsync(string.Empty);
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
