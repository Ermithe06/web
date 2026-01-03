using System;
using System.Globalization;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Maui.Charting.Converters
{
    public class TodayAppointmentColorConverter : IValueConverter
    {
        // value is expected to be a bool (IsToday)
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isToday && isToday)
            {
                // Highlight today's appointments
                return Colors.LightGreen;
            }

            // Default background (transparent = inherit page color)
            return Colors.Transparent;
        }

        // Not used, but required by IValueConverter
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

