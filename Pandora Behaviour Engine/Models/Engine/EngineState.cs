// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

﻿namespace Pandora.Models.Engine;

public enum EngineState
{
    Uninitialized,
    Preloading,
    Ready,
    Running,
    Success,
    Error
}