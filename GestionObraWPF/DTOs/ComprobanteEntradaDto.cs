using GestionObraWPF.Constantes;
using System;

namespace GestionObraWPF.DTOs
{
    public class ComprobanteEntradaDto : BaseDto
    {
        public long UsuarioId { get; set; }
        public long? SubRubroId { get; set; }
        public decimal? Cae { get; set; }
        public SubRubroDto SubRubro { get; set; }
        public UsadoEn Usado { get; set; }
        private decimal _monto;
        public decimal Monto
        {
            get { return _monto; }
            set { SetProperty(ref _monto, value); }
        }
        public decimal Descuento { get; set; }
        public decimal Interes { get; set; }
        public decimal Iva { get; set; }
        private string _detalle ="";
        public string Detalle
        {
            get { return _detalle; }
            set { SetProperty(ref _detalle, value); }
        }
        public DateTime Fecha { get; set; } = DateTime.Now;
        public int? NumeroComprobante { get; set; }
        public decimal Total => ((Monto - Descuento) + Interes);
        public TipoComprobanteEntrada TipoComprobanteEntrada { get; set; } = TipoComprobanteEntrada.A;
    }
}