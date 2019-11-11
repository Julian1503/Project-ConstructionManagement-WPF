using GestionObraWPF.Constantes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;


namespace GestionObraWPF.Converter
{
    public class EnumToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var valor = ((EstadoObra)value);
            if (valor == EstadoObra.EnProceso)
            {
                return $"En proceso";
            }
            else
            {
                return $"Finalizada";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
