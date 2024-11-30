using Pandora.MVVM.Data;
using Avalonia.Collections;
using Pandora.Command;
using Pandora.Core;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Globalization; 
using System.Threading;
using Pandora.Core.Engine.Configs;
using System.Security.Policy;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia;
using Pandora.API.Patch;
using Pandora.API.Patch.Engine.Config;
using System.Reflection;
using System.Xml.Linq;
using FluentAvalonia.UI.Controls;
using Pandora.Views;
using Avalonia.Controls;


namespace Pandora.ViewModels
{
    public class EngineViewModel : ViewModelBase
    {
        private readonly NemesisModInfoProvider nemesisModInfoProvider = new NemesisModInfoProvider();
        private readonly PandoraModInfoProvider pandoraModInfoProvider = new PandoraModInfoProvider();

        private HashSet<string> startupArguments = new(StringComparer.OrdinalIgnoreCase);

		private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

		public bool? DialogResult { get; set; } = true;
        private bool closeOnFinish = false;
        private bool autoRun = false; 
        public BehaviourEngine Engine { get; private set; } = new BehaviourEngine();

        public RelayCommand LaunchCommand { get; }
        public RelayCommand SetEngineConfigCommand { get; }
        public RelayCommand ExitCommand { get; }

        public RelayCommand ToggleAllCommand { get; }

        private List<IModInfo> mods = new();
		public List<IModInfo> Mods { 
            get => mods; 
            set
            {
                SetProperty(ref mods, value); 
			}
        }
        private List<ModInfoViewModel> hiddenModViewModels = new();
        private ObservableCollection<ModInfoViewModel> modViewModels = new(); 
		public ObservableCollection<ModInfoViewModel> ModViewModels 
        { 
            get => modViewModels; 
            set
            {
                SetProperty(ref modViewModels, value);
            }
        }

		public bool LaunchEnabled { get; set; } = true;

		public string SearchBGText { get => searchBGText; set
            {
                SetProperty(ref searchBGText, value);
            }
        }

		private bool engineRunning = false;
		public bool MenuEnabled 
        { 
            get => menuEnabled; 
            private set
            { 
                SetProperty(ref menuEnabled, value);
            } 
        }
		public bool EngineRunning
        {
            get => engineRunning; 
            set
            {
                SetProperty(ref engineRunning, value); 
            }
        }
        private FileInfo activeModConfig; 

        private Dictionary<string, IModInfo> modsByCode = new Dictionary<string, IModInfo>();

        private bool modInfoCache = false;
        private static readonly DirectoryInfo workingDirectory = new DirectoryInfo(Directory.GetCurrentDirectory());

        private DirectoryInfo currentDirectory = BehaviourEngine.SkyrimGameDirectory ?? workingDirectory;

        private Task preloadTask;

        private ObservableCollection<IEngineConfigurationViewModel> engineConfigs = new ObservableCollection<IEngineConfigurationViewModel>();
		

		public ObservableCollection<IEngineConfigurationViewModel> EngineConfigurationViewModels 
        {
            get => engineConfigs; 
            set 
            {
                SetProperty(ref engineConfigs, value); 
            } 
        }
        private IEngineConfigurationFactory engineConfigurationFactory;
		private string logText = "";
		public string LogText { 
            get => logText;
            set
            {
                SetProperty(ref logText, value); 
            }
        }
        private string cachedSearchText = string.Empty;
        private string searchText = "";
		private string searchBGText = "Search";
		private bool menuEnabled = false;

		public string SearchText { get => searchText; 
            set 
            { 
                
                if (String.IsNullOrEmpty(value))
                {
					AssignModPriorities(Mods.Where(m => m.Active).ToList());
					var sortedViewModels = ModViewModels.Concat(hiddenModViewModels).OrderBy(m => m.Priority == 0).ThenBy(m => m.Priority).ToList();
					ModViewModels.Clear(); 
                    hiddenModViewModels.Clear();
                    foreach(var modViewModel in sortedViewModels)
                    {
                        ModViewModels.Add(modViewModel); 
                    }
					SearchBGText = cachedSearchText;
                    cachedSearchText = string.Empty;
				}
                else
                {
					SearchBGText = string.Empty;
					HashSet<ModInfoViewModel> foundMods = SearchModViewModels(value, ModViewModels).ToHashSet();
					for (int i = ModViewModels.Count - 1; i >= 0; i--)
					{
						var modViewModel = ModViewModels[i];
						if (foundMods.Contains(modViewModel))
						{
							continue;
						}
                        hiddenModViewModels.Add(modViewModel);
						ModViewModels.RemoveAt(i);
					}
					foundMods = SearchModViewModels(value, hiddenModViewModels).ToHashSet();
					for (int i = hiddenModViewModels.Count - 1; i >= 0; i--)
					{
						var modViewModel = hiddenModViewModels[i];
						if (!foundMods.Contains(modViewModel))
						{
							continue;
						}
						ModViewModels.Add(modViewModel);
						hiddenModViewModels.RemoveAt(i);
					}
				}
                if (cachedSearchText.Length < value.Length)
                {
                    cachedSearchText = value;
                }
                SetProperty(ref searchText, value); 
            } 
        }

		private static readonly char[] menuPathSeparators = new char[] { '/', '\\' };

		public void SortMods()
		{
            Mods = Mods.OrderBy(m => m.Code == "pandora").ThenBy(m => m.Priority == 0).ThenBy(m => m.Priority).ToList(); 
		}
		private IEnumerable<ModInfoViewModel> SearchModViewModels(string searchText, IEnumerable<ModInfoViewModel> modViewModels)
        {
            return modViewModels.Where(m => m.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase));

		}
        public EngineViewModel()
        {
            startupArguments = Environment.GetCommandLineArgs().ToHashSet(StringComparer.OrdinalIgnoreCase);
            LaunchCommand = new RelayCommand(LaunchEngine, CanLaunchEngine);
            ExitCommand = new RelayCommand(Exit);
            SetEngineConfigCommand = new RelayCommand(SetEngineConfiguration, CanLaunchEngine);
            ToggleAllCommand = new RelayCommand(ToggleSelectAll);

            CultureInfo culture;

			culture = CultureInfo.CreateSpecificCulture("en-US");

			CultureInfo.DefaultThreadCurrentCulture = culture;
			CultureInfo.DefaultThreadCurrentUICulture = culture;
			CultureInfo.CurrentCulture = culture;
			ReadStartupArguments();
			activeModConfig = new FileInfo($"{currentDirectory}\\Pandora_Engine\\ActiveMods.txt");
			preloadTask = Task.Run(Engine.PreloadAsync);
			Engine.SetOutputPath(currentDirectory);
			if (autoRun) { LaunchCommand.Execute(null); }


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
				var tempContainer = container.NestedViewModels
				.Where(vm => vm.Name.Equals(pathSegments[index], StringComparison.OrdinalIgnoreCase)).FirstOrDefault()
as EngineConfigurationViewModelContainer;
				if (tempContainer == null)
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
                await WriteLogBoxLine("Plugins loaded.");
            }
            MenuEnabled = true;
			//EngineConfigs.Add(new EngineConfigurationViewModel<SkyrimConfiguration>("Skyrim SE/AE", SetEngineConfigCommand));
			//EngineConfigs.Add(new EngineConfigurationViewModel<SkyrimDebugConfiguration>("Skyrim SE/AE Debug", SetEngineConfigCommand));
		}
        public async Task LoadAsync()
        {
            var launchDirectory = BehaviourEngine.AssemblyDirectory.FullName;
            ModViewModels.Clear(); 
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
                LoadModFolder(modInfos, await nemesisModInfoProvider?.GetInstalledMods(workingDirectory + "\\Nemesis_Engine\\mod")!);
                LoadModFolder(modInfos, await pandoraModInfoProvider?.GetInstalledMods(workingDirectory + "\\Pandora_Engine\\mod")!);
                //Current (defaults to Working folder) or Output (set via -o) folder
                LoadModFolder(modInfos, await nemesisModInfoProvider?.GetInstalledMods(currentDirectory + "\\Nemesis_Engine\\mod")!);
                LoadModFolder(modInfos, await pandoraModInfoProvider?.GetInstalledMods(currentDirectory + "\\Pandora_Engine\\mod")!);

                modInfoList = [.. modInfos];
            }
            modInfoList.ForEach(a => modsByCode.Add(a.Code, a));

            modInfoCache = LoadActiveMods(modInfoList);

            modInfoList = modInfoList.OrderBy(m => m.Code == "pandora").ThenBy(m => m.Priority == 0).ThenBy(m => m.Priority).ThenBy(m => m.Name).ToList();

            foreach (var modInfo in modInfoList)
            {
                Mods.Add(modInfo);
                ModViewModels.Add(new(modInfo)); 
            }
            await pluginsTask;
			await WriteLogBoxLine("Mods loaded.");
		}

        private static void LoadModFolder(HashSet<IModInfo> modInfos, IList<IModInfo> mods) {
            if (mods == null) { return; }

            foreach (var mod in mods)
            {
                if (!modInfos.Add(mod))
                {
                    logger.Warn($"Engine > Folder {mod.Folder.Parent?.Name} > Parse Info > {mod.Code} Already Exists > SKIPPED");
                }
            }
        }

        public void Exit(object? p)
        {
            //App.Current.MainWindow.Close();
        }
        internal async Task ClearLogBox() => LogText = "";
        internal async Task WriteLogBoxLine(string text)
        {
            StringBuilder sb = new StringBuilder(LogText);
            if (LogText.Length > 0) sb.Append(Environment.NewLine);
            sb.Append(text);
            LogText = sb.ToString();
        }
        internal async Task WriteLogBox(string text)
        {
            StringBuilder sb = new StringBuilder(LogText);
            sb.Append(text);
            LogText = sb.ToString();
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
            foreach(var arg in startupArguments)
            {
                if (arg.StartsWith("-o:", StringComparison.OrdinalIgnoreCase))
                {
                    var argArr = arg.AsSpan();
                    var pathArr = argArr.Slice(3);
                    var path = pathArr.Trim().ToString();
                    currentDirectory = new DirectoryInfo(path);
                    Engine.SetOutputPath(currentDirectory);
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
            foreach(var mod in loadedMods) 
            { 
                if (mod == null) continue;
                mod.Active = false;  
            }
            using (var readStream = activeModConfig.OpenRead())
            {
                using (var streamReader  = new StreamReader(readStream))
                {
					string? expectedLine;
                    uint priority = 0; 
                    while ((expectedLine = streamReader.ReadLine()) != null)
                    {
                        IModInfo? modInfo;
                        if (!modsByCode.TryGetValue(expectedLine, out modInfo)) continue;
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
            foreach(var modViewModel in modViewModels)
            {
                priority++;
                modViewModel.Priority = priority;
            }
        }
        private List<IModInfo> AssignModPriorities(List<IModInfo> mods)
        {
            uint priority = 0;
			foreach(var mod in mods)
            {
                priority++;
                mod.Priority = priority; 
            }

            return mods; 
        }

        private List<IModInfo> GetActiveModsByPriority() => Mods.Where(m => m.Active).OrderBy(m => m.Code == "pandora").ThenBy(m => m.Priority == 0).ThenBy(m => m.Priority).ToList();
		private async void SetEngineConfiguration(object? config)
		{
			if (config == null) { return; }
			engineConfigurationFactory = (IEngineConfigurationFactory)config;
            await preloadTask;
            var newConfig = engineConfigurationFactory.Config;
			Engine = newConfig != null ? new BehaviourEngine(newConfig) : Engine;
            Engine.SetOutputPath(currentDirectory);
            preloadTask = Engine.PreloadAsync();
		}
        private async void ToggleSelectAll(object? isChecked)
        {
            bool check = (bool)isChecked!;
            if (check)
            {
                foreach(var modViewModel in ModViewModels)
                {
                    modViewModel.Active = true; 
                }
            }
            else
            {
				foreach (var modViewModel in ModViewModels)
				{
					modViewModel.Active = false;
				}
			}
        }
		private async void LaunchEngine(object? parameter)
        {
			lock (LaunchCommand)
			{
				EngineRunning = true;
				LaunchEnabled = !EngineRunning;
			}
			
            logText= string.Empty;

			var configInfoMessage = $"Engine launched with configuration: {Engine.Configuration.Name}. Do not exit before the launch is finished.";
			await WriteLogBoxLine(configInfoMessage);
			await WriteLogBoxLine("Waiting for preload to finish.");
			Stopwatch timer = Stopwatch.StartNew();
			await preloadTask;
            await WriteLogBoxLine("Preload finished.");
            AssignModPrioritiesFromViewModels(ModViewModels); 
			List<IModInfo> activeMods = GetActiveModsByPriority();
            
            IModInfo? baseModInfo = Mods.Where(m => m.Code == "pandora").FirstOrDefault();

            if (baseModInfo == null) { await WriteLogBoxLine("FATAL ERROR: Pandora Base does not exist. Ensure the engine was installed properly and data is not corrupted."); return; }
			if (!baseModInfo.Active)
			{
				baseModInfo.Active = true;
				activeMods.Add(baseModInfo);
			}
			baseModInfo.Priority = uint.MaxValue;

            bool success = false;
			await Task.Run(async() => { success = await Engine.LaunchAsync(activeMods); }); 
            

            timer.Stop();

            logger.Info(configInfoMessage);
			await WriteLogBoxLine(Engine.GetMessages(success));

            if (!success)
            {
                await WriteLogBoxLine($"Launch aborted. Existing output was not cleared, and current patch list will not be saved.");
            }
            else
            {
				await WriteLogBoxLine($"Launch finished in {Math.Round(timer.ElapsedMilliseconds / 1000.0, 2)} seconds");
				await Task.Run(() => { SaveActiveMods(activeMods); });

                if (closeOnFinish) 
                {
					if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime lifetime) lifetime.Shutdown();
				}
			}
            await WriteLogBoxLine(string.Empty);
			var newConfig = engineConfigurationFactory.Config;
            Engine = newConfig != null ? new BehaviourEngine(newConfig) : new BehaviourEngine();

			Engine.SetOutputPath(currentDirectory);
			preloadTask = Task.Run(Engine.PreloadAsync);

			lock (LaunchCommand)
            {
				EngineRunning = false;
				LaunchEnabled = !EngineRunning;
			}

        }

        private bool CanLaunchEngine(object? parameter)
        {

            return !EngineRunning;
            
        }
    }
}
