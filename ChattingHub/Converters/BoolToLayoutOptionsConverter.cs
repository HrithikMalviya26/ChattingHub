using Microsoft.Maui.Controls;
using System;
using System.Globalization;

namespace ChattingHub.Converters
{
    public class BooleanToLayoutOptionsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isCurrentUser)
            {
                // Align messages to the right if they are from the current user
                return isCurrentUser ? LayoutOptions.EndAndExpand : LayoutOptions.StartAndExpand;
            }
            return LayoutOptions.StartAndExpand; // Default to left alignment
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
