// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using Pandora.API.Patch.Engine.Config;
using Pandora.Models.Patch.Skyrim64;
using System;

namespace Pandora.Models.Patch.Configs;

public class SkyrimConfiguration : IEngineConfiguration
{
	public string Name { get; } = "Skyrim SE/AE";

	public string Description { get; } = @"Engine configuration for Skyrim SE/AE behavior files";

	public Type PatcherType { get; } = typeof(SkyrimPatcher);
}
