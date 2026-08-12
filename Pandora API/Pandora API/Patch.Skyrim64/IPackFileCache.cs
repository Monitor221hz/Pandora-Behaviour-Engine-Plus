// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using System.Diagnostics.CodeAnalysis;

namespace Pandora.API.Patch.Skyrim64;

public interface IPackFileCache
{
    IPackFile LoadPackFile(FileInfo file);
    IPackFileCharacter LoadPackFileCharacter(FileInfo file);
    IPackFileCharacter LoadPackFileCharacter(FileInfo file, IProject project);
    IPackFileGraph LoadPackFileGraph(FileInfo file);
    IPackFileGraph LoadPackFileGraph(FileInfo file, IProject project);
    IPackFileSkeleton LoadPackFileSkeleton(FileInfo file);
    IPackFileSkeleton LoadPackFileSkeleton(FileInfo file, IProject project);
    bool TryLookupSharedProjects(
        IPackFile packFile,
        [NotNullWhen(true)] out List<IProject>? projects
    );
}
