// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using Pandora.API.Patch;
using Pandora.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pandora.Utils.Extensions;

public static class ModExtensions
{
	extension (ModInfoViewModel	m)
	{
		public bool IsPandora => m.Code == "pandora";
	}

	extension (IEnumerable<ModInfoViewModel> mods)
	{
		public void RecalculatePriorities()
		{
			uint priority = 1;
			foreach (var mod in mods)
			{
				mod.Priority = priority++;
			}
		}

		public void SetModsActiveState(bool isActive)
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

		public ModInfoViewModel? EnsurePandoraActive()
		{
			var pandora = mods.FirstOrDefault(m => m.IsPandora);
			pandora?.Active = true;
			return pandora;
		}

		public List<IModInfo> GetSortedActiveMods() => mods
			.Where(m => m.Active)
			.OrderBy(m => m.Priority)
			.ThenBy(m => !m.IsPandora)
			.Select(m => m.ModInfo)
			.ToList();
	}
}
