// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Pandora.Views;

namespace Pandora.Platform.Windows;

public sealed class WindowStateService(MainWindow window) : IWindowStateService
{
	public void Initialize()
	{
		var savedHeight = Properties.GUISettings.Default.WindowHeight;
		var savedWidth = Properties.GUISettings.Default.WindowWidth;

		if (savedHeight > 100) window.Height = savedHeight;
		if (savedWidth > 100) window.Width = savedWidth;
	}

	public void SetVisualState(WindowVisualState state) => window.SetVisualState(state);
	public void FlashWindow() => window.FlashUntilFocused();

	public void Shutdown() =>
		(Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.Shutdown();
}