// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

﻿namespace Pandora.API.Patch.Engine.Config;

public interface IEngineConfiguration
{
    string Name { get; }

    string Description { get; }

    public Type PatcherType { get; }
}
