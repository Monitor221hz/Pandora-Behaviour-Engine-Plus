// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using Avalonia.Controls;
using System;

namespace Pandora.Utils.Extensions;

public static class DataGridColumnExtensions
{
	extension (DataGridColumnHeader? header)
	{
		public void ApplySortToColumn(Action<DataGridColumn> sortAction)
		{
			if (header == null)
				return;

			if (DataGridColumn.GetColumnContainingElement(header) is { } column)
			{
				sortAction(column);
			}
		}
	}

}
