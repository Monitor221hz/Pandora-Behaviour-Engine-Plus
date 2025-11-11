// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using Avalonia.ReactiveUI;
using Pandora.ViewModels;

namespace Pandora.Views;

public partial class LaunchElement : ReactiveUserControl<EngineViewModel>
{
	public LaunchElement()
	{
		InitializeComponent();
	}
}
