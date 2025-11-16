// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using Pandora.API.Patch.Engine.Config;

namespace Pandora.ViewModels;

public class ConstEngineConfigurationFactory<T> : IEngineConfigurationFactory
	where T : class, IEngineConfiguration, new()
{
	public ConstEngineConfigurationFactory(string name)
	{
		Name = name;
	}

	public string Name { get; set; }

	public IEngineConfiguration? Config => new T();
}
