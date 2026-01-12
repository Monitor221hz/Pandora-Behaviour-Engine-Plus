// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using System.Collections.Generic;
using System.IO;

namespace Pandora.Services.Skyrim;

public class GamePathSettings
{
	public DirectoryInfo? GameDataDirectory { get; set; }

	public DirectoryInfo? OutputDirectory { get; set; }
}

public class PathsConfiguration : Dictionary<string, GamePathSettings>
{
}