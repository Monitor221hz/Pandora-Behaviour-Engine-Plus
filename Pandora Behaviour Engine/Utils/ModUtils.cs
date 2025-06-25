using Avalonia.Controls;
using Pandora.API.Patch;
using Pandora.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pandora.Utils;

public static class ModUtils
{
	private const string PandoraCode = "pandora";

	public static List<IModInfo> GetActiveModsByPriority(IEnumerable<ModInfoViewModel> sourceMods) =>
		sourceMods.Where(m => m.Active)
			.Select(m => m.ModInfo)
			.OrderBy(m => m.Priority)
			.ToList();

	public static Func<ModInfoViewModel, bool> BuildFilter(string searchText) =>
		mod => string.IsNullOrEmpty(searchText) || mod.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase);

	public static void ApplySort(DataGridColumnHeader? header, Action<DataGridColumn> sortAction)
	{
		if (header is null) return;

		if (DataGridColumn.GetColumnContainingElement(header) is { } column)
		{
			sortAction(column);
		}
	}

	public static void SetAllModActiveStates(IEnumerable<ModInfoViewModel> mods, bool isActive)
	{
		foreach (var mod in mods)
		{
			if (!IsPandora(mod))
				mod.Active = isActive;
		}
	}

	public static void AssignPrioritiesAlphanumerically(List<ModInfoViewModel> mods)
	{
		var ordered = mods
			.Where(m => !IsPandora(m))
			.OrderBy(m => m.Name, StringComparer.OrdinalIgnoreCase)
			.ToList();

		uint priority = 1;
		foreach (var mod in ordered)
		{
			mod.Active = true;
			mod.Priority = priority++;
		}

		if (mods.FirstOrDefault(IsPandora) is { } pandora)
		{
			pandora.Active = true;
			pandora.Priority = priority;
		}
	}

	public static ModInfoViewModel? ExtractPandoraMod(IEnumerable<ModInfoViewModel> mods)
	{
		var pandora = mods.FirstOrDefault(IsPandora);

		if (pandora is not null)
			pandora.Active = true;

		return pandora;
	}

	public static bool? IsAllSelectedExceptPandora(IReadOnlyCollection<ModInfoViewModel> query)
	{
		if (query.Count == 0) return false;

		var selectedCount = query.Count(x => x.Active) - 1;

		return selectedCount switch
		{
			0 => false,
			var count when count == query.Count - 1 => true,
			_ => null
		};
	}

	public static bool IsPandora(IModInfo mod) =>
		string.Equals(mod.Code, PandoraCode, StringComparison.OrdinalIgnoreCase);

	public static bool IsPandora(ModInfoViewModel mod) =>
		string.Equals(mod.Code, PandoraCode, StringComparison.OrdinalIgnoreCase);
}
