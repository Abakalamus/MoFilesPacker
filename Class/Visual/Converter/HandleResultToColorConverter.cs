using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace FilesBoxing.Class.Visual.Converter
{
    public class HandleResultToColorConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var defaultColor = new SolidColorBrush(Colors.White);
            try
            {
                var countFiles = values[0] == null ? (byte?)null : System.Convert.ToByte(values[0]);
                var isPackaged = values[1] == null ? (bool?)null : System.Convert.ToBoolean(values[1]);
                if (countFiles == null || isPackaged == null)
                    return defaultColor;
                if (countFiles == 0 || isPackaged == false)
                    return new SolidColorBrush(Colors.DarkOrange);
                return new SolidColorBrush(Colors.LightGreen);
            }
            catch (Exception)
            {
                return new SolidColorBrush(Colors.LightPink);
            }
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}