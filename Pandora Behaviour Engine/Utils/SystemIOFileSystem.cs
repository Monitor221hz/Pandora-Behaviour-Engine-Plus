// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using System.IO;

namespace Pandora.Utils;

public class SystemIOFileSystem : IFileSystem
{
	public bool FileExists(string path) => File.Exists(path);

	public bool DirectoryExists(string path) => Directory.Exists(path);

	public string? GetParentDirectoryPath(string path) => Directory.GetParent(path)?.FullName;

	public string GetFileName(string path) => Path.GetFileName(path);

	public string Combine(params string[] paths) => Path.Combine(paths);
}