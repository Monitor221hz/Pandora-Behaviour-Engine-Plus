using Avalonia.Controls;
using Avalonia.Data.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Pandora.Converters;

public class FilterConfigurableColumnsConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is IEnumerable<DataGridColumn> columns)
        {
            return columns.Where(c => (c.CanUserReorder || c.CanUserResize) && c.Header?.ToString() != (parameter?.ToString() ?? string.Empty)).ToList();
        }
        return Enumerable.Empty<DataGridColumn>();
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
