// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using Pandora.API.Patch.Config;
using Pandora.API.Services;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using System.Collections.ObjectModel;

namespace Pandora.ViewModels.Configuration;

public partial class ConfigurationOptionViewModel : ViewModelBase, IEngineConfigurationViewModel
{
	private readonly IEngineConfigurationService _configService;

	public IEngineConfigurationFactory Factory { get; }
	public string Name { get; }

	public ObservableCollection<IEngineConfigurationViewModel> Children { get; } = [];

	public ConfigurationOptionViewModel(
		string name,
		IEngineConfigurationFactory factory,
		IEngineConfigurationService configService)
	{
		Name = name;
		Factory = factory;
		_configService = configService;
	}

	[ReactiveCommand]
	public void Select() => _configService.SetCurrentFactory(Factory);
}