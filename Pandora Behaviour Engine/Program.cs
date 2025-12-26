// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using System;
using Avalonia;
using ReactiveUI.Avalonia;

namespace Pandora;

internal sealed class Program
{
	// Initialization code. Don't use any Avalonia, third-party APIs or any
	// SynchronizationContext-reliant code before AppMain is called: things aren't initialized
	// yet and stuff might break.
	[STAThread]
	public static void Main(string[] args) =>
		BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);

	// Avalonia configuration, don't remove; also used by visual designer.
	public static AppBuilder BuildAvaloniaApp() =>
		AppBuilder
			.Configure<App>()
			.UsePlatformDetect()
			.WithInterFont()
			.UseReactiveUI()
			.LogToTrace();
}
