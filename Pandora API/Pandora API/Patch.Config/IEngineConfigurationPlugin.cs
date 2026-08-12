// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

﻿namespace Pandora.API.Patch.Config;

public interface IEngineConfigurationPlugin
{
    public enum OptionFlags
    {
        None = 0,
        HidePatches = 1,
    }

    public string DisplayName { get; }
    public string MenuPath { get; }
    public IEngineConfigurationFactory Factory { get; }
}
