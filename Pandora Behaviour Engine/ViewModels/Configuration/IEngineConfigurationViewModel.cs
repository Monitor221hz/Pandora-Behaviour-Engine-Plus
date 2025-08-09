using Pandora.API.Patch.Engine.Config;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reactive;

namespace Pandora.ViewModels;

public interface IEngineConfigurationViewModel : INotifyPropertyChanged
{
	public string Name { get; }
	public ReactiveCommand<IEngineConfigurationFactory, Unit>? SetCommand { get; }
	public ObservableCollection<IEngineConfigurationViewModel> NestedViewModels { get; }
}
