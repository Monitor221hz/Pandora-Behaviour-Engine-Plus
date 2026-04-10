// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using System;
using System.Diagnostics;
using System.IO;
using Pandora.Paths.Abstractions;
using Pandora.Paths.Extensions;

namespace Pandora.Paths;

public sealed class ApplicationPaths : IApplicationPaths
{
	private const string PANDORA_ENGINE_FOLDERNAME = "Pandora_Engine";
	private const string CONFIG_FILE = "Settings.json";

	public FileInfo PathConfig => _pathConfig.Value;
	private readonly Lazy<FileInfo> _pathConfig;
	private readonly Lazy<DirectoryInfo> _assemblyDirectory;

	public DirectoryInfo AppDataDirectory =>
		new(
			Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)
				/ "Pandora Behaviour Engine"
		);
	public DirectoryInfo AssemblyDirectory => _assemblyDirectory.Value;
	public DirectoryInfo EngineDirectory =>
		new(AssemblyDirectory.FullName / PANDORA_ENGINE_FOLDERNAME);
	public DirectoryInfo TemplateDirectory => new(EngineDirectory.FullName / "Skyrim" / "Template");

	public ApplicationPaths()
	{
		if (!AppDataDirectory.Exists)
			AppDataDirectory.Create();

		_pathConfig = new Lazy<FileInfo>(() =>
			new FileInfo(AppDataDirectory.FullName / CONFIG_FILE)
		);
#pragma warning disable CA1839 // Use 'Environment.ProcessPath' in USVFS, this would return the virtualized path, while we want the real path.
		_assemblyDirectory = new Lazy<DirectoryInfo>(() =>
			new DirectoryInfo(
				Path.GetDirectoryName(Process.GetCurrentProcess().MainModule!.FileName)!
			)
		);
#pragma warning restore CA1839 // Use 'Environment.ProcessPath'
	}
}
