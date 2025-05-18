using System;
using Avalonia.Data.Converters;

namespace Pandora.Converters
{
    public class PandoraCodeToEnabledConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is string code)
                return !string.Equals(code, "pandora", StringComparison.OrdinalIgnoreCase);
            return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
} 