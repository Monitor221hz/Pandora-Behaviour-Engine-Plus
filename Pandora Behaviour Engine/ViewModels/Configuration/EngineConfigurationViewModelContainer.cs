// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using System.Collections.ObjectModel;
using System.Reactive;
using Pandora.API.Patch.Config;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace Pandora.ViewModels;

public partial class EngineConfigurationViewModelContainer
	: ViewModelBase,
		IEngineConfigurationViewModel
{
	[Reactive]
	private string _name;

	public ReactiveCommand<IEngineConfigurationFactory, Unit>? SetCommand { get; }

	public ObservableCollection<IEngineConfigurationViewModel> NestedViewModels { get; }

	public EngineConfigurationViewModelContainer(string name)
	{
		Name = name;
		NestedViewModels = [];
	}

	public EngineConfigurationViewModelContainer(
		string name,
		params IEngineConfigurationViewModel[] viewModels
	)
	{
		Name = name;
		NestedViewModels = new ObservableCollection<IEngineConfigurationViewModel>(viewModels);
	}
}
