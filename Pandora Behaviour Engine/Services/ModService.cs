using NLog;
using Pandora.API.Patch;
using Pandora.Data;
using Pandora.Utils;
using Pandora.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Pandora.Services;

public class ModService
{
	private static readonly NLog.Logger logger = LogManager.GetCurrentClassLogger();

	private readonly IModSettingsStore _settingsStore;
	private readonly IEnumerable<IModInfoProvider> _modInfoProviders;

	public ModService(IModSettingsStore settingsStore, IEnumerable<IModInfoProvider> modInfoProviders)
	{
		_settingsStore = settingsStore ?? throw new ArgumentNullException(nameof(settingsStore));
		_modInfoProviders = modInfoProviders ?? throw new ArgumentNullException(nameof(modInfoProviders));
	}

	public async Task<List<ModInfoViewModel>> LoadModsAsync(params DirectoryInfo[] directories)
	{
		try
		{
			var modInfos = await LoadModInfosAsync(directories);
			var viewModels = modInfos.Select(m => new ModInfoViewModel(m)).ToList();

			await _settingsStore.ApplySettingsAsync(viewModels);

			var pandora = viewModels.FirstOrDefault(ModUtils.IsPandora)
				?? throw new InvalidOperationException("FATAL ERROR: Pandora Base does not exist.");

			pandora.Active = true;
			pandora.Priority = (uint)viewModels.Count;

			return viewModels.OrderBy(m => m.Priority).ToList();
		}
		catch (Exception ex)
		{
			logger.Error(ex, "Error loading mods.");
			throw;
		}
	}

	public Task SaveActiveModsAsync(IEnumerable<ModInfoViewModel> mods)
	{
		return _settingsStore.SaveActiveModsAsync(mods);
	}

	private async Task<HashSet<IModInfo>> LoadModInfosAsync(IEnumerable<DirectoryInfo> directories)
	{
		var stopwatch = System.Diagnostics.Stopwatch.StartNew();
		var modInfos = new HashSet<IModInfo>();

		var pathsToScan = GetModPaths(directories)
			.DistinctBy(p => p.path, StringComparer.OrdinalIgnoreCase)
			.ToList();

		var searchTasks = pathsToScan.Select(async p =>
		{
			var (path, provider) = p;
			try
			{
				return await provider.GetInstalledMods(path);
			}
			catch (Exception ex)
			{
				logger.Warn(ex, $"Error loading mods from directory {path}.");
				return null;
			}
		});

		var results = await Task.WhenAll(searchTasks);

		foreach (var mods in results)
		{
			if (mods is not null && mods.Count > 0)
			{
				modInfos.UnionWith(mods);
			}
		}
		return modInfos;
	}

	private IEnumerable<(string path, IModInfoProvider provider)> GetModPaths(IEnumerable<DirectoryInfo> directories)
	{
		foreach (var dir in directories.Where(d => d.Exists))
		{
			foreach (var provider in _modInfoProviders)
			{
				var fullPath = Path.Combine(dir.FullName, provider.SingleRelativePath);
				if (Directory.Exists(fullPath))
				{
					yield return (fullPath, provider);
				}
			}
		}
	}
}

public record ModSaveEntry
{
	public uint Priority { get; init; }
	public bool Active { get; init; }
}