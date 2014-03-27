using System;
using System.Windows;
using System.Windows.Data;

namespace ToastinetWPF.Converters
{
    public class BoolVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Convert a boolean into a System.Windows.Visibility. Visible when the original value is true.
        /// To reverse the behavior, set "false" as ConverterParameter
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool mode = (parameter as string) != "false";
            var val = (bool)value;

            if (mode == val) return Visibility.Visible;
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
