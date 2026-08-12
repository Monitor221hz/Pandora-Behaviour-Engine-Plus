// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

namespace Pandora.API.Patch.Skyrim64.AnimSetData;

public interface ISetCachedAnimInfo
{
    uint EncodedExtension { get; }
    uint EncodedFileName { get; }
    uint EncodedPath { get; }
    ISetCachedAnimInfo Encode(string folderPath, string fileName);
    string ToString();
}
