using GestionObraWPF.Constantes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionObraWPF.DTOs
{
    public class DetalleCajaDto
    {
        public CajaDto Caja { get; set; }
        public long CajaId { get; set; }
        public decimal Monto { get; set; }
        public TipoPago TipoPago { get; set; }
        public  TipoMovimiento TipoMovimiento { get; set; }
    }
}
