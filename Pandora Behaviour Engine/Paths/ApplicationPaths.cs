// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using Pandora.Paths.Abstractions;
using Pandora.Paths.Extensions;
using System;
using System.Diagnostics;
using System.IO;

namespace Pandora.Paths;

public sealed class ApplicationPaths : IApplicationPaths
{
	private const string PANDORA_ENGINE_FOLDERNAME = "Pandora_Engine";
	private const string CONFIG_FILENAME = "Settings.json";

	// Will be virtualized by MO2 to support different settings for different profiles
	public FileInfo ConfigFile { get; } = new FileInfo(Path.GetDirectoryName(Environment.ProcessPath)! / CONFIG_FILENAME);

	public DirectoryInfo AppDataDirectory { get; } =
		new(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) / "Pandora Behaviour Engine");

#pragma warning disable CA1839 // Use 'Environment.ProcessPath' in USVFS, this would return the virtualized path, while we want the real path.
	public DirectoryInfo AssemblyDirectory { get; } = new DirectoryInfo(Path.GetDirectoryName(Process.GetCurrentProcess().MainModule!.FileName)!);
#pragma warning restore CA1839 // Use 'Environment.ProcessPath'

	public DirectoryInfo EngineDirectory { get; }
	public DirectoryInfo TemplateDirectory { get; }

	public ApplicationPaths()
	{
		if (!AppDataDirectory.Exists)
			AppDataDirectory.Create();

		if (!ConfigFile.Exists)
		{
			// Migrate config from old location (before MO2 multi-profile support) if it exists
			var oldConfigPath = Path.Join(AppDataDirectory.FullName, CONFIG_FILENAME);
			if (File.Exists(oldConfigPath))
			{
				File.Copy(oldConfigPath, ConfigFile.FullName);
				// Refresh so ConfigFile.Exists will return true now
				ConfigFile.Refresh();
			}
		}

		EngineDirectory = new(AssemblyDirectory.FullName / PANDORA_ENGINE_FOLDERNAME);
		TemplateDirectory = new(EngineDirectory.FullName / "Skyrim" / "Template");
	}
}