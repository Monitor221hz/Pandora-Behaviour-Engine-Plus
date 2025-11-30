// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DynamicData.Binding;
using NLog;
using Pandora.API.Patch;
using Pandora.Data;
using Pandora.Logging;
using Pandora.Utils;
using Pandora.ViewModels;

namespace Pandora.Services;

public static class ModLoader
{
	private static readonly NLog.Logger logger = LogManager.GetCurrentClassLogger();

	public static async Task<HashSet<IModInfo>> LoadModsAsync(
		IEnumerable<IModInfoProvider> providers,
		IEnumerable<DirectoryInfo> directories
	)
	{
		var modInfos = new HashSet<IModInfo>();

		var pathsToScan = ModUtils
			.ResolvePaths(directories, providers)
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

	public static async Task LoadModsVMAsync(
		ObservableCollectionExtended<ModInfoViewModel> mods,
		IEnumerable<DirectoryInfo> directories,
		IEnumerable<IModInfoProvider> providers
	)
	{
		mods.Clear();

		var uniqueDirectories = directories
			.Where(d => d is not null && d.Exists)
			.DistinctBy(d => d.FullName, StringComparer.OrdinalIgnoreCase)
			.ToList();

		var modInfos = await LoadModsAsync(providers, uniqueDirectories);
		var modViewModels = modInfos.Select(m => new ModInfoViewModel(m)).ToList();

		await JsonModSettingsStore.ApplyAsync(modViewModels, PandoraPaths.ActiveModsFile.FullName);

		mods.AddRange(modViewModels);
		EngineLoggerAdapter.AppendLine($"Mods loaded.");
	}
}

public record ModSaveEntry
{
	public uint Priority { get; init; }
	public bool Active { get; init; }
}
