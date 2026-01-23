// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Pandora.Enums;
using Pandora.Views;

namespace Pandora.Platform.Windows;

public sealed class WindowStateService(MainWindow window) : IWindowStateService
{
	private readonly MainWindow _mainWindow = window;

	public void SetVisualState(WindowVisualState state)
	{
		_mainWindow.SetVisualState(state);
	}

	public void FlashWindow()
	{
		_mainWindow.FlashUntilFocused();
	}

	public void Shutdown()
	{
		if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
		{
			desktop.Shutdown();
		}
	}
}