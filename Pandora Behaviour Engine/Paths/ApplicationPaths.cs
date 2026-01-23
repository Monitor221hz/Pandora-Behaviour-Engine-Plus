// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

﻿using Pandora.Paths.Abstractions;
using Pandora.Paths.Extensions;
using System;
using System.Diagnostics;
using System.IO;

namespace Pandora.Paths;

public sealed class ApplicationPaths : IApplicationPaths
{
	private const string PANDORA_ENGINE_FOLDERNAME = "Pandora_Engine";
	private const string CONFIG_FILE = "Paths.json";

	public FileInfo PathConfig => _pathConfig.Value;
	private readonly Lazy<FileInfo> _pathConfig;

	public DirectoryInfo AssemblyDirectory => new(Path.GetDirectoryName(Process.GetCurrentProcess().MainModule!.FileName!)!);
	public DirectoryInfo EngineDirectory => new(AssemblyDirectory.FullName / PANDORA_ENGINE_FOLDERNAME);
	public DirectoryInfo TemplateDirectory => new(EngineDirectory.FullName / "Skyrim" / "Template");

	public ApplicationPaths()
	{
		_pathConfig = new Lazy<FileInfo>(() => new FileInfo(EngineDirectory.FullName / CONFIG_FILE));
	}
}
 