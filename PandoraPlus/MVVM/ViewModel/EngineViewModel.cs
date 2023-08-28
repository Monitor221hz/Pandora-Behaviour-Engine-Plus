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

namespace Pandora.MVVM.ViewModel
{
    public class EngineViewModel : INotifyPropertyChanged
    {
        private readonly IModInfoProvider modinfoProvider;
        private string logText = "";

        public event PropertyChangedEventHandler? PropertyChanged;

        public bool? DialogResult { get; set; } = true;

        public BehaviourEngine Engine { get; private set; } = new BehaviourEngine();

        public RelayCommand LaunchCommand { get; }
        public RelayCommand ExitCommand { get; }

        public ObservableCollection<NemesisModInfo> Mods { get; set; } = new ObservableCollection<NemesisModInfo>();

        private bool engineRunning = false;

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

            
        }
        public async Task LoadAsync()
        {
            var modInfos = await modinfoProvider?.GetInstalledMods("C:\\Games\\Skyrim Modding\\Creation Tools\\Skyrim.Behavior.Tool\\PandoraTEST\\Nemesis_Engine\\mod")!;
            foreach (var modInfo in modInfos)
            {
                Mods.Add(modInfo);
            }
        }

        public void Exit(object? p)
        {
            App.Current.MainWindow.Close();
        }

        internal async Task WriteLogBoxLine(string text)
        {
            StringBuilder sb = new StringBuilder(LogText);
            if (LogText.Length > 0) sb.Append('\n');
            sb.Append(text);
            LogText = sb.ToString();
        }
        internal async Task WriteLogBox(string text)
        {
            StringBuilder sb = new StringBuilder(LogText);
            sb.Append(text);
            LogText = sb.ToString();
        }

        private async void LaunchEngine(object? parameter)
        {
            engineRunning = true;
            Engine = new BehaviourEngine(); 
            logText= string.Empty;

            await WriteLogBoxLine("Engine launched.");
            Stopwatch timer = Stopwatch.StartNew();
            await Task.Run(async () => { await Engine.LaunchAsync(Mods.Where(x => x.Active).ToList<IModInfo>()); }); 
            
            timer.Stop();
            await WriteLogBoxLine($"Launch finished in {Math.Round(timer.ElapsedMilliseconds / 1000.0, 2)} seconds");

            engineRunning = false; 
        }

        private bool CanLaunchEngine(object? parameter)
        {

            return !engineRunning;
            
        }
    }
}
