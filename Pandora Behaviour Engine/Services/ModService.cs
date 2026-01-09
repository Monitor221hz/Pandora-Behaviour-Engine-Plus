// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using DynamicData;
using DynamicData.Binding;
using Pandora.API.Data;
using Pandora.API.DTOs;
using Pandora.API.Services;
using Pandora.Data;
using Pandora.Utils;
using Pandora.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Pandora.Services;

public interface IModService
{
	IObservable<IChangeSet<ModInfoViewModel>> Connect();

	Task RefreshModsAsync();

	Task SaveSettingsAsync();
}

public class ModService : IModService
{
	private readonly IModLoaderService _loader;
	private readonly IModSettingsService _settings;
	private readonly IPathResolver _pathResolver;

	public ObservableCollectionExtended<ModInfoViewModel> Source { get; } = [];

	public IObservable<IChangeSet<ModInfoViewModel>> Connect() => Source.ToObservableChangeSet();

	public ModService(IModLoaderService loader, IModSettingsService settings, IPathResolver pathResolver)
	{
		_loader	= loader;
		_settings = settings;
		_pathResolver = pathResolver;
	}

	private IEnumerable<IModInfoProvider> ResolveProviders()
	{
		yield return new NemesisModInfoProvider();
		yield return new PandoraModInfoProvider();
	}

	private IEnumerable<DirectoryInfo> ResolveDirectories()
	{
		yield return _pathResolver.GetAssemblyFolder();
		yield return _pathResolver.GetCurrentFolder();
		yield return _pathResolver.GetGameDataFolder();
	}

	private void ApplySettings(List<ModInfoViewModel> mods, List<ModSaveEntry> settings)
	{
		var settingsMap = settings
			.DistinctBy(x => x.Code)
			.ToDictionary(x => x.Code, StringComparer.OrdinalIgnoreCase);

		foreach (var mod in mods)
		{
			if (settingsMap.TryGetValue(mod.Code, out var entry))
			{
				mod.Active = entry.Active;
				mod.Priority = entry.Priority;
			}
		}
	}

	public async Task RefreshModsAsync()
	{
		var rawMods = await _loader.LoadModsAsync(ResolveProviders(), ResolveDirectories());

		var modVMs = rawMods.Select(m => new ModInfoViewModel(m)).ToList();

		var savedSettings = await _settings.LoadAsync();

		if (savedSettings.Count == 0)
		{
			modVMs.ResetToAlphanumeric();
		}
		else
		{
			ApplySettings(modVMs, savedSettings);
		}

		modVMs.NormalizePriorities();

		Source.Load(modVMs);
	}

	public async Task SaveSettingsAsync()
	{
		var entries = Source
			.Where(m => !m.IsPandora)
			.Select(m => new ModSaveEntry(m.Code, m.Active, m.Priority))
			.OrderBy(e => e.Priority);

		await _settings.SaveAsync(entries);
	}
}