﻿using System;
using System.Windows.Data;

namespace GestionObraWPF.Converter
{
    public class BoolConverterToYesNo : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if ((bool)value) return "Si";
            return "No";
                }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
            /* not sure if you need convert back */
        }
    }
}
