using Pandora.Command;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Pandora.ViewModels;

public interface IEngineConfigurationViewModel : INotifyPropertyChanged
{
	public string Name { get; }
	public RelayCommand? SetCommand { get; }
	public ObservableCollection<IEngineConfigurationViewModel> NestedViewModels { get; }
}
