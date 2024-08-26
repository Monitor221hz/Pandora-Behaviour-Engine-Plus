using NLog.Filters;
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
public interface IEngineConfigurationViewModel : INotifyPropertyChanged
{
	public string Name { get; }
	public RelayCommand? SetCommand { get; }
	public ObservableCollection<IEngineConfigurationViewModel> NestedViewModels { get; }
}
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

public class EngineConfigurationViewModelContainer : IEngineConfigurationViewModel
{
	public event PropertyChangedEventHandler? PropertyChanged;
	private void RaisePropertyChanged([CallerMemberName] string? propertyName = null)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
	public string Name { get; private set; }

	public RelayCommand? SetCommand { get; } = null;

	public ObservableCollection<IEngineConfigurationViewModel> NestedViewModels { get; private set; } = new ObservableCollection<IEngineConfigurationViewModel>();
	public EngineConfigurationViewModelContainer(string name, params IEngineConfigurationViewModel[] viewModels)
	{
		Name = name;
		foreach (var viewModel in viewModels) { NestedViewModels.Add(viewModel); }
	}
}

