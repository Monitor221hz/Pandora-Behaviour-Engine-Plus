// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using Pandora.API.Patch.Engine.Config;

namespace Pandora.Skyrim.Configuration;

public class NemesisToPandoraConfiguration : IEngineConfiguration
{
	public string Name { get; set; } = "Nemesis -> Pandora";

	public string Description { get; } = "Convert Nemesis patches to Pandora patches";

	public Type PatcherType { get; } = typeof(NemesisPandoraConverter);
}
