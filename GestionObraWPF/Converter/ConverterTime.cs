using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace GestionObraWPF.Converter
{
    public class ConverterTime : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var valor = ((TimeSpan)value);
            if (valor.Hours != 0)
            {
                return $"{valor.Hours.ToString("00")}:{valor.Minutes.ToString("00")}";
            }
            else
            {
                if (valor.Minutes != 0)
                {
                    return $"{valor.Minutes.ToString("00")}";
                }
                else
                {
                    return "0";
                }
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
