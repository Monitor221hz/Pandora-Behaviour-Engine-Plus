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

public class ModSaveEntry
{
	public uint Priority { get; set; }
	public bool Active { get; set; }
}

public class ModSettingsService
{
	private static readonly Logger logger = LogManager.GetCurrentClassLogger();

	private readonly string _settingsPath;
	private static readonly JsonSerializerOptions _jsonOptions = new()
	{
		WriteIndented = true
	};

	public ModSettingsService(string settingsPath)
	{
		_settingsPath = settingsPath;
	}

	public async Task SaveSettingsAsync(IEnumerable<ModInfoViewModel> mods)
	{
		var saveData = new Dictionary<string, ModSaveEntry>(StringComparer.OrdinalIgnoreCase);

		foreach (var mod in mods)
		{
			if (ModUtils.IsPandora(mod))
				continue;

			saveData[mod.Code] = new ModSaveEntry
			{
				Priority = mod.Priority,
				Active = mod.Active
			};
		}

		var json = JsonSerializer.Serialize(saveData, _jsonOptions);
		await File.WriteAllTextAsync(_settingsPath, json);
	}

	public async Task ApplySettingsAsync(IEnumerable<ModInfoViewModel> mods)
	{
		var modList = mods.ToList();

		if (!File.Exists(_settingsPath))
		{
			ModUtils.AssignPrioritiesAlphanumerically(modList);
			return;
		}

		try
		{
			var json = await File.ReadAllTextAsync(_settingsPath);
			var saved = JsonSerializer.Deserialize<Dictionary<string, ModSaveEntry>>(json)
						?? new(StringComparer.OrdinalIgnoreCase);

			var pandora = modList.FirstOrDefault(ModUtils.IsPandora);
			var nonPandoraMods = modList.Where(m => !ModUtils.IsPandora(m)).ToList();

			var ordered = new List<ModInfoViewModel>();

			foreach (var mod in nonPandoraMods)
			{
				if (saved.TryGetValue(mod.Code, out var entry))
				{
					mod.Active = entry.Active;
					mod.Priority = 0;
					ordered.Add(mod);
				}
			}

			var newMods = nonPandoraMods.Except(ordered).ToList();
			foreach (var mod in newMods)
			{
				mod.Active = true;
				mod.Priority = 0;
				ordered.Add(mod);
			}

			uint priority = 1;
			foreach (var mod in ordered)
			{
				mod.Priority = priority++;
			}

			if (pandora is not null)
			{
				pandora.Active = true;
				pandora.Priority = priority;
			}
		}
		catch (Exception ex) when (ex is IOException or JsonException)
		{
			logger.Error(ex, "Failed to load mod settings");
			ModUtils.AssignPrioritiesAlphanumerically(modList);
		}
	}

}
