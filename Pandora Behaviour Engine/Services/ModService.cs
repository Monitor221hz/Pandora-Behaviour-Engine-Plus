using DynamicData;
using Pandora.API.Patch;
using Pandora.Data;
using Pandora.Models;
using Pandora.ViewModels;
using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Pandora.Services;

public class ModService
{
	private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

	private readonly NemesisModInfoProvider nemesisModInfoProvider = new();
	private readonly PandoraModInfoProvider pandoraModInfoProvider = new();

	private FrozenDictionary<string, IModInfo> modsByCode;
	private bool modInfoCache = false;
	private readonly FileInfo activeModConfig;

	public ModService(string activeModConfigPath)
	{
		activeModConfig = new FileInfo(activeModConfigPath);
	}

	public async Task<List<IModInfo>> LoadModsAsync(params DirectoryInfo[] directories)
	{
		var modInfos = new HashSet<IModInfo>();

		var pathProviderPairs = directories
			.SelectMany(dir => new (string path, IModInfoProvider provider)[]
			{
				(Path.Combine(dir.FullName, "Nemesis_Engine", "mod"), nemesisModInfoProvider),
				(Path.Combine(dir.FullName, "Pandora_Engine", "mod"), pandoraModInfoProvider)
			})
			.DistinctBy(p => p.path)
			.ToArray();

		var tasks = pathProviderPairs
			.Select(pair => pair.provider.GetInstalledMods(pair.path))
			.ToArray();

		var results = await Task.WhenAll(tasks);

		foreach (var mods in results)
		{
			if (mods is not null)
				modInfos.UnionWith(mods);
		}

		var modInfoList = modInfos.ToList();
		modsByCode = modInfoList.ToFrozenDictionary(m => m.Code);

		modInfoCache = LoadActiveMods(modInfoList);

		return modInfoList.OrderBy(m => m.Code == "pandora")
			.ThenBy(m => m.Priority == 1)
			.ThenBy(m => m.Priority)
			.ThenBy(m => m.Name)
			.ToList();
	}

	public bool LoadActiveMods(List<IModInfo> loadedMods)
	{
		if (!activeModConfig.Exists) return false;

		foreach (var mod in loadedMods)
			mod.Active = false;

		using (FileStream readStream = activeModConfig.OpenRead())
		{
			using (StreamReader streamReader = new(readStream))
			{
				string? expectedLine;
				uint priority = 1;
				while ((expectedLine = streamReader.ReadLine()) != null)
				{
					if (!modsByCode.TryGetValue(expectedLine, out IModInfo? modInfo)) continue;
					modInfo.Priority = priority++;
					modInfo.Active = true;
				}
			}
		}
		return true;
	}

	public void SaveActiveMods(List<IModInfo> activeMods)
	{
		activeModConfig.Directory?.Create();
		using (FileStream writeStream = activeModConfig.Create())
		{
			using (StreamWriter streamWriter = new(writeStream))
			{
				foreach (var modInfo in activeMods)
				{
					streamWriter.WriteLine(modInfo.Code);
				}
			}
		}
	}

	public static void AssignModPrioritiesFromViewModels(IEnumerable<ModInfoViewModel> modViewModels)
	{
		uint priority = 1;
		foreach (var mod in modViewModels)
		{
			mod.Priority = priority++;
		}
	}

	public static List<IModInfo> GetActiveModsByPriority(IEnumerable<ModInfoViewModel> sourceMods) =>
		sourceMods.Where(m => m.Active)
			.Select(m => m.ModInfo)
			.OrderBy(m => m.Code == "pandora")
			.ThenBy(m => m.Priority == 1)
			.ThenBy(m => m.Priority)
			.ToList();
}