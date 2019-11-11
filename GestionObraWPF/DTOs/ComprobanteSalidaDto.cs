using GestionObraWPF.Constantes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionObraWPF.DTOs
{
    public class ComprobanteSalidaDto : BaseDto
    {
        public long UsuarioId { get; set; }
        public long SubRubroId { get; set; }
        public SubRubroDto SubRubro { get; set; }
        public UsadoEn Usado { get; set; }
        public decimal Descuento { get; set; }
        public decimal Interes { get; set; }
        public decimal? Cae { get; set; }
        public decimal Iva { get; set; }
        public decimal Percepciones { get; set; }
        public decimal Retenciones { get; set; }
        public decimal Monto { get; set; }
        private string _detalle="";
        public string Detalle
        {
            get { return _detalle; }
            set { SetProperty(ref _detalle, value); }
        }
        public DateTime Fecha { get; set; } = DateTime.Now;
        public decimal Total => ((Monto - Descuento) + Interes) + Retenciones + Iva + Percepciones;
        public int NumeroComprobante { get; set; }
        public TipoComprobanteSalida TipoComprobanteSalida { get; set; }
    }
}
