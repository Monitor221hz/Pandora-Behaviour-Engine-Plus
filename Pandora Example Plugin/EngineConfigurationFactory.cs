// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using Pandora.API.Patch.Engine.Config;

namespace ExamplePlugin;

public class EngineConfigurationFactory : IEngineConfigurationFactory
{
    public string Name { get; } = "Example";

    public IEngineConfiguration? Config => new ExampleEngineConfiguration();
}
