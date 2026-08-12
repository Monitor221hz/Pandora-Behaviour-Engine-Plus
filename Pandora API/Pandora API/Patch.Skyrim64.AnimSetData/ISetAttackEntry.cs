// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

namespace Pandora.API.Patch.Skyrim64.AnimSetData;

public interface ISetAttackEntry
{
    string AttackTrigger { get; }
    IList<string> ClipNames { get; }
    int NumClips { get; }
    int Unk { get; }
    string ToString();
}
