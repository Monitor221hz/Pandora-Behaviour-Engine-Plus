using Pandora.API.Patch.Engine.Config;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using System.Collections.ObjectModel;
using System.Reactive;

namespace Pandora.ViewModels;

public partial class EngineConfigurationViewModelContainer : ViewModelBase, IEngineConfigurationViewModel
{
    [Reactive] private string _name;

    public ReactiveCommand<IEngineConfigurationFactory, Unit>? SetCommand { get; }

    public ObservableCollection<IEngineConfigurationViewModel> NestedViewModels { get; }

    public EngineConfigurationViewModelContainer(string name)
    {
        Name = name;
        NestedViewModels = [];
    }
    public EngineConfigurationViewModelContainer(string name, params IEngineConfigurationViewModel[] viewModels)
    {
        Name = name;
        NestedViewModels = new ObservableCollection<IEngineConfigurationViewModel>(viewModels);
    }

}