// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using System.Collections.Generic;
using System.Threading.Tasks;
using Pandora.API.Patch;
using Pandora.API.Patch.Engine.Config;

namespace Pandora.Models.Engine;

public interface IEngineRunner
{
	Task PreloadAsync();
	Task<EngineResult> RunAsync(IReadOnlyList<IModInfo> mods);
	Task SwitchConfigurationAsync(IEngineConfiguration config);
}
