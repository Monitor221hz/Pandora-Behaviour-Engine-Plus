// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using Pandora.Core.Settings.DTOs;
using System;

namespace Pandora.Core.Settings.SubSettings;

public interface IThemeSettings : INotifySettingsChanged
{
	AppTheme Theme { get; set; }

	IObservable<AppTheme> ThemeChanged { get; }

	void Initialize(AppSettings appSettings);
}