// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

﻿namespace Pandora.API.Patch.Skyrim64.AnimData;

public interface IClipMotionDataBlock
{
    string ClipID { get; set; }
    float Duration { get; }
    int NumRotations { get; }
    int NumTranslations { get; }
    IList<string> Rotations { get; }
    IList<string> Translations { get; }

    int GetLineCount();
    string ToString();
}
