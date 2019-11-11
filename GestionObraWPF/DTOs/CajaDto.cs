using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionObraWPF.DTOs
{
   public class CajaDto :BaseDto
    {
        public DateTime? FechaCierre { get; set; } = DateTime.Now;
        public DateTime FechaApertura { get; set; } = DateTime.Now;
        public decimal MontoApertura { get; set; }
        public decimal MontoCierre { get; set; }
        public long UsuarioAperturaId { get; set; }
        public long? UsuarioCierreId { get; set; }
        public decimal MontoSistema { get; set; }
        public decimal Diferencia { get; set; }
    }
}
