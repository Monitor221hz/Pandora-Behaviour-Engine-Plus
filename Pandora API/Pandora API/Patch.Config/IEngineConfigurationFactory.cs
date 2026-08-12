// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

﻿using Pandora.API.Patch.Engine.Config;

namespace Pandora.API.Patch.Config;

public interface IEngineConfigurationFactory
{
    public IEngineConfiguration? Create();
}

public interface IEngineConfigurationFactory<T> : IEngineConfigurationFactory
    where T : class, IEngineConfiguration
{
    public string Name { get; }
}
