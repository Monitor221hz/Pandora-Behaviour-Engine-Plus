// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

﻿using System.Diagnostics.CodeAnalysis;

namespace Pandora.API.Patch.Skyrim64.AnimData;

public interface IMotionData
{
    void AddDummyClipMotionData(string id);
    public IList<IClipMotionDataBlock> Blocks { get; }
    int GetLineCount();
    string ToString();
    bool TryGetBlock(int id, [NotNullWhen(true)] out IClipMotionDataBlock? block);
    void AddClipMotionData(IClipMotionDataBlock motionDataBlock);
}
