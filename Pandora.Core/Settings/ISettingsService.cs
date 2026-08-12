// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using Pandora.Core.Settings.SubSettings;

namespace Pandora.Core.Settings;

public interface ISettingsService
{
	IThemeSettings Theme { get; }
	IPathSettings Paths { get; }

	void Initialize();
}