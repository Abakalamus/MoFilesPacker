using FilesBoxing.Interface.Visual;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace FilesBoxing.Class.Visual.Converter
{
    internal class OnOffCommandNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return "Неопределенное состояние";
            var allMoInfo = (IEnumerable<IMoProcessInfo>)value;
            return allMoInfo.Any(x => x.IsSelected) ? "Отключить все МО" : "Включить все МО";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}