// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using NLog;
using Pandora.API.Patch;
using Pandora.Core.Mods.Abstractions;
using Pandora.Core.Mods.Extensions;
using Pandora.Core.Mods.Providers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Pandora.Core.Mods;

public class ModLoaderService : IModLoaderService
{
	private static readonly NLog.Logger Logger = LogManager.GetCurrentClassLogger();

	private readonly IEnumerable<IModInfoProvider> _providers;

	public ModLoaderService(IEnumerable<IModInfoProvider> providers)
	{
		_providers = providers;
	}

	public async Task<HashSet<IModInfo>> LoadModsAsync(IEnumerable<DirectoryInfo> directories)
	{
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
				Logger.Warn(
					ex,
					$"Error loading mods from directory '{p.path}' using provider '{p.provider.GetType().Name}'."
				);
				return Enumerable.Empty<IModInfo>();
			}
		});

		var results = await Task.WhenAll(searchTasks);

		var merged = new Dictionary<(string Code, Version Version), IModInfo>();
		foreach (var mods in results)
		{
			foreach (var mod in mods)
			{
				var key = (mod.Code, mod.Version);
				if (merged.TryGetValue(key, out var existing))
				{
					if (mod.Format == IModInfo.ModFormat.Pandora && existing.Format != IModInfo.ModFormat.Pandora)
					{
						Logger.Info(
							$"Mod Loader > Code \"{mod.Code}\" > Preferring Pandora format over {existing.Format} for mod \"{mod.Name}\"."
						);
						merged[key] = mod;
					}
				}
				else
				{
					merged.Add(key, mod);
				}
			}
		}

		var modInfos = new HashSet<IModInfo>(merged.Values);

		modInfos.NormalizePriorities();

		return modInfos;
	}
}