// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using Avalonia.Controls;
using Avalonia.Media;
using FluentAvalonia.UI.Windowing;
using System;

namespace Pandora.Platform.Windows;

public static class WindowsPlatformHelper
{
	private static readonly Color DefaultBorderColor = Color.Parse("#FF9370DB");
	private static readonly Color RunningBorderColor = Color.Parse("#FFFF614D");

	extension(Window window)
	{
		public void SetVisualWindowState(WindowVisualState state)
		{
			if (window is not FAAppWindow faAppWindow || !OperatingSystem.IsWindows() || Pandora.Core.Platform.WineHelper.IsWineRunning)
				return;

			switch (state)
			{
				case WindowVisualState.Idle:
					faAppWindow.PlatformFeatures.SetWindowBorderColor(DefaultBorderColor);
					faAppWindow.PlatformFeatures.SetTaskBarProgressBarState(
						FATaskBarProgressBarState.None
					);
					break;

				case WindowVisualState.Running:
					faAppWindow.PlatformFeatures.SetWindowBorderColor(RunningBorderColor);
					faAppWindow.PlatformFeatures.SetTaskBarProgressBarState(
						FATaskBarProgressBarState.Indeterminate
					);
					break;

				case WindowVisualState.Indeterminate:
					faAppWindow.PlatformFeatures.SetTaskBarProgressBarState(
						FATaskBarProgressBarState.Indeterminate
					);
					break;

				case WindowVisualState.Error:
					faAppWindow.PlatformFeatures.SetTaskBarProgressBarState(
						FATaskBarProgressBarState.Error
					);
					break;
			}
		}
	}
}