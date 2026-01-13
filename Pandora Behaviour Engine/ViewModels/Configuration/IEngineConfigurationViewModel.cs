using ReactiveUI;
using System.Collections.ObjectModel;

namespace Pandora.ViewModels.Configuration;

public interface IEngineConfigurationViewModel
{
    string Name { get; }
	bool IsChecked { get; }
	IReactiveCommand? SelectCommand { get; }
	ObservableCollection<IEngineConfigurationViewModel> Children { get; }
}