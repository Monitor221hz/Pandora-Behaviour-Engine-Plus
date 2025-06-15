using Pandora.API.Patch;
using Pandora.Data;
using Pandora.Utils;
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

	private readonly ModSettingsService modSettingsService;

	public ModService(string modSavePath)
	{
		modSettingsService = new ModSettingsService(modSavePath);
	}

	public async Task<List<IModInfo>> LoadModsAsync(params DirectoryInfo[] directories)
	{
		var modInfos = new HashSet<IModInfo>();

		foreach (var (path, provider) in GetModPaths(directories).DistinctBy(p => p.path))
		{
			var mods = await provider.GetInstalledMods(path);
			if (mods is not null)
				modInfos.UnionWith(mods);
		}

		return [.. modInfos];
	}

	private IEnumerable<(string path, IModInfoProvider provider)> GetModPaths(IEnumerable<DirectoryInfo> directories)
	{
		foreach (var dir in directories)
		{
			yield return (Path.Join(dir.FullName, "Nemesis_Engine", "mod"), nemesisModInfoProvider);
			yield return (Path.Join(dir.FullName, "Pandora_Engine", "mod"), pandoraModInfoProvider);
		}
	}

	public async Task<List<ModInfoViewModel>> PrepareModViewModelsAsync(IEnumerable<IModInfo> modInfos)
	{
		var viewModels = modInfos.Select(m => new ModInfoViewModel(m)).ToList();

		await modSettingsService.ApplySettingsAsync(viewModels);

		var pandoraMod = ModUtils.ExtractPandoraMod(viewModels);
		if (pandoraMod is null)
		{
			const string error = "FATAL ERROR: Pandora Base does not exist.";
			EngineLogger.AppendLine(error);
			logger.Error(error);
		}

		return viewModels.OrderBy(m => m.Priority).ToList();
	}


	public Task SaveActiveModsAsync(IEnumerable<ModInfoViewModel> mods) =>
		modSettingsService.SaveSettingsAsync(mods);
}
