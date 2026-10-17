// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using Pandora.Core.Paths.Extensions;
using Pandora.Core.Platform.CreationEngine;
using System;
using System.IO;

namespace Pandora.Core.Paths.Validation;

public sealed class GameDataValidator(IGameDescriptor descriptor) : IGameDataValidator
{
	public DirectoryInfo? Normalize(DirectoryInfo input)
	{
		var dataDir = GetDataDirectory(input);
		return dataDir != null && IsValid(dataDir) ? dataDir : null;
	}

	public bool IsValid(DirectoryInfo input)
	{
		var dataDir = GetDataDirectory(input);
		if (dataDir is null)
			return false;

		var gameRoot = dataDir.Parent;
		if (gameRoot is null || !gameRoot.Exists)
			return false;

		foreach (var exe in descriptor.ExecutableNames)
		{
			if (FileExistsCaseInsensitive(gameRoot, exe))
				return true;
		}

		return false;
	}

	private static bool FileExistsCaseInsensitive(DirectoryInfo directory, string fileName)
	{
		if (File.Exists(directory.FullName / fileName))
			return true;

		try
		{
			return directory.EnumerateFiles().Any(f => f.Name.Equals(fileName, StringComparison.OrdinalIgnoreCase));
		}
		catch
		{
			return false;
		}
	}

	private static DirectoryInfo? GetDataDirectory(DirectoryInfo input)
	{
		if (!input.Exists)
			return null;

		if (input.Name.Equals("Data", StringComparison.OrdinalIgnoreCase))
			return input;

		var candidate = new DirectoryInfo(input.FullName / "Data");
		if (candidate.Exists)
			return candidate;

		try
		{
			return input.EnumerateDirectories()
				.FirstOrDefault(d => d.Name.Equals("Data", StringComparison.OrdinalIgnoreCase));
		}
		catch
		{
			return null;
		}
	}
}