// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Pandora.API.Patch;

namespace Pandora.Mods.Providers;

public abstract class FileBasedModInfoProvider : IModInfoProvider
{
	public async Task<List<IModInfo>> GetInstalledMods(string folderPath)
	{
		var folder = new DirectoryInfo(folderPath);
		if (!folder.Exists)
			return [];

		var mods = new List<IModInfo>();
		foreach (var modFolder in folder.EnumerateDirectories())
		{
			var infoFile = new FileInfo(Path.Combine(modFolder.FullName, InfoFileName));
			if (!infoFile.Exists)
				continue;

			var modInfo = await TryParseAsync(infoFile);
			if (modInfo is not null)
				mods.Add(modInfo);
		}
		return mods;
	}

	public abstract string SingleRelativePath { get; }
	protected abstract string InfoFileName { get; }

	protected abstract Task<IModInfo?> TryParseAsync(FileInfo infoFile);
}
