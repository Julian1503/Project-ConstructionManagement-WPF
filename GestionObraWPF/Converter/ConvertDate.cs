using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace GestionObraWPF.Converter
{
    public class ConvertDate : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var valor = ((DateTime)value);

            return $"{valor.Day.ToString("00")}/{valor.Month.ToString("00")}/{valor.Year.ToString("00")}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
