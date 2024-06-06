using Pandora.MVVM.Data;

using Pandora.Command;
using Pandora.Core;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Pandora.MVVM.View.Controls;
using System;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Threading.Channels;
using System.Windows.Media.Animation;
using System.IO;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using Pandora.Core.Engine.Configs;
using System.Security.Policy;


namespace Pandora.MVVM.ViewModel
{
    public class EngineViewModel : INotifyPropertyChanged
    {
        private readonly NemesisModInfoProvider nemesisModInfoProvider = new NemesisModInfoProvider();
        private readonly PandoraModInfoProvider pandoraModInfoProvider = new PandoraModInfoProvider();

		
        private HashSet<string> startupArguments = new(StringComparer.OrdinalIgnoreCase);

        public event PropertyChangedEventHandler? PropertyChanged;

		private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

		public bool? DialogResult { get; set; } = true;
        private bool closeOnFinish = false;
        private bool autoRun = false; 
        public BehaviourEngine Engine { get; private set; } = new BehaviourEngine();

        public RelayCommand LaunchCommand { get; }
        public RelayCommand SetEngineConfigCommand { get; }
        public RelayCommand ExitCommand { get; }

        private List<IModInfo>  hiddenMods = new List<IModInfo>();
        public ObservableCollection<IModInfo> Mods { get; set; } = new ObservableCollection<IModInfo>();

	
		public bool LaunchEnabled { get; set; } = true;

		public string SearchBGText { get => searchBGText; set
            {
                searchBGText = value;
                RaisePropertyChanged(nameof(SearchBGText));
            }
        }

		private bool engineRunning = false;

        private FileInfo activeModConfig; 

        private Dictionary<string, IModInfo> modsByCode = new Dictionary<string, IModInfo>();

        private bool modInfoCache = false;

        private static DirectoryInfo currentDirectory = new DirectoryInfo(Directory.GetCurrentDirectory());

        private Task preloadTask;

        private ObservableCollection<IEngineConfigurationViewModel> engineConfigs = new ObservableCollection<IEngineConfigurationViewModel>();
		

		public ObservableCollection<IEngineConfigurationViewModel> EngineConfigurationViewModels 
        { get => engineConfigs; 
            set 
            { 
                engineConfigs = value; 
                RaisePropertyChanged(nameof(EngineConfigurationViewModels));
            } 
        }
        private IEngineConfigurationFactory engineConfigurationFactory;
		private string logText = "";
		public string LogText { 
            get => logText;
            set
            {
                logText = value;
                RaisePropertyChanged(nameof(LogText));
            }
        }
        private string cachedSearchText = string.Empty;
        private string searchText = "";
		private string searchBGText = "Search";

		public string SearchText { get => searchText; 
            set 
            { 
                
                if (String.IsNullOrEmpty(value))
                {
					AssignModPriorities(Mods.Where(m => m.Active).ToList());
					var sortedModInfos = Mods.Concat(hiddenMods).OrderBy(m => m.Priority == 0).ThenBy(m => m.Priority).ToList();
					hiddenMods.Clear();
                    Mods.Clear();
                    foreach(var mod in sortedModInfos)
                    {
                        Mods.Add(mod);
                    }
                    SearchBGText = cachedSearchText;
                    cachedSearchText = string.Empty;
				}
                else
                {
					SearchBGText = string.Empty;
					HashSet<IModInfo> foundMods = SearchModInfos(value, Mods).ToHashSet();
					for (int i = Mods.Count - 1; i >= 0; i--)
					{
						IModInfo mod = Mods[i];
						if (foundMods.Contains(mod))
						{
							continue;
						}
						hiddenMods.Add(mod);
						Mods.RemoveAt(i);
					}
					foundMods = SearchModInfos(value, hiddenMods).ToHashSet();
					for (int i = hiddenMods.Count - 1; i >= 0; i--)
					{
						IModInfo? mod = hiddenMods[i];
						if (!foundMods.Contains(mod))
						{
							continue;
						}
						Mods.Add(mod);
						hiddenMods.RemoveAt(i);
					}
				}
                if (cachedSearchText.Length < value.Length)
                {
                    cachedSearchText = value;
                }
				searchText = value;
				RaisePropertyChanged(nameof(SearchText));
            } 
        }
        private IEnumerable<IModInfo> SearchModInfos(string searchText, IEnumerable<IModInfo> mods)
        {
            return mods.Where(m => m.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase));

		}
		private void RaisePropertyChanged([CallerMemberName] string? propertyName=null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public EngineViewModel()
        {
            startupArguments = Environment.GetCommandLineArgs().ToHashSet(StringComparer.OrdinalIgnoreCase);
            LaunchCommand = new RelayCommand(LaunchEngine, CanLaunchEngine);
            ExitCommand = new RelayCommand(Exit);
            SetEngineConfigCommand = new RelayCommand(SetEngineConfiguration, CanLaunchEngine);
			activeModConfig = new FileInfo($"{currentDirectory}\\Pandora_Engine\\ActiveMods.txt");
			CultureInfo culture;

			culture = CultureInfo.CreateSpecificCulture("en-US");

			CultureInfo.DefaultThreadCurrentCulture = culture;
			CultureInfo.DefaultThreadCurrentUICulture = culture;
			CultureInfo.CurrentCulture = culture;
			ReadStartupArguments();
            SetupConfigurationOptions();
			preloadTask = Task.Run(Engine.PreloadAsync);

			if (autoRun) { LaunchCommand.Execute(null); }


		}
        private void SetupConfigurationOptions()
        {
            engineConfigurationFactory = new EngineConfigurationViewModel<SkyrimConfiguration>("Normal", SetEngineConfigCommand);

			EngineConfigurationViewModels.Add(
                new EngineConfigurationViewModelContainer("Skyrim SE/AE",
                    new EngineConfigurationViewModelContainer("Behavior", 

                        new EngineConfigurationViewModelContainer("Patch",
                            (EngineConfigurationViewModel<SkyrimConfiguration>)engineConfigurationFactory,
                            new EngineConfigurationViewModel<SkyrimDebugConfiguration>("Debug", SetEngineConfigCommand)
                        )
                        //,
                        //new EngineConfigurationViewModelContainer("Convert"
                            
                        //),
                        //new EngineConfigurationViewModelContainer("Validate"
                        //)
					    )
				    )
                );
			
			//EngineConfigs.Add(new EngineConfigurationViewModel<SkyrimConfiguration>("Skyrim SE/AE", SetEngineConfigCommand));
			//EngineConfigs.Add(new EngineConfigurationViewModel<SkyrimDebugConfiguration>("Skyrim SE/AE Debug", SetEngineConfigCommand));
		}
        public async Task LoadAsync()
        {
			

			List<IModInfo> modInfos = new List<IModInfo>();

			modInfos.AddRange(await nemesisModInfoProvider?.GetInstalledMods(currentDirectory + "\\Nemesis_Engine\\mod")!);
			modInfos.AddRange(await pandoraModInfoProvider?.GetInstalledMods(currentDirectory + "\\Pandora_Engine\\mod")!);


			for (int i = 0; i < modInfos.Count; i++)
            {
				IModInfo? modInfo = modInfos[i];
				//Mods.Add(modInfo);
				IModInfo? existingModInfo;
                if (modsByCode.TryGetValue(modInfo.Code, out existingModInfo))
                {
                    logger.Warn($"Engine > Folder {modInfo.Folder.Parent?.Name} > Parse Info > {modInfo.Code} Already Exists > SKIPPED");
                    modInfos.RemoveAt(i);
                    continue;
                }
                modsByCode.Add(modInfo.Code, modInfo);
            }

            modInfoCache = LoadActiveMods(modInfos);

            modInfos = modInfos.OrderBy(m => m.Priority == 0).ThenBy(m => m.Priority).ToList();

            foreach(var modInfo in modInfos) { Mods.Add(modInfo);  }
            await WriteLogBoxLine("Mods loaded.");
		}

        public void Exit(object? p)
        {
            App.Current.MainWindow.Close();
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
				Engine = new BehaviourEngine(new SkyrimDebugConfiguration());
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
                    Engine.Configuration.Patcher.SetOutputPath(pathArr.Trim().ToString());
                    continue;
                }
    //            if (arg.Equals("-sseDebug", StringComparison.OrdinalIgnoreCase))
    //            {
    //                Engine = new BehaviourEngine(new SkyrimDebugConfiguration());
    //                continue;
    //            }
    //            if (arg.Equals("-autorun", StringComparison.OrdinalIgnoreCase))
    //            {
				//	closeOnFinish = true;
				//	launchImmediate = true;
    //                continue;
				//}
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


            if (activeModConfig.Exists) { activeModConfig.Delete(); }
			
            
            using (var writeStream = activeModConfig.OpenWrite())
            {
                using (StreamWriter streamWriter = new StreamWriter(writeStream)) 
                { 
                    foreach(var modInfo in activeMods)
                    {
                        streamWriter.WriteLine(modInfo.Code);
                    }
                }
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

        private List<IModInfo> GetActiveModsByPriority() => AssignModPriorities(Mods.Where(m => m.Active).ToList());
		private async void SetEngineConfiguration(object? config)
		{
			if (config == null) { return; }
			engineConfigurationFactory = (IEngineConfigurationFactory)config;
            await preloadTask;
            var newConfig = engineConfigurationFactory.Config;
			Engine = newConfig != null ? new BehaviourEngine(newConfig) : Engine;
		}
		private async void LaunchEngine(object? parameter)
        {
			lock (LaunchCommand)
			{
				engineRunning = true;
				LaunchEnabled = !engineRunning;
			}
			
            logText= string.Empty;

			var configInfoMessage = $"Engine launched with configuration: {Engine.Configuration.Name}";
			await WriteLogBoxLine(configInfoMessage);
			await preloadTask;
			
			List<IModInfo> activeMods = GetActiveModsByPriority();

            IModInfo? baseModInfo = Mods.Where(m => m.Code == "pandora").FirstOrDefault();

            if (baseModInfo == null) { await WriteLogBoxLine("FATAL ERROR: Pandora Base does not exist. Ensure the engine was installed properly and data is not corrupted."); return; }
			if (!baseModInfo.Active)
			{
				baseModInfo.Active = true;
				activeMods.Add(baseModInfo);
			}
			baseModInfo.Priority = uint.MaxValue;



			Stopwatch timer = Stopwatch.StartNew();
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
                    System.Windows.Application.Current.Shutdown(); 
                }
			}



			var newConfig = engineConfigurationFactory.Config;
            Engine = newConfig != null ? new BehaviourEngine(newConfig) : new BehaviourEngine();
			preloadTask = Task.Run(Engine.PreloadAsync);

			lock (LaunchCommand)
            {
				engineRunning = false;
				LaunchEnabled = !engineRunning;
			}

        }

        private bool CanLaunchEngine(object? parameter)
        {

            return !engineRunning;
            
        }
    }
}
