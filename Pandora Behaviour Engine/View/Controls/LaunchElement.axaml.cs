// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors
using Pandora.ViewModels;
using ReactiveUI.Avalonia;

namespace Pandora.Views;

public partial class LaunchElement : ReactiveUserControl<EngineViewModel>
{
	public LaunchElement()
	{
		InitializeComponent();
	}
}
