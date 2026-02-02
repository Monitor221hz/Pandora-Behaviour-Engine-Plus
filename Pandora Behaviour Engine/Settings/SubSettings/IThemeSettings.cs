using Pandora.Platform.Avalonia;
// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using Pandora.Settings.DTOs;
using System;

namespace Pandora.Settings.SubSettings;

public interface IThemeSettings : INotifySettingsChanged
{
	AppTheme Theme { get; set; }

	IObservable<AppTheme> ThemeChanged { get; }

	void Initialize(AppSettings appSettings);
}
