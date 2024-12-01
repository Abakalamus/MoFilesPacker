using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace FilesBoxing.Class.Visual.Converter
{
    internal class ProgressInfoVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var isWorkStarted = (bool)value;
            return isWorkStarted ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}