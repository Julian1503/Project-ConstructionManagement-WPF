using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace GestionObraWPF.Converter
{
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class ConvertBooleanToVisibilityCollapsed : IValueConverter

    {

        public object Convert(object value, Type targetType,

            object parameter, CultureInfo culture)

        {

            bool? res = value as bool?;

            if (res != null && res.HasValue)

            {

                if (res.Value)

                    return Visibility.Visible;

                else

                    return Visibility.Collapsed;

            }

            return value;

        }

        public object ConvertBack(object value, Type targetType,

            object parameter, CultureInfo culture)

        {

            Visibility res = (Visibility) value;

            return res == Visibility.Visible ? true : false;

        }

    }
}
