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
			activeModConfig = new FileInfo($"{Directory.GetCurrentDirectory()}\\Pandora_Engine\\ActiveMods.txt");

		}
        public async Task LoadAsync()
        {

            List<IModInfo> modInfos = new List<IModInfo>();
#if DEBUG
            modInfos = await modinfoProvider?.GetInstalledMods("C:\\Games\\Skyrim Modding\\Creation Tools\\Skyrim.Behavior.Tool\\PandoraTEST\\Pandora_Engine\\mod")!;
#endif
			modInfos.AddRange(await modinfoProvider?.GetInstalledMods(Directory.GetCurrentDirectory() + "\\Nemesis_Engine\\mod")!);
			modInfos.AddRange(await modinfoProvider?.GetInstalledMods(Directory.GetCurrentDirectory() + "\\Pandora_Engine\\mod")!);


			foreach (var modInfo in modInfos)
            {
                Mods.Add(modInfo);
                modsByCode.Add(modInfo.Code, modInfo);
            }

            LoadActiveMods();
        }

        public void Exit(object? p)
        {
            App.Current.MainWindow.Close();
        }

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

        private bool LoadActiveMods()
        {
            if (!activeModConfig.Exists) return false;
            foreach(var mod in Mods) 
            { 
                if (mod == null) continue;
                mod.Active = false;  
            }
            using (var readStream = activeModConfig.OpenRead())
            {
                using (var streamReader  = new StreamReader(readStream))
                {
					string? expectedLine;

                    while ((expectedLine = streamReader.ReadLine()) != null)
                    {
                        IModInfo? modInfo;
                        if (!modsByCode.TryGetValue(expectedLine, out modInfo)) continue;
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
        private async void LaunchEngine(object? parameter)
        {
			lock (LaunchCommand)
			{
				engineRunning = true;
				LaunchEnabled = !engineRunning;
			}
			CultureInfo culture;
            
			culture = CultureInfo.CreateSpecificCulture("en-US");

			CultureInfo.DefaultThreadCurrentCulture = culture;
			CultureInfo.DefaultThreadCurrentUICulture = culture;
            CultureInfo.CurrentCulture = culture;

			Engine = new BehaviourEngine(); 
            logText= string.Empty;

            await WriteLogBoxLine("Engine launched.");

            
            Stopwatch timer = Stopwatch.StartNew();
            var activeMods = Mods.Where(x => x.Active).ToList<IModInfo>();

			await Task.Run(async () => { await Engine.LaunchAsync(activeMods); }); 
            
            timer.Stop();
            await WriteLogBoxLine($"Launch finished in {Math.Round(timer.ElapsedMilliseconds / 1000.0, 2)} seconds");

            await WriteLogBoxLine(Engine.GetMessages());

            await Task.Run(() => { SaveActiveMods(activeMods); });
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
