using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform;
using Avalonia.Styling;
using System;
using System.Threading.Tasks;

namespace Pandora.Utils;

public static class AvaloniaServices
{
	public static async Task DoSetClipboardTextAsync(string? text)
	{
		if (Application.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop ||
			desktop.MainWindow?.Clipboard is not { } provider)
			throw new NullReferenceException("Missing Clipboard instance.");

		await provider.SetTextAsync(text);
	}

	public static async Task DoLaunchUriAsync(string uri)
	{
		if (Application.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop ||
			desktop.MainWindow?.Launcher is not { } provider)
			throw new NullReferenceException("Missing Launcher instance.");

		await provider.LaunchUriAsync(new Uri(uri));
	}

	public static void ApplyTheme(PlatformThemeVariant variant)
	{
		Application.Current!.RequestedThemeVariant = variant == PlatformThemeVariant.Light
			? ThemeVariant.Light
			: ThemeVariant.Dark;

		Properties.GUISettings.Default.AppTheme = (int)variant;
		Properties.GUISettings.Default.Save();
	}
}
