// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

﻿// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using HKX2E;

namespace Pandora.API.Patch.Skyrim64;

public interface IFNISAnimation
{
    public string AnimationFilePath { get; }
    public FNISAnimFlags Flags { get; set; }
    public string GraphEvent { get; }
    public bool HasModifier { get; }
    public int Hash { get; }
    public IFNISAnimation? NextAnimation { get; set; }
    public FNISAnimType TemplateType { get; }
    public void BuildAnimation(
        DirectoryInfo templateFolder,
        IFNISAnimationListBuildContext context
    );
    public bool BuildBehavior(IFNISAnimationListBuildContext buildContext);
    public void BuildFlags(
        IFNISAnimationListBuildContext buildContext,
        IPackFileGraph graph,
        hkbStateMachineStateInfo stateInfo,
        hkbClipGenerator clip
    );
    public string ToString();
}
