// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using DynamicData;
using DynamicData.Binding;
using NLog;
using Pandora.API.Patch;
using Pandora.DTOs;
using Pandora.Logging.Extensions;
using Pandora.Mods.Extensions;
using Pandora.Paths.Abstractions;
using Pandora.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace Pandora.Mods.Services;

public class ModService : IModService, IDisposable
{
	private static readonly NLog.Logger logger = LogManager.GetCurrentClassLogger();

	private readonly IModLoaderService _loader;
	private readonly IModSettingsService _settings;
	private readonly IEnginePathsFacade _pathContext;

	private List<IModInfo>? _cachedCoreMods;

	private readonly CompositeDisposable _disposables = new();

	public ObservableCollectionExtended<ModInfoViewModel> Source { get; } = [];

	public IObservable<IChangeSet<ModInfoViewModel>> Connect() => Source.ToObservableChangeSet();

	public ModService(IModLoaderService loader, IModSettingsService settings, IEnginePathsFacade pathContext)
	{
		_loader	= loader;
		_settings = settings;
		_pathContext = pathContext;
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
			var coreModsSet = await _loader.LoadModsAsync([_pathContext.AssemblyFolder]);
			_cachedCoreMods = coreModsSet.ToList();
		}

		var gameDirs = new[] { _pathContext.GameDataFolder };
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

		logger.UiInfo($"{Source.Count} mods loaded.");
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

	public void Dispose() => _disposables.Dispose();
}