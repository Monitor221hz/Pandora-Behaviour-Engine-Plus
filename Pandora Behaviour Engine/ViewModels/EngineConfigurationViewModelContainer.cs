using Pandora.Command;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Pandora.ViewModels;

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
	public EngineConfigurationViewModelContainer(string name)
	{
		Name = name;
		NestedViewModels = new(); 
	}
}

