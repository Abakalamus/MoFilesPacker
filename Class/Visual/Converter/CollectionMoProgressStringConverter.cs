using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using FilesBoxing.Interface.Visual;

namespace FilesBoxing.Class.Visual.Converter
{
    internal class CollectionMoProgressStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return string.Empty;
            var collection = (IEnumerable<IMoProcessInfo>)value;
            var readyCount = collection.Count(x => x.CountFiles != null && x.IsPackageFileCreated != null);
            return $"{readyCount}/{collection.Count()}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}