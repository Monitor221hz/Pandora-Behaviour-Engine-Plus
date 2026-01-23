// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

﻿// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using Avalonia.Controls;
using System;

namespace Pandora.Utils;

public static class DataGridUtils
{
	public static void ApplySort(DataGridColumnHeader? header, Action<DataGridColumn> sortAction)
	{
		if (header != null && DataGridColumn.GetColumnContainingElement(header) is { } column)
		{
			sortAction(column);
		}
	}
}
