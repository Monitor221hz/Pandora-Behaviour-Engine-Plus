// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using Pandora.API.Patch;
using Pandora.API.Patch.Engine.Config;
using Pandora.Models.Patch.IO.Skyrim64;
using Pandora.Models.Patch.Skyrim64;

namespace Pandora.Models.Patch.Configs;

public class SkyrimDebugConfiguration : IEngineConfiguration
{
	public string Name { get; } = "Skyrim SE/AE Debug";

	public string Description { get; } =
		@"Engine configuration for Skyrim SE/AE behavior files (with debug info)";

	public IPatcher Patcher { get; } = new SkyrimPatcher(new DebugPackFileExporter());
}
