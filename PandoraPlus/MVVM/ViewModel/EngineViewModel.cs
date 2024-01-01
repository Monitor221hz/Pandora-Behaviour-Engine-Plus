using Pandora.MVVM.Data;
using Pandora.MVVM.Model;
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

namespace Pandora.MVVM.ViewModel
{
    public class EngineViewModel : INotifyPropertyChanged
    {
        private readonly IModInfoProvider modinfoProvider;
        private string logText = "";

        public event PropertyChangedEventHandler? PropertyChanged;

		private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

		public bool? DialogResult { get; set; } = true;

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
        public EngineViewModel(IModInfoProvider modinfoProvider)
        {
            this.modinfoProvider = modinfoProvider;
            LaunchCommand = new RelayCommand(LaunchEngine, CanLaunchEngine);
            ExitCommand = new RelayCommand(Exit);
			activeModConfig = new FileInfo($"{currentDirectory}\\Pandora_Engine\\ActiveMods.txt");
			CultureInfo culture;

			culture = CultureInfo.CreateSpecificCulture("en-US");

			CultureInfo.DefaultThreadCurrentCulture = culture;
			CultureInfo.DefaultThreadCurrentUICulture = culture;
			CultureInfo.CurrentCulture = culture;

			preloadTask = Task.Run(Engine.PreloadAsync);

		}
        public async Task LoadAsync()
        {
            List<IModInfo> modInfos = new List<IModInfo>();
#if DEBUG
            modInfos = await modinfoProvider?.GetInstalledMods("C:\\Games\\Skyrim Modding\\Creation Tools\\Skyrim.Behavior.Tool\\PandoraTEST\\Pandora_Engine\\mod")!;
#endif
			modInfos.AddRange(await modinfoProvider?.GetInstalledMods(currentDirectory + "\\Nemesis_Engine\\mod")!);
			modInfos.AddRange(await modinfoProvider?.GetInstalledMods(currentDirectory + "\\Pandora_Engine\\mod")!);


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

			Stopwatch timer = Stopwatch.StartNew();

			await Task.Run(async() => { await Engine.LaunchAsync(activeMods); }); 
            
            timer.Stop();

            await ClearLogBox();
           

            await WriteLogBoxLine(Engine.GetMessages());

			await WriteLogBoxLine($"Launch finished in {Math.Round(timer.ElapsedMilliseconds / 1000.0, 2)} seconds");

			await Task.Run(() => { SaveActiveMods(activeMods); });

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
