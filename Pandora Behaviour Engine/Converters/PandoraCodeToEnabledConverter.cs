using Avalonia.Data.Converters;
using System;
using System.Globalization;

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
 