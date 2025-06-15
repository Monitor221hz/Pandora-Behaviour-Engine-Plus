using Avalonia;
using Avalonia.Controls;
using Avalonia.Data.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandora.Converters;

public class GridLinesEnumMatchConverter : IValueConverter
{
	public static readonly GridLinesEnumMatchConverter Instance = new();

	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value is DataGridGridLinesVisibility visibility && parameter is string param)
		{
			return string.Equals(visibility.ToString(), param, StringComparison.OrdinalIgnoreCase);
		}
		return false;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value is bool b && b && parameter is string param &&
			Enum.TryParse<DataGridGridLinesVisibility>(param, out var result))
		{
			return result;
		}
		return AvaloniaProperty.UnsetValue;
	}
}