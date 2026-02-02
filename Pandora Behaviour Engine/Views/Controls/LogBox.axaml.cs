// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using Avalonia;
using Pandora.ViewModels;
using ReactiveUI.Avalonia;
using System;

namespace Pandora.Views;

public partial class LogBox : ReactiveUserControl<LogBoxViewModel>
{
	public event Action<bool>? ExpanderToggled;

	public LogBox()
	{
		InitializeComponent();

		LogExpander.Expanded += (_, _) => ExpanderToggled?.Invoke(true);
		LogExpander.Collapsed += (_, _) => ExpanderToggled?.Invoke(false);
	}
}
