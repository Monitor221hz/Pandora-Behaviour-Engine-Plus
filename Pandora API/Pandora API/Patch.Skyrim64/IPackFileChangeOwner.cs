// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using System.Xml.Linq;

namespace Pandora.API.Patch.Skyrim64;

public interface IPackFileChangeOwner
{
    IModInfo Origin { get; set; }
    void AddChange(IPackFileChange change);
    void AddElementAsChange(XElement element);
    void Apply(IPackFile packFile);
    void ApplyForNode(string nodeName, IPackFile packFile);
    void ApplyForType(IPackFile packFile, IPackFileChange.ChangeType changeType);
    void ApplyInOrder(IPackFile packFile);
    void Validate(IPackFile packFile, IPackFileValidator validator);
}
