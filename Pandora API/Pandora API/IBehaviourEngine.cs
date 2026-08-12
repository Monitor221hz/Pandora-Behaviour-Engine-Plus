// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using Pandora.API.Patch;
using Pandora.API.Patch.Engine.Config;

namespace Pandora.API;

public interface IBehaviourEngine
{
    IEngineConfiguration Configuration { get; }
    string GetMessages(bool success);
    Task<bool> LaunchAsync(List<IModInfo> mods);
    Task PreloadAsync();
}
