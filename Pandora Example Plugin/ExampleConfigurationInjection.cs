// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using Pandora.API.Patch.Config;
using Pandora.API.Patch.Engine.Config;

namespace ExamplePlugin;

public class ExampleConfigurationInjection : IEngineConfigurationPlugin
{
    public string MenuPath { get; } = "Skyrim 64/Behavior/Patch";

    public string DisplayName { get; } = "Example";

    public IEngineConfigurationFactory Factory { get; } = new EngineConfigurationFactory();
}
