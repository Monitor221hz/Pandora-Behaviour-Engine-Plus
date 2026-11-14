// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using System;
using System.Globalization;
using Avalonia.Controls;
using Avalonia.Data.Converters;

namespace Pandora.Converters;

public class BoolToMenuToggleConverter : IValueConverter
{
	public static readonly BoolToMenuToggleConverter Instance = new();

	public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		return value is true ? MenuItemToggleType.Radio : MenuItemToggleType.None;
	}

	public object ConvertBack(
		object? value,
		Type targetType,
		object? parameter,
		CultureInfo culture
	)
	{
		throw new NotSupportedException();
	}
}
