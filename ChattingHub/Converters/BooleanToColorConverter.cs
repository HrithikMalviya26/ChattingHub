using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace ChattingHub.Converters
{
    public class BooleanToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isCurrentUser)
            {
                // Return different colors based on whether it's the current user's message
                return isCurrentUser ? Color.FromHex("#0078D4") : Color.FromHex("#FF00FF"); // Blue for sender, light gray for receiver
            }
            return Color.FromHex("#E5E5EA"); // Default color for unrecognized values
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException(); // Not needed for this converter
        }
    }
}
