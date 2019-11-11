using System;
using Exportable.Attribute;
using Exportable.Models;
using GestionObraWPF.Constantes;

namespace GestionObraWPF.DTOs
{
    public class ComprobanteCompraDto : BaseDto
    {
        [Exportable(Position = 0, HeaderName = "Fecha del comprobante", Format = "dd-MM-yyyy", TypeValue = FieldValueType.Date)]
        public DateTime Fecha { get; set; } = DateTime.Now;
        [Exportable(IsIgnored = true)]
        public int NumeroCompra { get; set; }
        [Exportable(Position = 1, HeaderName = "Numero de comprobante", TypeValue = FieldValueType.Numeric,Format = "#0")]
        public int? NumeroCompronte { get; set; }
        [Exportable(Position = 2, HeaderName = "Cae", TypeValue = FieldValueType.Numeric,Format = "#0")]
        public int? Cae { get; set; }
        [Exportable(Position = 3, HeaderName = "Descripcion",  TypeValue = FieldValueType.Text)]
        public string Descripcion { get; set; }
        [Exportable(Position = 4, HeaderName = "Cuit",  TypeValue = FieldValueType.Text)]
        public string CUIT => Proveedor == null ? cuit : Proveedor.Cuit;
        [Exportable(Position = 5, HeaderName = "Proveedor", TypeValue = FieldValueType.Text)]
        public string ProveedorDescripcion => Proveedor == null ? " " : Proveedor.RazonSocial;
        [Exportable(Position = 6, HeaderName = "Proyecto/Cuentas particulares", TypeValue = FieldValueType.Text)]
        public string DescripcionObra => Obra == null ? " " : $"{Obra.Codigo} - {Obra.Descripcion}";
        [Exportable(Position = 7, HeaderName = "Subtotal",  TypeValue = FieldValueType.Numeric, Format = "#0")]
        public decimal Monto { get; set; }
        [Exportable(Position = 8, HeaderName = "Descuento",  TypeValue = FieldValueType.Numeric, Format = "#0")]
        public decimal Descuento { get; set; }
        [Exportable(Position = 9, HeaderName = "Recargos",  TypeValue = FieldValueType.Numeric, Format = "#0")]
        public decimal Recargos { get; set; }
        [Exportable(Position = 10, HeaderName = "Iva",  TypeValue = FieldValueType.Numeric, Format = "#0")]
        public decimal Iva { get; set; }
        [Exportable(Position = 11, HeaderName = "Percepciones",  TypeValue = FieldValueType.Numeric, Format = "#0")]
        public decimal Percepciones { get; set; }
        [Exportable(Position = 12, HeaderName = "Retenciones",  TypeValue = FieldValueType.Numeric, Format = "#0")]
        public decimal Retenciones { get; set; }
        [Exportable(IsIgnored = true)]
        public long? ProveedorId { get; set; }
        [Exportable(IsIgnored = true)]
        public long? ObraId { get; set; }
        public string cuit { get; set; } = "";
        [Exportable(Position = 13, HeaderName = "Total a pagar",  TypeValue = FieldValueType.Numeric, Format = "#0")]
        public decimal Total => ((Monto - Descuento) + Recargos) + Retenciones + Iva + Percepciones;
        public decimal TotalSinImpuestos => ((Monto - Descuento) + Recargos);
        public TipoPago FormaPago { get; set; }
        [Exportable(Position = 14, HeaderName = "Dinero pagado",  TypeValue = FieldValueType.Numeric, Format = "#0")]
        public decimal Pagando
        {
            get { return _pagando; }
            set { SetProperty(ref _pagando, value); }
        }
        private decimal _pagando;
        [Exportable(Position = 15, HeaderName = "Pagado", TypeValue = FieldValueType.Bool)]
        public bool Pagado { get; set; }


        [Exportable(IsIgnored =true)]
        public virtual ObraDto Obra { get; set; }
        [Exportable(IsIgnored =true)]
        public virtual ProveedorDto Proveedor { get; set; }
    }
}
