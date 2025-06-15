using Avalonia.Controls;
using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace Pandora.Converters;

public class GridLinesVisibilityToBoolConverter : IValueConverter
{
	public static readonly GridLinesVisibilityToBoolConverter Instance = new();

	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value is DataGridGridLinesVisibility visibility)
		{
			return visibility != DataGridGridLinesVisibility.None;
		}

		return false;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotSupportedException();
	}
}
