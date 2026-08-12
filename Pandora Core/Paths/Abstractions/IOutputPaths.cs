// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using System.IO;

namespace Pandora.Core.Paths.Abstractions;

public interface IOutputPaths
{
	DirectoryInfo PandoraEngineDirectory { get; }
	DirectoryInfo MeshesDirectory { get; }

	FileInfo ActiveModsFile { get; }
	FileInfo PreviousOutputFile { get; }
}