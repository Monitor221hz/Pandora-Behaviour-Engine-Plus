// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Avalonia.Controls;
using Pandora.API.Patch;
using Pandora.Data;
using Pandora.Utils.Extensions;
using Pandora.ViewModels;

namespace Pandora.Utils;

public static class ModUtils
{
	public static Func<ModInfoViewModel, bool> BuildNameFilter(string searchText) =>
		mod =>
			string.IsNullOrWhiteSpace(searchText)
			|| mod.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase);


	public static IEnumerable<(string path, IModInfoProvider provider)> ResolvePaths(
		IEnumerable<DirectoryInfo> baseDirs,
		IEnumerable<IModInfoProvider> providers
	)
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
	public static bool? AreAllNonPandoraModsSelected(IReadOnlyCollection<ModInfoViewModel> mods)
	{
		if (mods.Count == 0)
			return false;

		var nonPandoraMods = mods.Where(m => !m.IsPandora).ToList();
		int activeCount = nonPandoraMods.Count(m => m.Active);

		return activeCount switch
		{
			0 => false,
			var count when count == nonPandoraMods.Count => true,
			_ => null,
		};
	}
}
