// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using System.Collections.Generic;

namespace Pandora.Models.Extensions;

public static class ListExtensions
{
	public static IEnumerable<T> FastReverse<T>(this IList<T> items)
	{
		for (int i = items.Count - 1; i >= 0; i--)
		{
			yield return items[i];
		}
	}
}
