using GestionObraWPF.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionObraWPF.DTOs
{
    public class DetalleComprobanteDto : BaseDto
    {
        private int _cantidad;
        public long ComprobanteId { get; set; }
        public long MaterialId { get; set; }
        public MaterialDto Material { get; set; }
        public ComprobanteCompraDto Comprobante { get; set; }
        public string Codigo { get; set; } = "";
        public string Descripcion { get; set; } = "";
        public decimal PrecioUnitario { get; set; }
        public int Cantidad { get { return _cantidad; } set { SetProperty(ref _cantidad, value); } }
        public decimal SubTotal => Cantidad * PrecioUnitario;

    }
}
