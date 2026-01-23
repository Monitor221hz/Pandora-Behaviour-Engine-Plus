// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

﻿using Pandora.Paths.Abstractions;
using Pandora.Paths.Extensions;
using System.IO;

namespace Pandora.Paths;

public sealed class OutputPaths(IUserPaths userPaths) : IOutputPaths
{
	private const string ENGINE_FOLDER = "Pandora_Engine";
    private const string MESH_FOLDER = "meshes";
    private const string ACTIVE_MODS = "ActiveMods.json";
    private const string PREVIOUS_OUT = "PreviousOutput.txt";

	public DirectoryInfo PandoraEngineDirectory => new(userPaths.Output.FullName / ENGINE_FOLDER);
    public DirectoryInfo MeshesDirectory => new(userPaths.Output.FullName / MESH_FOLDER);

    public FileInfo ActiveModsFile => new(PandoraEngineDirectory.FullName / ACTIVE_MODS);
    public FileInfo PreviousOutputFile => new(PandoraEngineDirectory.FullName / PREVIOUS_OUT);
}

