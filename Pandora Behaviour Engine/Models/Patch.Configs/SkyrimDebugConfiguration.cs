// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using Pandora.API.Patch.Engine.Config;
using Pandora.Models.Patch.Skyrim64;
using System;

namespace Pandora.Models.Patch.Configs;

public class SkyrimDebugConfiguration : IEngineConfiguration
{
	public string Name { get; } = "Skyrim SE/AE Debug";

	public string Description { get; } =
		@"Engine configuration for Skyrim SE/AE behavior files (with debug info)";

	public Type PatcherType { get; } = typeof(SkyrimPatcher);
}
