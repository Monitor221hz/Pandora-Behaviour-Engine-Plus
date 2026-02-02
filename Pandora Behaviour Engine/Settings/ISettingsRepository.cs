// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using Pandora.Settings.DTOs;

namespace Pandora.Settings;

public interface ISettingsRepository
{
	RootConfiguration Load();
	void Save(RootConfiguration data);
}
