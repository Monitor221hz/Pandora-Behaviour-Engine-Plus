using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
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
}
