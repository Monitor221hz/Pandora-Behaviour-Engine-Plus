// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

namespace Pandora.API.Patch.Skyrim64.AnimSetData;

public interface IAnimSet
{
    IList<ISetCachedAnimInfo> AnimInfos { get; }
    IList<ISetAttackEntry> AttackEntries { get; }
    IList<ISetCondition> Conditions { get; }
    int NumAnimationInfos { get; }
    int NumAttackEntries { get; }
    int NumConditions { get; }
    int NumTriggers { get; }
    IList<string> Triggers { get; }
    string VersionName { get; }
    void AddAnimInfo(ISetCachedAnimInfo animInfo);
    string ToString();
}
