// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using Pandora.API.Patch.Config;
using System.Collections.ObjectModel;

namespace Pandora.ViewModels.Configuration;

public class ConfigurationCategoryViewModel : ViewModelBase, IEngineConfigurationViewModel
{
	public string Name { get; }
	public ObservableCollection<IEngineConfigurationViewModel> Children { get; } = [];

	public ConfigurationCategoryViewModel(string name)
	{
		Name = name;
	}
}
