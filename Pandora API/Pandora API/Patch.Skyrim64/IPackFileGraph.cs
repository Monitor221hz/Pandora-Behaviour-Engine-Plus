// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using HKX2E;

namespace Pandora.API.Patch.Skyrim64;

public interface IPackFileGraph : IPackFile, IEquatable<IPackFileGraph>
{
    hkbBehaviorGraphData Data { get; }
    uint InitialEventCount { get; }
    uint InitialVariableCount { get; }
    hkbBehaviorGraphStringData StringData { get; }
    hkbVariableValueSet VariableValueSet { get; }

    int AddDefaultEvent(string name);
    bool AddEventBuffer(string name);
    void ApplyPriorityChanges(IPackFileDispatcher dispatcher);
    bool Equals(IPackFileGraph? other);
    int FindEvent(string name);
    void FlushEventBuffer(string name);
    int GetHashCode();
    void Load();
    void PopPriorityXmlAsObjects();
    void PushPriorityObjects();
    void PushXmlAsObjects();
}
