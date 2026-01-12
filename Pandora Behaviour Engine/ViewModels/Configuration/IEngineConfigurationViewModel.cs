// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reactive;
using Pandora.API.Patch.Config;
using ReactiveUI;

namespace Pandora.ViewModels;

public interface IEngineConfigurationViewModel : INotifyPropertyChanged
{
	public string Name { get; }
	public ReactiveCommand<IEngineConfigurationFactory, Unit>? SetCommand { get; }
	public ObservableCollection<IEngineConfigurationViewModel> NestedViewModels { get; }
}
