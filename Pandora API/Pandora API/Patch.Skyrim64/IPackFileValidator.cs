// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using System.Xml.Linq;

namespace Pandora.API.Patch.Skyrim64;

public interface IPackFileValidator
{
    void TrackElement(XElement element);
    void Validate(
        IPackFile packFile,
        params scoped ReadOnlySpan<List<IPackFileChange>> changeLists
    );
    bool ValidateEventsAndVariables(IPackFileGraph graph);
    void ValidateTrackedElements();
}
