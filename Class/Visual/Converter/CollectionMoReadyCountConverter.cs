using FilesBoxing.Interface.Visual;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace FilesBoxing.Class.Visual.Converter
{
    internal class CollectionMoReadyCountConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return 0;
            var collection = (IEnumerable<IMoProcessInfo>)value;
            return collection.Count(x => x.CountFiles != null && x.IsPackageFileCreated != null);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
