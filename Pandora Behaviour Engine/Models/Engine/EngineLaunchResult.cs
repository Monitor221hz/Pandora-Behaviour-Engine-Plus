// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

﻿using System;

namespace Pandora.Models.Engine;

public record EngineResult(bool IsSuccess, TimeSpan Duration, string? Message)
{
    public static EngineResult Fail(string? message, TimeSpan duration = default) =>
        new(false, duration, message);

    public static EngineResult Success(string? message, TimeSpan duration) =>
        new(true, duration, message);
}