// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using System.Collections.ObjectModel;
using Pandora.ViewModels;
using ReactiveUI;

namespace Pandora.Configuration.ViewModels;

public sealed class ConfigurationActionViewModel : ViewModelBase, IEngineConfigurationViewModel
{
	public ConfigurationActionViewModel(string name, IReactiveCommand command)
	{
		Name = name;
		SelectCommand = command;
	}

	public string Name { get; }
	public bool IsChecked => false;
	public bool IsCheckable => false;
	public IReactiveCommand? SelectCommand { get; }
	public ObservableCollection<IEngineConfigurationViewModel> Children { get; } = [];
}
