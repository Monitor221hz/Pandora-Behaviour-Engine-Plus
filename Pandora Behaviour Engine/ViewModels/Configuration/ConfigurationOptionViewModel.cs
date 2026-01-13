// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using Pandora.API.Patch.Config;
using Pandora.Services.Interfaces;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using System.Collections.ObjectModel;
using System.Reactive.Linq;

namespace Pandora.ViewModels.Configuration;

public partial class ConfigurationOptionViewModel : ViewModelBase, IEngineConfigurationViewModel
{
	private readonly IEngineConfigurationService _configService;

	public string Name { get; }
	public IEngineConfigurationFactory Factory { get; }
	public IReactiveCommand? SelectCommand => ExecuteSelectCommand;

	[ObservableAsProperty]
	private bool _isChecked;

	public ObservableCollection<IEngineConfigurationViewModel> Children { get; } = [];

	public ConfigurationOptionViewModel(
		string name,
		IEngineConfigurationFactory factory,
		IEngineConfigurationService configService)
	{
		Name = name;
		Factory = factory;
		_configService = configService;

		_configService.CurrentFactoryChanged
			.Select(current => current == Factory)
			.ObserveOn(RxApp.MainThreadScheduler)
			.ToProperty(this, x => x.IsChecked);
	}

	[ReactiveCommand]
	public void ExecuteSelect() => _configService.SetCurrentFactory(Factory);
}