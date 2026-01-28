// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using Pandora.API.Patch.Config;
using Pandora.API.Patch.Engine.Config;
using System;

namespace Pandora.Models.Patch.Configs;

public class ConstEngineConfigurationFactory<T>(Func<T> producer) : IEngineConfigurationFactory<T>
	where T : class, IEngineConfiguration
{
	private readonly Func<T> _producer = producer;
	public string Name { get; } = $"Const Configuration Factory ({nameof(T)})";

	public IEngineConfiguration? Create() => _producer();
}
