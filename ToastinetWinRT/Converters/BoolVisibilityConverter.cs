using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace ToastinetWinRT.Converters
{
    public class BoolVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool mode = (parameter as string) != "false";
            var val = (bool)value;

            if (mode == val) return Visibility.Visible;
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
