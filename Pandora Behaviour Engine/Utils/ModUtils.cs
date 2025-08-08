using Avalonia.Controls;
using Pandora.API.Patch;
using Pandora.Data;
using Pandora.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Pandora.Utils;

public static class ModUtils
{
	private const string PandoraCode = "pandora";

	public static bool IsPandoraMod(string? code) => string.Equals(code, PandoraCode, StringComparison.OrdinalIgnoreCase);
	public static bool IsPandoraMod(IModInfo mod) => IsPandoraMod(mod.Code);
	public static bool IsPandoraMod(ModInfoViewModel mod) => IsPandoraMod(mod.Code);

	public static Func<ModInfoViewModel, bool> BuildNameFilter(string searchText) =>
		mod => string.IsNullOrWhiteSpace(searchText) || mod.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase);

	public static List<IModInfo> GetSortedActiveMods(IEnumerable<ModInfoViewModel> sourceMods) =>
		sourceMods
			.Where(m => m.Active)
			.OrderBy(m => m.Priority)
			.ThenBy(m => IsPandoraMod(m.Code))
			.Select(m => m.ModInfo)
			.ToList();

	public static void NormalizeModPriorities(IEnumerable<ModInfoViewModel> mods)
	{
		uint priority = 1;

		foreach (var mod in mods
			.Where(mod => !IsPandoraMod(mod))
			.OrderBy(mod => mod.Priority)
			.ThenBy(mod => mod.Name, StringComparer.OrdinalIgnoreCase))
		{
			mod.Priority = priority++;
		}
	}

	public static void SetAlphanumericPriorities(IEnumerable<ModInfoViewModel> mods)
	{
		uint priority = 1;

		foreach (var mod in mods
			.Where(m => !IsPandoraMod(m))
			.OrderBy(m => m.Name, StringComparer.OrdinalIgnoreCase))
		{
			mod.Active = true;
			mod.Priority = priority++;
		}

		EnsurePandoraModActive(mods, priority);
	}

	public static void SetModsActiveState(IEnumerable<ModInfoViewModel> mods, bool isActive)
	{
		foreach (var mod in mods.Where(mod => !IsPandoraMod(mod)))
		{
			mod.Active = isActive;
		}
	}

	public static bool? AreAllNonPandoraModsSelected(IReadOnlyCollection<ModInfoViewModel> mods)
	{
		if (mods.Count == 0) return false;

		var nonPandoraMods = mods.Where(m => !IsPandoraMod(m)).ToList();
		int activeCount = nonPandoraMods.Count(m => m.Active);

		return activeCount switch
		{
			0 => false,
			var count when count == nonPandoraMods.Count => true,
			_ => null
		};
	}

	public static ModInfoViewModel? EnsurePandoraModActive(IEnumerable<ModInfoViewModel> mods, uint? priority = null)
	{
		var pandora = mods.FirstOrDefault(IsPandoraMod);
		if (pandora is null)
			return null;

		pandora.Active = true;

		if (priority.HasValue)
			pandora.Priority = priority.Value;

		return pandora;
	}

	public static IEnumerable<(string path, IModInfoProvider provider)> ResolvePaths(IEnumerable<DirectoryInfo> baseDirs, IEnumerable<IModInfoProvider> providers)
	{
		foreach (var dir in baseDirs)
		{
			foreach (var provider in providers)
			{
				var fullPath = Path.Combine(dir.FullName, provider.SingleRelativePath);
				if (Directory.Exists(fullPath))
					yield return (fullPath, provider);
			}
		}
	}

	public static void ApplySortToColumn(DataGridColumnHeader? header, Action<DataGridColumn> sortAction)
	{
		if (header == null) return;

		if (DataGridColumn.GetColumnContainingElement(header) is { } column)
		{
			sortAction(column);
		}
	}
}
