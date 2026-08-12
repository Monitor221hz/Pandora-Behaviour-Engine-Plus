// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using HKX2E;
using Pandora.API.Patch;

namespace Pandora.API.Patch.Skyrim64;

public interface IFNISAnimationListBuildContext
{
    IModInfo ModInfo { get; }
    IProjectManager ProjectManager { get; }
    IProject TargetProject { get; }

    hkbStringEventPayload BuildCommonStringEventPayload(string name);
}
