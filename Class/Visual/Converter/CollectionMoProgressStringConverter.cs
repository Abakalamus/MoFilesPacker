using FilesBoxing.Interface.Visual;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace FilesBoxing.Class.Visual.Converter
{
    internal class CollectionMoProgressStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return string.Empty;
            var collection = (IList<IMoProcessInfo>)value;
            var readyCount = collection.Count(x => x.CountFiles != null && x.IsPackageFileCreated != null);
            return $"{readyCount}/{collection.Count}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}