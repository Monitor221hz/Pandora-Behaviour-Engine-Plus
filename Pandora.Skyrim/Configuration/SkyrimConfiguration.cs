// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using System;
using Pandora.API.Patch.Engine.Config;
using Pandora.Skyrim;

namespace Pandora.Skyrim.Configuration;

public class PandoraTemplateConfiguration : IEngineConfiguration
{
	public string Name { get; } = "Pandora Template";

	public string Description { get; } =
		@"Engine configuration for packing/unpacking Pandora's template behavior files, which used a customized havok binary format.";

	public Type PatcherType { get; } = typeof(PandoraTemplatePacker);
}

public class SkyrimConfiguration : IEngineConfiguration
{
	public string Name { get; } = "Skyrim SE/AE";

	public string Description { get; } = @"Engine configuration for Skyrim SE/AE behavior files";

	public Type PatcherType { get; } = typeof(SkyrimPatcher);
}
