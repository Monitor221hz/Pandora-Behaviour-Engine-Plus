using NLog.Filters;
using Pandora.Command;
using Pandora.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Pandora.MVVM.ViewModel;

public interface IEngineConfigurationFactory : INotifyPropertyChanged
{
	public string Name { get; }
	public IEngineConfiguration Config { get; }
}

public class EngineConfigurationViewModel<T> : IEngineConfigurationFactory where T : class, IEngineConfiguration, new()
{
	public event PropertyChangedEventHandler? PropertyChanged;
	private void RaisePropertyChanged([CallerMemberName] string? propertyName = null)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
	public string Name { get; private set; }
	public IEngineConfiguration Config => new T();

	public RelayCommand SetCommand { get; }
	public EngineConfigurationViewModel(string name, RelayCommand setCommand) 
	{
		Name = name;
		SetCommand = setCommand;
	}

}
