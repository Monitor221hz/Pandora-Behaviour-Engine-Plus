// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using Pandora.Settings.SubSettings;

namespace Pandora.Settings;

public interface ISettingsService
{
	IThemeSettings Theme { get; }
	IPathSettings Paths { get; }

	void Initialize();
}