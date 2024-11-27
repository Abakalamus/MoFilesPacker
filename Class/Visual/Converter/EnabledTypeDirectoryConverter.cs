﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace FilesBoxing.Class.Visual.Converter
{
    public class EnabledTypeDirectoryConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                var idSelectedGroup = System.Convert.ToInt32(values[0]);
                var idCollections = (IEnumerable<int>)values[1];

                return idCollections.Contains(idSelectedGroup);
            }
            catch (Exception)
            {
                return null;
            }

        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        // return new object[] { 1, new List<int>{1} };
        //public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        //{
        //    var idSelectedGroup = System.Convert.ToInt32(value);
        //    var idCollections = (IEnumerable<int>)parameter;

        //    return idCollections.Contains(idSelectedGroup);
        //}

        //public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        //{
        //    throw new NotImplementedException();
        //}
    }
}