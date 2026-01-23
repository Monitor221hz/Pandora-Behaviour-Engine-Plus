// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

﻿using System.IO;

namespace Pandora.Paths.Abstractions;

public interface IApplicationPaths
{
	/// <summary>
	/// Gets the real file system path to the folder containing the running executable.
	///
	/// Unlike typical assembly path methods, this returns the actual disk path even when
	/// running inside virtualized environments like Mod Organizer 2 (MO2), bypassing the VFS.
	/// </summary>
	DirectoryInfo AssemblyDirectory { get; }
	DirectoryInfo TemplateDirectory { get; }
	DirectoryInfo EngineDirectory { get; }
	FileInfo PathConfig { get; }
}
