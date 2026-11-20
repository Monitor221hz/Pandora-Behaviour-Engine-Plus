// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using Pandora.API.Patch.Engine.Config;

namespace Pandora.Skyrim.Configuration;

public class NemesisAnimDataConfiguration : IEngineConfiguration
{
	public string Name { get; set; } = "Reformat Nemesis Animdata";

	public string Description { get; } =
		"Realign Nemesis animation set condition edits so each edit covers the whole condition.";

	public Type PatcherType { get; } = typeof(NemesisAnimDataReformatter);
}
