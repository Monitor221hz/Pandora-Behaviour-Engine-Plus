// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using Avalonia.Media;
using FluentAvalonia.UI.Windowing;
using Pandora.Services.Interfaces;
using System;

namespace Pandora.Platform.Windows;

public static class WindowsPlatformHelper
{
	private static readonly Color DefaultBorderColor = Color.Parse("#FF9370DB");
	private static readonly Color RunningBorderColor = Color.Parse("#FFFF614D");

	extension (AppWindow window)
	{
		public void SetVisualWindowState(WindowVisualState state)
		{
			if (!OperatingSystem.IsWindows())
				return;

			switch (state)
			{
				case WindowVisualState.Idle:
					window.PlatformFeatures.SetWindowBorderColor(DefaultBorderColor);
					window.PlatformFeatures.SetTaskBarProgressBarState(TaskBarProgressBarState.None);
					break;

				case WindowVisualState.Running:
					window.PlatformFeatures.SetWindowBorderColor(RunningBorderColor);
					window.PlatformFeatures.SetTaskBarProgressBarState(
						TaskBarProgressBarState.Indeterminate
					);
					break;

				case WindowVisualState.Indeterminate:
					window.PlatformFeatures.SetTaskBarProgressBarState(
						TaskBarProgressBarState.Indeterminate
					);
					break;

				case WindowVisualState.Error:
					window.PlatformFeatures.SetTaskBarProgressBarState(TaskBarProgressBarState.Error);
					break;
			}
		}
	}

}
