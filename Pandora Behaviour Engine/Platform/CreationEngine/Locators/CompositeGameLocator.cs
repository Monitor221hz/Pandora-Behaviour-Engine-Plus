// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using System.Collections.Generic;
using System.IO;

namespace Pandora.Platform.CreationEngine.Locators;

public sealed class CompositeGameLocator(IEnumerable<IGameLocator> locators) : IGameLocator
{
	public DirectoryInfo? TryLocateGameData()
	{
		foreach (var locator in locators)
		{
			var candidate = locator.TryLocateGameData();
			if (candidate is not null)
				return candidate;
		}

		return null;
	}
}