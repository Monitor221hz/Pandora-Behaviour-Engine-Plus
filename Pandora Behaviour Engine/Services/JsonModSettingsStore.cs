using NLog;
using Pandora.Utils;
using Pandora.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Pandora.Services;

public interface IModSettingsStore
{
	Task ApplySettingsAsync(List<ModInfoViewModel> mods);
	Task SaveActiveModsAsync(IEnumerable<ModInfoViewModel> mods);
}

public class JsonModSettingsStore : IModSettingsStore
{
	private static readonly Logger logger = LogManager.GetCurrentClassLogger();
	private static readonly JsonSerializerOptions jsonOptions = new() { WriteIndented = true };
	private readonly string _settingsPath;

	public JsonModSettingsStore(string settingsPath)
	{
		_settingsPath = settingsPath 
			?? throw new ArgumentNullException(nameof(settingsPath));
	}

	public async Task ApplySettingsAsync(List<ModInfoViewModel> mods)
	{
		var settings = await LoadSettingsAsync();
		if (settings.Count == 0)
		{
			logger.Info("Mod settings file not found or corrupted, default settings used.");
			ModUtils.AssignPrioritiesAlphanumerically(mods);
			return;
		}

		foreach (var mod in mods.Where(m => !ModUtils.IsPandora(m)))
		{
			if (settings.TryGetValue(mod.Code, out var entry))
			{
				mod.Active = entry.Active;
				mod.Priority = entry.Priority;
			}
			else
			{
				mod.Active = true;
				mod.Priority = uint.MaxValue;
			}
		}
	}

	public async Task SaveActiveModsAsync(IEnumerable<ModInfoViewModel> mods)
	{
		try
		{
			var settings = mods
				.Where(m => !ModUtils.IsPandora(m))
				.ToDictionary(
					m => m.Code,
					m => new ModSaveEntry
					{
						Active = m.Active,
						Priority = m.Priority
					},
					StringComparer.OrdinalIgnoreCase
				);

			var json = JsonSerializer.Serialize(settings, jsonOptions);
			await File.WriteAllTextAsync(_settingsPath, json);
			logger.Info("Mod settings saved successfully.");
		}
		catch (Exception ex) when (ex is IOException or JsonException)
		{
			logger.Error(ex, "Error saving mod settings.");
			throw;
		}
	}

	private async Task<Dictionary<string, ModSaveEntry>> LoadSettingsAsync()
	{
		if (!File.Exists(_settingsPath))
			return new(StringComparer.OrdinalIgnoreCase);

		try
		{
			var json = await File.ReadAllTextAsync(_settingsPath);
			var data = JsonSerializer.Deserialize<Dictionary<string, ModSaveEntry>>(json, jsonOptions);
			return data?.Where(kvp => kvp.Value is not null)
				.ToDictionary(kvp => kvp.Key, kvp => kvp.Value!, StringComparer.OrdinalIgnoreCase)
				?? new(StringComparer.OrdinalIgnoreCase);
		}
		catch (Exception ex) when (ex is IOException or JsonException)
		{
			logger.Warn(ex, "Error loading settings, returning empty dictionary.");
			return new(StringComparer.OrdinalIgnoreCase);
		}
	}
}
