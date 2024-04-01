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

		private string logText = "";
        private HashSet<string> startupArguments = new(StringComparer.OrdinalIgnoreCase);

        public event PropertyChangedEventHandler? PropertyChanged;

		private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

		public bool? DialogResult { get; set; } = true;
        private bool closeOnFinish = false; 

        public BehaviourEngine Engine { get; private set; } = new BehaviourEngine();

        public RelayCommand LaunchCommand { get; }
        public RelayCommand ExitCommand { get; }

        public ObservableCollection<IModInfo> Mods { get; set; } = new ObservableCollection<IModInfo>();

        public bool LaunchEnabled { get; set; } = true;



        private bool engineRunning = false;

        private FileInfo activeModConfig; 

        private Dictionary<string, IModInfo> modsByCode = new Dictionary<string, IModInfo>();

        private bool modInfoCache = false;

        private static DirectoryInfo currentDirectory = new DirectoryInfo(Directory.GetCurrentDirectory());

        private Task preloadTask;

		public string LogText { 
            get => logText;
            set
            {
                logText = value;
                RaisePropertyChanged(nameof(LogText));
            }
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
			activeModConfig = new FileInfo($"{currentDirectory}\\Pandora_Engine\\ActiveMods.txt");
			CultureInfo culture;

			culture = CultureInfo.CreateSpecificCulture("en-US");

			CultureInfo.DefaultThreadCurrentCulture = culture;
			CultureInfo.DefaultThreadCurrentUICulture = culture;
			CultureInfo.CurrentCulture = culture;
			ReadStartupArguments();
			preloadTask = Task.Run(Engine.PreloadAsync);
            

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
				LaunchCommand.Execute(null);
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

		private async void LaunchEngine(object? parameter)
        {
			lock (LaunchCommand)
			{
				engineRunning = true;
				LaunchEnabled = !engineRunning;
			}
			
            logText= string.Empty;

            await WriteLogBoxLine("Engine launched.");
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

            await ClearLogBox();
           

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

            
			

			Engine = new BehaviourEngine();
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
