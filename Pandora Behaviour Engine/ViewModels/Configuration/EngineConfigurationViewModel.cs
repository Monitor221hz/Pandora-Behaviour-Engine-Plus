// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using System.Collections.ObjectModel;
using System.Reactive;
using Pandora.API.Patch.Config;
using Pandora.API.Patch.Engine.Config;
using ReactiveUI;

namespace Pandora.ViewModels;

public partial class EngineConfigurationViewModel
	: ViewModelBase,
		IEngineConfigurationFactory,
		IEngineConfigurationViewModel
{
	private readonly IEngineConfigurationFactory engineConfigurationFactory;
	public IEngineConfigurationFactory Factory => engineConfigurationFactory;

	public string Name { get; init; }

	public ObservableCollection<IEngineConfigurationViewModel> NestedViewModels { get; } = [];

	public ReactiveCommand<IEngineConfigurationFactory, Unit>? SetCommand { get; }

	public EngineConfigurationViewModel(
		IEngineConfigurationFactory factory,
		string name,
		ReactiveCommand<IEngineConfigurationFactory, Unit> setCommand
	)
	{
		engineConfigurationFactory = factory;
		Name = name;
		SetCommand = setCommand;
	}

	public IEngineConfiguration? Create()
	{
		return engineConfigurationFactory.Create();
	}
}
