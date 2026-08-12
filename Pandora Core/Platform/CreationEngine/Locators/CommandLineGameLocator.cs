// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using Pandora.Core.CLI;
using System.IO;

namespace Pandora.Core.Platform.CreationEngine.Locators;

public sealed class CommandLineGameLocator(LaunchOptions options) : IGameLocator
{
	public DirectoryInfo? TryLocateGameData()
	{
		if (options.SkyrimGameDirectory is null)
			return null;

		return options.SkyrimGameDirectory;
	}
}