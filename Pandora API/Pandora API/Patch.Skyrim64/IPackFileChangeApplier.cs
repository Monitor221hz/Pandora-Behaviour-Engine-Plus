// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

namespace Pandora.API.Patch.Skyrim64;

public interface IPackFileChangeApplier
{
    void ApplyForNode(
        string nodeName,
        IPackFile packFile,
        List<IPackFileChangeOwner> changeSetList
    );
    void ApplyInOrder(IPackFile packFile, List<IPackFileChangeOwner> changeSetList);
}
