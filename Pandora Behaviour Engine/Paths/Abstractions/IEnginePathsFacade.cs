// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

﻿using System.IO;

namespace Pandora.Paths.Abstractions;

public interface IEnginePathsFacade
{
	DirectoryInfo GameDataFolder { get; }
	DirectoryInfo OutputFolder { get; }

	DirectoryInfo AssemblyFolder { get; }
	DirectoryInfo TemplateFolder { get; }
	DirectoryInfo EngineFolder { get; }


	DirectoryInfo OutputEngineFolder { get; }
	DirectoryInfo OutputMeshesFolder { get; }

	FileInfo OutputPreviousFile { get; }
	FileInfo ActiveModsFile { get; }
	FileInfo PathConfig { get; }
}
