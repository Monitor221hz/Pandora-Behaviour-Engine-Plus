using ReactiveUI;
using System.Collections.ObjectModel;

namespace Pandora.Configuration.ViewModels;

public interface IEngineConfigurationViewModel
{
    string Name { get; }
	bool IsChecked { get; }
	IReactiveCommand? SelectCommand { get; }
	ObservableCollection<IEngineConfigurationViewModel> Children { get; }
}