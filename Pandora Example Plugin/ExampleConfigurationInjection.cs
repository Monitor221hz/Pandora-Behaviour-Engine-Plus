// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using Pandora.API.Patch.Engine.Config;

namespace ExamplePlugin;

public class ExampleConfigurationInjection : IEngineConfigurationPlugin
{
	public string MenuPath { get; } = "Skyrim 64/Behavior/Patch";

	public IEngineConfigurationFactory Factory { get; } = new EngineConfigurationFactory();
}
