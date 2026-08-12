// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using System;

namespace Pandora.API.Patch.Skyrim64;

[Flags]
public enum FNISAnimFlags
{
    None,
    Acyclic = 1 << 0,
    AnimObjects = 1 << 1,
    AnimatedCamera = 1 << 2,
    AnimatedCameraSet = 1 << 3,
    AnimatedCameraReset = 1 << 4,
    BSA = 1 << 5,
    Headtracking = 1 << 6,
    Known = 1 << 7,
    MotionDriven = 1 << 8,
    Sticky = 1 << 9,
    TransitionNext = 1 << 10,

    // special runtime added flags, not parsable
    SequenceStart = 1 << 11,
    SequenceFinish = 1 << 12,
}
