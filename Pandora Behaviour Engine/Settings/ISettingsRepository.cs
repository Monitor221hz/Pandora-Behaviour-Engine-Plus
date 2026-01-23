// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

﻿using Pandora.Paths.Configuration.DTOs;
using System.IO;

namespace Pandora.Settings;

public interface ISettingsRepository
{
	PathsConfiguration Load();
	void Save(PathsConfiguration data);
}
