using Exportable.Attribute;
using Exportable.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionObraWPF.DTOs
{
    public class JornalDto : BaseDto
    {
        [Exportable(IsIgnored =true)]
        public long ObraId { get; set; }
        [Exportable(IsIgnored =true)]
        public ObraDto Obra { get; set; }
        [Exportable(Position = 0, HeaderName = "Fecha del jornal", Format ="dd-MM-yyyy", TypeValue = FieldValueType.Date)]
        public DateTime DiaJornal { get; set; } = DateTime.Now;
        [Exportable(Position = 1, HeaderName = "Obra",  TypeValue = FieldValueType.Text)]
        public string DescripcionObra => Obra == null ? " " : $"{Obra.Codigo} - {Obra.Descripcion}";
        [Exportable(Position = 2, HeaderName = "Viatico", TypeValue = FieldValueType.Numeric, Format = "#0")]
        public decimal Viatico { get; set; }
        [Exportable(Position = 3, HeaderName = "Gasolina", TypeValue = FieldValueType.Numeric, Format = "#0")]
        public decimal Gasolina { get; set; }
        [Exportable(Position = 4, HeaderName = "Multas", TypeValue = FieldValueType.Numeric, Format = "#0")]
        public decimal Multas { get; set; }
        [Exportable(Position = 5, HeaderName = "Repuestos", TypeValue = FieldValueType.Numeric, Format = "#0")]
        public decimal Repuestos { get; set; }
        [Exportable(Position = 6, HeaderName = "Otros", TypeValue = FieldValueType.Numeric, Format = "#0")]
        public decimal Otros { get; set; }
        [Exportable(IsIgnored = true)]
        public int NumeroOrden { get;  set; }
    }
}
