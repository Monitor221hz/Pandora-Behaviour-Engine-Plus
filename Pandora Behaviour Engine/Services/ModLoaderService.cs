// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NLog;
using Pandora.API.Data;
using Pandora.API.Patch;
using Pandora.API.Services;
using Pandora.Utils;

namespace Pandora.Services;

public class ModLoaderService : IModLoaderService
{
	private static readonly NLog.Logger logger = LogManager.GetCurrentClassLogger();

	public async Task<HashSet<IModInfo>> LoadModsAsync(
		IEnumerable<IModInfoProvider> providers,
		IEnumerable<DirectoryInfo> directories
	)
	{
		var modInfos = new HashSet<IModInfo>();

		var pathsToScan = PathDiscoveryUtils
			.ResolveProviderPaths(directories, providers)
			.DistinctBy(p => p.path, StringComparer.OrdinalIgnoreCase)
			.ToList();

		var searchTasks = pathsToScan.Select(async p =>
		{
			try
			{
				return await p.provider.GetInstalledMods(p.path);
			}
			catch (Exception ex)
			{
				logger.Warn(
					ex,
					$"Error loading mods from directory '{p.path}' using provider '{p.provider.GetType().Name}'."
				);
				return Enumerable.Empty<IModInfo>();
			}
		});

		var results = await Task.WhenAll(searchTasks);

		foreach (var mods in results)
		{
			modInfos.UnionWith(mods);
		}

		return modInfos;
	}

}