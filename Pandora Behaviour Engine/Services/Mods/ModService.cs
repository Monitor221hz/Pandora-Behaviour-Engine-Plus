// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using DynamicData;
using DynamicData.Binding;
using Pandora.API.Patch;
using Pandora.Services.Interfaces;
using Pandora.DTOs;
using Pandora.Utils;
using Pandora.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace Pandora.Services.Mods;

public class ModService : IModService, IDisposable
{
	private readonly IModLoaderService _loader;
	private readonly IModSettingsService _settings;
	private readonly IPathResolver _pathResolver;

	private List<IModInfo>? _cachedCoreMods;

	private IDisposable? _pathSubscription;

	public ObservableCollectionExtended<ModInfoViewModel> Source { get; } = [];

	public IObservable<IChangeSet<ModInfoViewModel>> Connect() => Source.ToObservableChangeSet();

	public ModService(IModLoaderService loader, IModSettingsService settings, IPathResolver pathResolver)
	{
		_loader	= loader;
		_settings = settings;
		_pathResolver = pathResolver;

		_pathSubscription = _pathResolver.GameDataFolderChanged
			.Skip(1)
			.Subscribe(async _ => await RefreshModsAsync());
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
		if (_cachedCoreMods == null)
		{
			var coreDirs = new[]
			{
				_pathResolver.GetAssemblyFolder(),
				_pathResolver.GetCurrentFolder()
			};

			var coreModsSet = await _loader.LoadModsAsync(coreDirs);
			_cachedCoreMods = coreModsSet.ToList();
		}

		var gameDirs = new[] { _pathResolver.GetGameDataFolder() };
		var gameModsSet = await _loader.LoadModsAsync(gameDirs);

		var allMods = _cachedCoreMods
			.Concat(gameModsSet)
			.DistinctBy(m => m.Code, StringComparer.OrdinalIgnoreCase)
			.ToList();

		var modVMs = allMods.Select(m => new ModInfoViewModel(m)).ToList();

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

	public IReadOnlyList<IModInfo> GetActiveMods()
	{
		return Source
			.Where(m => m.Active)
			.OrderBy(m => m.Priority)
			.ThenBy(m => !m.IsPandora)
			.Select(m => m.ModInfo)
			.ToList();
	}

	public void Dispose()
	{
		_pathSubscription?.Dispose();
	}
}