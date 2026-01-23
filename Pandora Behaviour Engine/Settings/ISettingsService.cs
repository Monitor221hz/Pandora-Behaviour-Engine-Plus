// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

﻿using System.IO;

namespace Pandora.Settings;

public interface ISettingsService
{
	bool IsGameDataValid { get; }
	bool NeedsUserSelection { get; }

	void Initialize();
	void SetGameDataFolder(DirectoryInfo dir);
	void SetOutputFolder(DirectoryInfo dir);
}