// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using FluentAvalonia.UI.Controls;
using FluentAvalonia.UI.Windowing;
using Pandora.Platform.Windows;
using Pandora.ViewModels;
using Pandora.Views.Pages;
using Pandora.Views.Pages.DTOs;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using System;

namespace Pandora.Views;

[IViewFor<MainWindowViewModel>]
public partial class MainWindow : AppWindow
{
	public MainWindow(IServiceProvider serviceProvider)
	{
		InitializeComponent();

		RoutedViewHost.ViewLocator = new DIViewLocator(serviceProvider);
		NavPanel.SelectionChanged += NavPanelOnSelectionChanged;
		Closed += MainWindow_Closed;

		ConfigureTitleBar();
	}

	private void MainWindow_Closed(object? sender, EventArgs e)
	{
		Properties.GUISettings.Default.WindowHeight = Height;
		Properties.GUISettings.Default.WindowWidth = Width;
		Properties.GUISettings.Default.Save();
	}

	private void ConfigureTitleBar()
	{
		TitleBar.ExtendsContentIntoTitleBar = true;
		if (!OperatingSystem.IsWindows())
			TitleBar.ExtendsContentIntoTitleBar = false;

		TitleBar.TitleBarHitTestType = TitleBarHitTestType.Complex;
		TitleBar.Height = 32;
	}

	private void NavPanelOnSelectionChanged(object? sender, NavigationViewSelectionChangedEventArgs e)
	{
		if (ViewModel is null) return;

		if (e.SelectedItem is NavigationItem item)
		{
			ViewModel.NavigateToUriCommand.Execute(item.Route);
		}
	}

	public void SetVisualState(WindowVisualState state) => this.SetVisualWindowState(state);
}
