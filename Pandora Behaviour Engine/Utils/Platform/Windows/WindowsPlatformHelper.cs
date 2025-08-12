using Avalonia.Media;
using FluentAvalonia.UI.Windowing;
using System;

namespace Pandora.Utils.Platform.Windows;

public static class WindowsPlatformHelper
{
	private static readonly Color DefaultBorderColor = Color.Parse("#FF9370DB");
	private static readonly Color RunningBorderColor = Color.Parse("#FFFF614D");

	public static void SetVisualWindowState(AppWindow window, WindowVisualState state)
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
				window.PlatformFeatures.SetTaskBarProgressBarState(TaskBarProgressBarState.Indeterminate);
				break;

			case WindowVisualState.Indeterminate:
				window.PlatformFeatures.SetTaskBarProgressBarState(TaskBarProgressBarState.Indeterminate);
				break;

			case WindowVisualState.Error:
				window.PlatformFeatures.SetTaskBarProgressBarState(TaskBarProgressBarState.Error);
				break;
		}
	}
}