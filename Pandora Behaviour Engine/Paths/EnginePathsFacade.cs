// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

﻿using Pandora.Paths.Abstractions;
using System.IO;

namespace Pandora.Paths;

public sealed class EnginePathsFacade(
	IApplicationPaths appPaths,
	IUserPaths userPaths,
	IOutputPaths outputPaths) : IEnginePathsFacade
{
	public DirectoryInfo GameDataFolder => userPaths.GameData;
	public DirectoryInfo OutputFolder => userPaths.Output;

	public DirectoryInfo AssemblyFolder => appPaths.AssemblyDirectory;
	public DirectoryInfo TemplateFolder => appPaths.TemplateDirectory;
	public DirectoryInfo EngineFolder => appPaths.EngineDirectory;

	public FileInfo PathConfig => appPaths.PathConfig;

	public DirectoryInfo OutputEngineFolder => outputPaths.PandoraEngineDirectory;
	public DirectoryInfo OutputMeshesFolder => outputPaths.MeshesDirectory;

	public FileInfo OutputPreviousFile => outputPaths.PreviousOutputFile;
	public FileInfo ActiveModsFile => outputPaths.ActiveModsFile;
}
