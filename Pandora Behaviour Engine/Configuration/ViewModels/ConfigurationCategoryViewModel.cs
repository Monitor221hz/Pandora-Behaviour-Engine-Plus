// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using Pandora.ViewModels;
using ReactiveUI;
using System.Collections.ObjectModel;

namespace Pandora.Configuration.ViewModels;

public class ConfigurationCategoryViewModel(string name) : ViewModelBase, IEngineConfigurationViewModel
{
	public string Name { get; } = name;
	public bool IsChecked => false;
	public IReactiveCommand? SelectCommand => null;
	public ObservableCollection<IEngineConfigurationViewModel> Children { get; } = [];
}
