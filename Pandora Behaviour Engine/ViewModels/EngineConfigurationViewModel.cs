using Pandora.API.Patch.Engine.Config;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Reactive;

namespace Pandora.ViewModels;

public partial class EngineConfigurationViewModel : ViewModelBase, IEngineConfigurationFactory, IEngineConfigurationViewModel
{
    private readonly IEngineConfigurationFactory engineConfigurationFactory;
	public IEngineConfigurationFactory Factory => engineConfigurationFactory;

	public string Name => engineConfigurationFactory.Name;

    public IEngineConfiguration? Config => engineConfigurationFactory.Config;

    public ObservableCollection<IEngineConfigurationViewModel> NestedViewModels { get; } = [];

    public ReactiveCommand<IEngineConfigurationFactory, Unit>? SetCommand { get; }

    public EngineConfigurationViewModel(IEngineConfigurationFactory factory, ReactiveCommand<IEngineConfigurationFactory, Unit> setCommand)
    {
        engineConfigurationFactory = factory;
        SetCommand = setCommand;
    }
}

