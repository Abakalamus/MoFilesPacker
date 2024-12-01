using FilesBoxing.Interface.Visual;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace FilesBoxing.Class.Visual.Converter
{
    internal class CollectionMoSelectedCountConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return 0;
            var collection = (IList<IMoProcessInfo>)value;
            return collection.All(x => !x.IsSelected) ? 1 : collection.Count(x => x.IsSelected);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}