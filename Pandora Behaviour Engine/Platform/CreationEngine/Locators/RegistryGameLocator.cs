// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

﻿using GameFinder.RegistryUtils;
using NexusMods.Paths;
using System;
using System.IO;

namespace Pandora.Platform.CreationEngine.Locators;

public sealed class RegistryGameLocator(
	IGameDescriptor gameDescriptor,
	IRegistry? registry,
	IFileSystem fileSystem) : IGameLocator
{
	public DirectoryInfo? TryLocateGameData()
	{
		if (!OperatingSystem.IsWindows() || registry is null)
			return null;

		var key = registry.OpenBaseKey(RegistryHive.LocalMachine).OpenSubKey(gameDescriptor.SubKey);
		if (key == null)
			return null;

		if (key.GetValue("Installed Path") is not string installPath)
			return null;

		var game = fileSystem.FromUnsanitizedFullPath(installPath);
		if (!game.DirectoryExists())
			return null;

		var dirInfo = new DirectoryInfo(game.GetFullPath());

		return dirInfo;
	}
}

