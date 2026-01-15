// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using Pandora.Mods.Providers;
using System.Collections.Generic;
using System.IO;

namespace Pandora.Utils;

public static class ModPathResolver
{
	public static IEnumerable<(string path, IModInfoProvider provider)> Resolve(
		IEnumerable<DirectoryInfo> baseDirs,
		IEnumerable<IModInfoProvider> providers)
	{
		foreach (var dir in baseDirs)
		{
			foreach (var provider in providers)
			{
				var fullPath = Path.Combine(dir.FullName, provider.SingleRelativePath);
				if (Directory.Exists(fullPath))
					yield return (fullPath, provider);
			}
		}
	}
}