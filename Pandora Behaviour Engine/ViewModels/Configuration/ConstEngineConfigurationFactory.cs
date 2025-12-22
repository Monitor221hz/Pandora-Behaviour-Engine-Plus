// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using System;
using Pandora.API.Patch;
using Pandora.API.Patch.Config;
using Pandora.API.Patch.Engine.Config;
using Pandora.Models.Patch.Configs;

namespace Pandora.ViewModels.Configuration;

public class ConstEngineConfigurationFactory<T>(Func<T> producer) : IEngineConfigurationFactory<T>
	where T : class, IEngineConfiguration
{
	private readonly Func<T> _producer = producer;
	public string Name { get; } = $"Const Configuration Factory ({nameof(T)})";

	public IEngineConfiguration? Create() => _producer();
}
