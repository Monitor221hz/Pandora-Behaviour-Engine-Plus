// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

namespace Pandora.API.Patch.Skyrim64.AnimSetData;

public interface IProjectAnimSetData
{
    IList<string> AnimSetFileNames { get; }
    IList<IAnimSet> AnimSets { get; }
    Dictionary<string, IAnimSet> AnimSetsByName { get; }
    int NumSets { get; }
    string ToString();
}
