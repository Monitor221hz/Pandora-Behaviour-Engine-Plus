// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using Pandora.Platform.Avalonia;

namespace Pandora.Settings.DTOs;

public class AppSettings
{
	public AppTheme Theme { get; set; } = AppTheme.System;
}