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

public class EngineConfigurationViewModel<T> : IEngineConfigurationFactory,IEngineConfigurationViewModel where T : class, IEngineConfiguration, new()
{
    public event PropertyChangedEventHandler? PropertyChanged;
	private void RaisePropertyChanged([CallerMemberName] string? propertyName = null)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
	public string Name { get; private set; }
	public IEngineConfiguration? Config => new T();

	public ObservableCollection<IEngineConfigurationViewModel> NestedViewModels { get; private set; } = new ObservableCollection<IEngineConfigurationViewModel>();

	public RelayCommand? SetCommand { get; } = null;

	public EngineConfigurationViewModel(string name, RelayCommand setCommand) 
	{
		Name = name;
		SetCommand = setCommand;
	}
}

