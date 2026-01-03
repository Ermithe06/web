using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace Maui.Charting.Converters
{
    public class BoolToColorConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            bool isToday = value is bool b && b;
            return isToday ? Colors.LightGreen : Colors.Transparent;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            // We don't convert the color back to a bool in this app
            return null;
        }
    }
}