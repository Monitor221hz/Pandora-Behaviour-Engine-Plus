// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json.Serialization;

namespace Pandora.Utils.Skyrim;

[JsonSerializable(typeof(SkyrimPathsConfiguration))]
public class SkyrimPathsConfiguration
{
	public DirectoryInfo? GameDataDirectory { get; set; }

	public DirectoryInfo? OutputDirectory { get; set; }
}
