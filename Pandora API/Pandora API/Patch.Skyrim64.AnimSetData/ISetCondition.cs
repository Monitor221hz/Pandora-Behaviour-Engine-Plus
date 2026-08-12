// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

namespace Pandora.API.Patch.Skyrim64.AnimSetData;

public interface ISetCondition
{
    int Value1 { get; }
    int Value2 { get; }
    string VariableName { get; }
    string ToString();
}
