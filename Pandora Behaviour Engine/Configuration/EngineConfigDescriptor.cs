// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using Pandora.API.Patch.Config;

namespace Pandora.Configuration;

public record EngineConfigDescriptor(
	IEngineConfigurationFactory Factory,
	string Name,
	string? MenuPath = null
);
