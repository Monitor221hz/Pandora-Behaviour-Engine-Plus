// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Pandora.API.Patch;
using Pandora.API.Patch.Engine.Config;

namespace Pandora.Models.Engine;

public interface IBehaviourEngine
{
	EngineState State { get; }
	IObservable<EngineState> StateChanged { get; }

	Task InitializeAsync();
	Task<EngineResult> RunAsync(IReadOnlyList<IModInfo> mods);
	Task SwitchConfigurationAsync(IEngineConfiguration config);
}
