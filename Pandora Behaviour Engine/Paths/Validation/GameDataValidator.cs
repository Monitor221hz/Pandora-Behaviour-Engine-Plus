// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

﻿using Pandora.Platform.CreationEngine;
using System;
using System.IO;
using Pandora.Paths.Extensions;

namespace Pandora.Paths.Validation;

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
			if (File.Exists(gameRoot.FullName / exe))
				return true;
		}

		return false;
	}

	private static DirectoryInfo? GetDataDirectory(DirectoryInfo input)
	{
		if (!input.Exists)
			return null;

		if (input.Name.Equals("Data", StringComparison.OrdinalIgnoreCase))
			return input;

		var candidate = new DirectoryInfo(input.FullName / "Data");
		return candidate.Exists ? candidate : null;
	}

}
