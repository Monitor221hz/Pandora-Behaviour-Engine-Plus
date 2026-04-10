// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using System.Collections.ObjectModel;
using ReactiveUI;

namespace Pandora.Configuration.ViewModels;

public interface IEngineConfigurationViewModel
{
	string Name { get; }
	bool IsChecked { get; }
	IReactiveCommand? SelectCommand { get; }
	ObservableCollection<IEngineConfigurationViewModel> Children { get; }
}
