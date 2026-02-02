// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Styling;
using Pandora.Platform.Avalonia;
using Pandora.Settings.SubSettings;
using System;
using System.Reactive.Linq;

namespace Pandora.Themes;

public sealed class Themer(IThemeSettings themeSettings) : IDisposable
{
	private IDisposable? _subscription;

	public void Initialize()
	{
		_subscription = themeSettings.ThemeChanged
			.StartWith(themeSettings.Theme)
			.Subscribe(ApplyTheme);
	}

	private static void ApplyTheme(AppTheme theme)
	{
		if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime)
		{
			Application.Current.RequestedThemeVariant = theme switch
			{
				AppTheme.Light => ThemeVariant.Light,
				AppTheme.Dark => ThemeVariant.Dark,
				_ => ThemeVariant.Default
			};
		}
	}

	public void Dispose()
	{
		_subscription?.Dispose();
	}
}
