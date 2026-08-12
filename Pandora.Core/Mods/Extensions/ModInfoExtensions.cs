// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using Pandora.API.Patch;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pandora.Core.Mods.Extensions;

public static class ModInfoExtensions
{
	private const string PandoraCode = "pandora";

	public static bool IsPandora(this IModInfo mod) =>
		string.Equals(mod.Code, PandoraCode, StringComparison.OrdinalIgnoreCase);

	public static bool IsPandora(this string? code) =>
		string.Equals(code, PandoraCode, StringComparison.OrdinalIgnoreCase);

	/// <summary>
	/// Assigns sequential priorities (1, 2, 3, ...) to the mods in the order they appear.
	/// </summary>
	public static void RecalculatePriorities(this IEnumerable<IModInfo> mods)
	{
		uint priority = 1;
		foreach (var mod in mods)
		{
			mod.Priority = priority++;
		}
	}

	/// <summary>
	/// Activates or deactivates every non-Pandora mod in the collection.
	/// </summary>
	public static void SetAllActive(this IEnumerable<IModInfo> mods, bool isActive)
	{
		foreach (var mod in mods.Where(mod => !mod.IsPandora()))
		{
			mod.Active = isActive;
		}
	}

	/// <summary>
	/// Activates every non-Pandora mod, sorts alphanumerically by name (Pandora first),
	/// assigns sequential priorities, and ensures the Pandora mod is active.
	/// </summary>
	public static void ResetToAlphanumeric(this IEnumerable<IModInfo> mods)
	{
		var list = mods as IList<IModInfo> ?? mods.ToList();

		list.SetAllActive(true);

		var sorted = list.OrderBy(m => m.IsPandora())
			.ThenBy(m => m.Name, StringComparer.OrdinalIgnoreCase);

		sorted.RecalculatePriorities();

		list.EnsurePandoraActive();
	}

	/// <summary>
	/// Sorts the mods by existing priority (Pandora first, then by Priority, then by Name)
	/// and assigns sequential priorities. Ensures the Pandora mod is active.
	/// </summary>
	public static void NormalizePriorities(this IEnumerable<IModInfo> mods)
	{
		var list = mods as IList<IModInfo> ?? mods.ToList();

		var sorted = list.OrderBy(m => m.IsPandora())
			.ThenBy(m => m.Priority)
			.ThenBy(m => m.Name, StringComparer.OrdinalIgnoreCase);

		sorted.RecalculatePriorities();

		list.EnsurePandoraActive();
	}

	/// <summary>
	/// Moves <paramref name="itemToMove"/> by <paramref name="direction"/> positions (negative for earlier,
	/// positive for later) and recalculates priorities. Returns false if the move is not allowed
	/// (e.g. moving the Pandora mod, moving past either end, or landing on the Pandora mod).
	/// </summary>
	public static bool TryMoveAndRecalculate(
		this IEnumerable<IModInfo> mods,
		IModInfo itemToMove,
		int direction
	)
	{
		var list = mods as IList<IModInfo> ?? mods.ToList();

		if (list.Count < 2)
			return false;

		if (itemToMove.IsPandora())
			return false;

		int oldIndex = list.IndexOf(itemToMove);
		if (oldIndex < 0)
			return false;

		int newIndex = oldIndex + direction;

		if (newIndex < 0 || newIndex >= list.Count)
			return false;

		if (list[newIndex].IsPandora())
			return false;

		list.RemoveAt(oldIndex);
		list.Insert(newIndex, itemToMove);

		list.RecalculatePriorities();

		return true;
	}

	/// <summary>
	/// Returns active mods sorted by priority (Pandora last), as <see cref="IModInfo"/>.
	/// </summary>
	public static List<IModInfo> GetSortedActiveMods(this IEnumerable<IModInfo> mods)
	{
		return mods.Where(m => m.Active)
			.OrderBy(m => m.Priority)
			.ThenBy(m => !m.IsPandora())
			.ToList();
	}

	/// <summary>
	/// Ensures the Pandora mod (if present) is active. Returns the Pandora mod, or null if none.
	/// </summary>
	public static IModInfo? EnsurePandoraActive(this IEnumerable<IModInfo> mods)
	{
		var pandora = mods.FirstOrDefault(m => m.IsPandora());
		if (pandora is not null)
			pandora.Active = true;
		return pandora;
	}

	/// <summary>
	/// Returns:
	/// - <c>false</c> if no non-Pandora mods are active,
	/// - <c>true</c> if all non-Pandora mods are active,
	/// - <c>null</c> if some but not all are active.
	/// </summary>
	public static bool? AreAllNonPandoraModsSelected(this IReadOnlyCollection<IModInfo> mods)
	{
		if (mods.Count == 0)
			return false;

		var nonPandoraMods = mods.Where(m => !m.IsPandora()).ToList();
		int activeCount = nonPandoraMods.Count(m => m.Active);

		return activeCount switch
		{
			0 => false,
			var count when count == nonPandoraMods.Count => true,
			_ => null,
		};
	}

	/// <summary>
	/// Builds a predicate that returns true if a mod's name contains <paramref name="searchText"/>
	/// (case-insensitive). Returns a "match everything" predicate if searchText is blank.
	/// </summary>
	public static Func<IModInfo, bool> BuildNameFilter(string searchText)
	{
		if (string.IsNullOrWhiteSpace(searchText))
			return _ => true;

		return mod => mod.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase);
	}
}
