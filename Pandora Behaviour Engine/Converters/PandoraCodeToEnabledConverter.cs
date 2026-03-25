// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace Pandora.Converters;

public class PandoraCodeToEnabledConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value is string code)
			return !string.Equals(code, "pandora", StringComparison.OrdinalIgnoreCase);
		return true;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
