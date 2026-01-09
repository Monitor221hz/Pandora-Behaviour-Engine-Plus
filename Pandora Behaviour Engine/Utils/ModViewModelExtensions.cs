// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using Pandora.API.Patch;
using Pandora.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pandora.Utils;

public static class ModViewModelExtensions
{
	public static Func<ModInfoViewModel, bool> BuildNameFilter(string searchText)
	{
		if (string.IsNullOrWhiteSpace(searchText))
			return _ => true;

		return mod => mod.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase);
	}
	extension(ModInfoViewModel vm)
	{
		public bool IsPandora => vm.Code.IsPandora();
	}

	extension(IEnumerable<ModInfoViewModel> mods)
	{
		public void RecalculatePriorities()
		{
			uint priority = 1;
			foreach (var mod in mods)
			{
				mod.Priority = priority++;
			}
		}

		public void SetAllActive(bool isActive)
		{
			foreach (var mod in mods.Where(mod => !mod.IsPandora))
			{
				mod.Active = isActive;
			}
		}

		public void ResetToAlphanumeric()
		{
			foreach (var mod in mods.Where(m => !m.IsPandora))
			{
				mod.Active = true;
			}

			var sorted = mods
				.OrderBy(m => m.IsPandora)
				.ThenBy(m => m.Name, StringComparer.OrdinalIgnoreCase);

			sorted.RecalculatePriorities();

			mods.EnsurePandoraActive();
		}

		public void NormalizePriorities()
		{
			var sorted = mods
				.OrderBy(m => m.IsPandora)
				.ThenBy(m => m.Priority)
				.ThenBy(m => m.Name, StringComparer.OrdinalIgnoreCase);

			sorted.RecalculatePriorities();

			mods.EnsurePandoraActive();
		}

		public bool TryMoveAndRecalculate(ModInfoViewModel itemToMove, int direction)
		{
			var proxyList = mods.ToList();

			if (proxyList.Count < 2) return false;

			if (itemToMove.IsPandora) return false;

			int oldIndex = proxyList.IndexOf(itemToMove);
			if (oldIndex < 0) return false;

			int newIndex = oldIndex + direction;

			if (newIndex < 0 || newIndex >= proxyList.Count) return false;

			if (proxyList[newIndex].IsPandora) return false;

			proxyList.RemoveAt(oldIndex);
			proxyList.Insert(newIndex, itemToMove);

			proxyList.RecalculatePriorities();

			return true;
		}

		public List<IModInfo> GetSortedActiveMods()
		{
			return mods
				.Where(m => m.Active)
				.OrderBy(m => m.Priority)
				.ThenBy(m => !m.IsPandora)
				.Select(m => m.ModInfo)
				.ToList();
		}
		
		private ModInfoViewModel? EnsurePandoraActive()
		{
			var pandora = mods.FirstOrDefault(m => m.IsPandora);
			pandora?.Active = true;
			return pandora;
		}
	}

	public static bool? AreAllNonPandoraModsSelected(IReadOnlyCollection<ModInfoViewModel> mods)
	{
		if (mods.Count == 0)
			return false;

		var nonPandoraMods = mods.Where(m => !m.IsPandora).ToList();
		int activeCount = nonPandoraMods.Count(m => m.Active);

		return activeCount switch
		{
			0 => false,
			var count when count == nonPandoraMods.Count => true,
			_ => null,
		};
	}
}
