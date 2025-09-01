// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

namespace Pandora.Utils;

public interface IFileSystem
{
	bool FileExists(string path);

	bool DirectoryExists(string path);

	string? GetParentDirectoryPath(string path);

	string GetFileName(string path);

	string Combine(params string[] paths);
}