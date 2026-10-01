// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using Avalonia.Controls;
using Pandora.ViewModels;
using ReactiveUI.Avalonia;
using System;

namespace Pandora.Views;

public partial class EngineMenu : ReactiveUserControl<EngineMenuViewModel>
{
	public EngineMenu()
	{
		InitializeComponent();
	}

	protected override void OnLoaded(Avalonia.Interactivity.RoutedEventArgs e)
	{
		base.OnLoaded(e);

		if (OperatingSystem.IsWindows())
		{
			Avalonia.Controls.Win32Properties.SetNonClientHitTestResult(
				TitleBarHost,
				Avalonia.Controls.Win32Properties.Win32HitTestValue.Caption
			);
		}
	}
}
