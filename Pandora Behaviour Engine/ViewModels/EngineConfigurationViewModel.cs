using NLog.Filters;
using Pandora.API.Patch.Engine.Config;
using Pandora.Command;
using Pandora.Core;
using Pandora.Core.Engine.Configs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Pandora.ViewModels;
public class EngineConfigurationViewModel : IEngineConfigurationFactory, IEngineConfigurationViewModel
{
	private IEngineConfigurationFactory engineConfigurationFactory; 

    public event PropertyChangedEventHandler? PropertyChanged;
	private void RaisePropertyChanged([CallerMemberName] string? propertyName = null)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
	public string Name => engineConfigurationFactory.Name;
	public IEngineConfiguration? Config => engineConfigurationFactory.Config;

	public ObservableCollection<IEngineConfigurationViewModel> NestedViewModels { get; private set; } = new ObservableCollection<IEngineConfigurationViewModel>();

	public RelayCommand? SetCommand { get; } = null;

	public EngineConfigurationViewModel(IEngineConfigurationFactory factory,RelayCommand setCommand) 
	{
		engineConfigurationFactory = factory;
		SetCommand = setCommand;
	}
}

