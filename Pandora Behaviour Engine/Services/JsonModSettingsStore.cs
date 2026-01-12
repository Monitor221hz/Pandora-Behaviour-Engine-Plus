// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using NLog;
using Pandora.Utils;
using Pandora.ViewModels;

namespace Pandora.Services;

public static class JsonModSettingsStore
{
	private static readonly Logger logger = LogManager.GetCurrentClassLogger();
	private static readonly JsonSerializerOptions jsonOptions = new() { WriteIndented = true };
	private static readonly Dictionary<string, ModSaveEntry> EmptySettings = new(
		StringComparer.OrdinalIgnoreCase
	);

	public static async Task<(
		bool Success,
		Dictionary<string, ModSaveEntry> Settings
	)> TryLoadAsync(string path)
	{
		if (!File.Exists(path))
			return (false, EmptySettings);

		try
		{
			var json = await File.ReadAllTextAsync(path);
			var data = JsonSerializer.Deserialize<Dictionary<string, ModSaveEntry>>(
				json,
				jsonOptions
			);

			if (data is null)
			{
				logger.Warn("Deserialization returned null.");
				return (false, EmptySettings);
			}

			var settings = data.Where(kvp => kvp.Value is not null)
				.ToDictionary(kvp => kvp.Key, kvp => kvp.Value!, StringComparer.OrdinalIgnoreCase);

			return (true, settings);
		}
		catch (Exception ex) when (ex is IOException or JsonException)
		{
			logger.Warn(ex, "Failed to load mod settings.");
			return (false, EmptySettings);
		}
	}

	public static async Task ApplyAsync(IEnumerable<ModInfoViewModel> mods, string settingsPath)
	{
		var (success, settings) = await TryLoadAsync(settingsPath);
		var modList = mods.ToList();

		if (!success || settings.Count == 0)
		{
			logger.Warn("Settings file missing or invalid. Assigning priorities alphanumerically.");
			ModUtils.SetAlphanumericPriorities(modList);
			logger.Info("Mod settings applied successfully.");
			return;
		}

		foreach (var mod in modList)
		{
			if (settings.TryGetValue(mod.Code, out var entry))
			{
				mod.Active = entry.Active;
				mod.Priority = entry.Priority;
			}
		}

		ModUtils.NormalizeModPriorities(modList);
		ModUtils.EnsurePandoraModActive(modList, (uint)modList.Count);
		logger.Info("Mod settings applied successfully.");
	}

	public static async Task SaveAsync(IEnumerable<ModInfoViewModel> mods, string settingsPath)
	{
		try
		{
			var settings = mods.Where(m => !ModUtils.IsPandoraMod(m))
				.OrderBy(m => m.Priority)
				.ToDictionary(
					m => m.Code,
					m => new ModSaveEntry { Active = m.Active, Priority = m.Priority },
					StringComparer.OrdinalIgnoreCase
				);

			var json = JsonSerializer.Serialize(settings, jsonOptions);

			await File.WriteAllTextAsync(settingsPath, json);
			logger.Info("Mod settings saved successfully.");
		}
		catch (Exception ex) when (ex is IOException or JsonException)
		{
			logger.Error(ex, "Error saving mod settings.");
			throw;
		}
	}
}
