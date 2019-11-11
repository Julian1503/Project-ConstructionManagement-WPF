using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionObraWPF.Helpers
{
    public static class CalcularPorcentaje
    {
        public static decimal Descuento(decimal porcentaje, decimal total)
        {
            return total - (total * porcentaje / 100m);
        }
        public static decimal Recargo(decimal porcentaje, decimal total)
        {
            return total * (1 + porcentaje / 100); 
        }
    }
}
