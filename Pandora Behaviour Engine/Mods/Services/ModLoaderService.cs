// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using NLog;
using Pandora.API.Patch;
using Pandora.Logging.Extensions;
using Pandora.Mods.Providers;
using Pandora.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Pandora.Mods.Services;

public class ModLoaderService : IModLoaderService
{
	private static readonly NLog.Logger logger = LogManager.GetCurrentClassLogger();

	private readonly IEnumerable<IModInfoProvider> _providers;

	public ModLoaderService(IEnumerable<IModInfoProvider> providers)
	{
		_providers = providers;
	}

	public async Task<HashSet<IModInfo>> LoadModsAsync(
		IEnumerable<DirectoryInfo> directories
	)
	{
		var modInfos = new HashSet<IModInfo>();

		var pathsToScan = ModPathResolver
			.Resolve(directories, _providers)
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

		logger.UiInfo($"{modInfos.Count} mods loaded.");

		return modInfos;
	}

}