// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using Pandora.Paths.Contexts;
using Pandora.Paths.Extensions;

namespace PandoraTests.Fakes;

public sealed class FakeOutputPathContext : IOutputPathContext
{
    public DirectoryInfo PandoraEngineDirectory { get; }
    public DirectoryInfo MeshesDirectory { get; }

    public FileInfo PreviousOutputFile { get; }
    public FileInfo ActiveModsFile { get; }

    public FakeOutputPathContext(DirectoryInfo outputRoot)
    {
        PandoraEngineDirectory = new(outputRoot.FullName / "Pandora_Engine");
        MeshesDirectory = new(outputRoot.FullName / "meshes");
        PreviousOutputFile = new(PandoraEngineDirectory.FullName / "PreviousOutput.txt");
        ActiveModsFile = new(PandoraEngineDirectory.FullName / "ActiveMods.json");

        PandoraEngineDirectory.Create();
        MeshesDirectory.Create();
    }
}
