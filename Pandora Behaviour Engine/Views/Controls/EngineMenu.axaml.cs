// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using Avalonia.Controls;
using Avalonia.Interactivity;
using FluentAvalonia.UI.Windowing;
using Pandora.ViewModels;
using ReactiveUI.Avalonia;

namespace Pandora.Views;

public partial class EngineMenu : ReactiveUserControl<EngineMenuViewModel>
{
	public EngineMenu()
	{
		InitializeComponent();
	}

	protected override void OnLoaded(RoutedEventArgs e)
	{
		base.OnLoaded(e);

		if (VisualRoot is AppWindow aw)
		{
			TitleBarHost.ColumnDefinitions[2].Width = new GridLength(
				aw.TitleBar.RightInset,
				GridUnitType.Pixel
			);
		}
	}
}
