// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Pandora.Services.Interfaces;
using Pandora.Views;

namespace Pandora.Platform.Windows;

public class WindowStateService : IWindowStateService
{
	private MainWindow? GetMainWindow()
	{
		if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
		{
			return desktop.MainWindow as MainWindow;
		}
		return null;
	}

	public void SetVisualState(WindowVisualState state)
	{
		GetMainWindow()?.SetVisualState(state);
	}

	public void FlashWindow()
	{
		GetMainWindow()?.FlashUntilFocused();
	}

	public void Shutdown()
	{
		if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
		{
			desktop.Shutdown();
		}
	}
}