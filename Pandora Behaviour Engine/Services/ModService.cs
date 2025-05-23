using Pandora.API.Patch;
using Pandora.Data;
using Pandora.Models;
using Pandora.ViewModels;
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
	private Dictionary<string, IModInfo> modsByCode = [];
	private bool modInfoCache = false;
	private FileInfo activeModConfig;

	public ModService(string activeModConfigPath)
	{
		activeModConfig = new FileInfo(activeModConfigPath);
	}

	public async Task<List<IModInfo>> LoadModsAsync(DirectoryInfo launchDirectory, DirectoryInfo currentDirectory)
	{
		modsByCode.Clear();
		HashSet<IModInfo> modInfos = [];

		//Program folder
		await LoadModFolder(modInfos, await nemesisModInfoProvider?.GetInstalledMods(launchDirectory + "\\Nemesis_Engine\\mod")!);
		await LoadModFolder(modInfos, await pandoraModInfoProvider?.GetInstalledMods(launchDirectory + "\\Pandora_Engine\\mod")!);
		//Working folder, or Skyrim\Data folder
		await LoadModFolder(modInfos, await nemesisModInfoProvider?.GetInstalledMods(BehaviourEngine.CurrentDirectory + "\\Nemesis_Engine\\mod")!);
		await LoadModFolder(modInfos, await pandoraModInfoProvider?.GetInstalledMods(BehaviourEngine.CurrentDirectory + "\\Pandora_Engine\\mod")!);
		//Current (defaults to Working folder) or Output (set via -o) folder
		await LoadModFolder(modInfos, await nemesisModInfoProvider?.GetInstalledMods(currentDirectory + "\\Nemesis_Engine\\mod")!);
		await LoadModFolder(modInfos, await pandoraModInfoProvider?.GetInstalledMods(currentDirectory + "\\Pandora_Engine\\mod")!);

		List<IModInfo> modInfoList = [.. modInfos];
		modInfoList.ForEach(a => modsByCode.Add(a.Code, a));

		modInfoCache = LoadActiveMods(modInfoList);

		return modInfoList.OrderBy(m => m.Code == "pandora").ThenBy(m => m.Priority == 0).ThenBy(m => m.Priority).ThenBy(m => m.Name).ToList();
	}

	private async Task LoadModFolder(HashSet<IModInfo> modInfos, List<IModInfo> mods)
	{
		if (mods == null) return;
		modInfos.UnionWith(mods);
	}

	public bool LoadActiveMods(List<IModInfo> loadedMods)
	{
		if (!activeModConfig.Exists) return false;
		foreach (var mod in loadedMods)
		{
			if (mod == null) continue;
			mod.Active = false;
		}
		using (var readStream = activeModConfig.OpenRead())
		{
			using (var streamReader = new StreamReader(readStream))
			{
				string? expectedLine;
				uint priority = 0;
				while ((expectedLine = streamReader.ReadLine()) != null)
				{
					if (!modsByCode.TryGetValue(expectedLine, out IModInfo? modInfo)) continue;
					priority++;
					modInfo.Priority = priority;
					modInfo.Active = true;
				}
			}
		}
		return true;
	}

	public void SaveActiveMods(List<IModInfo> activeMods)
	{
		activeModConfig.Directory?.Create();
		using (var writeStream = activeModConfig.Create())
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

	public void AssignModPrioritiesFromViewModels(IEnumerable<ModInfoViewModel> modViewModels)
	{
		uint priority = 0;
		foreach (var modViewModel in modViewModels)
		{
			priority++;
			modViewModel.Priority = priority;
		}
	}

	public List<IModInfo> GetActiveModsByPriority(IEnumerable<ModInfoViewModel> sourceMods) =>
		sourceMods.Where(m => m.Active).Select(m => m.ModInfo).OrderBy(m => m.Code == "pandora").ThenBy(m => m.Priority == 0).ThenBy(m => m.Priority).ToList();
}