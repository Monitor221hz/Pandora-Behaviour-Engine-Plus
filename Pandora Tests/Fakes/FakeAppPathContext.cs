// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using Pandora.Paths.Contexts;
using System.Diagnostics;
using Pandora.Paths.Extensions;

namespace PandoraTests.Fakes;

public sealed class FakeAppPathContext : IAppPathContext
{
    public DirectoryInfo AssemblyDirectory { get; }
    public DirectoryInfo TemplateDirectory { get; }
    public DirectoryInfo EngineDirectory { get; }
    public FileInfo PathConfig { get; }

    public FakeAppPathContext(DirectoryInfo root)
    {
        AssemblyDirectory = new(Path.GetDirectoryName(Process.GetCurrentProcess().MainModule!.FileName!)!);
        EngineDirectory = new(root.FullName / "Pandora_Engine");
        TemplateDirectory = new(EngineDirectory.FullName / "Skyrim" / "Template");
        PathConfig = new(EngineDirectory.FullName / "Paths.json");

        TemplateDirectory.Create();
        EngineDirectory.Create();
    }
}
