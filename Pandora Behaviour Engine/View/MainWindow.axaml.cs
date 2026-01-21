// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using System;
using FluentAvalonia.UI.Windowing;
using Pandora.Platform.Windows;
using Pandora.Enums;

namespace Pandora.Views;

public partial class MainWindow : AppWindow
{
	public MainWindow()
	{
		InitializeComponent();

		SetVisualState(WindowVisualState.Idle);

		ConfigureTitleBar();
		RestoreWindowSize();

		Closed += MainWindow_Closed;
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
		TitleBar.TitleBarHitTestType = TitleBarHitTestType.Complex;
		TitleBar.Height = 42;
	}

	private void RestoreWindowSize()
	{
		var savedHeight = Properties.GUISettings.Default.WindowHeight;
		var savedWidth = Properties.GUISettings.Default.WindowWidth;

		if (savedHeight > 100)
			Height = savedHeight;

		if (savedWidth > 100)
			Width = savedWidth;
	}

	public void SetVisualState(WindowVisualState state) => this.SetVisualWindowState(state);
}
