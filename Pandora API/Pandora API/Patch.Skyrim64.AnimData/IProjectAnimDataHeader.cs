// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

﻿namespace Pandora.API.Patch.Skyrim64.AnimData;

public interface IProjectAnimDataHeader
{
    int AssetCount { get; set; }
    int HasMotionData { get; set; }
    int LeadInt { get; set; }
    IList<string> ProjectAssets { get; set; }

    int GetLineCount();
    string ToString();
}
