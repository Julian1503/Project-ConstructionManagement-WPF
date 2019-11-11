using Exportable.Attribute;
using Exportable.Models;
using GestionObraWPF.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionObraWPF.DTOs
{
    public  class JornalMaterialDto : BaseDto
    {
        [Exportable(IsIgnored =true)]
        public JornalDto Jornal { get; set; }
        [Exportable(IsIgnored =true)]
        public long JornalId { get; set; }
        [Exportable(IsIgnored =true)]
        public long MaterialId { get; set; }
        [Exportable(IsIgnored =true)]
        public MaterialDto Material { get; set; }
        [Exportable(Position = 0, HeaderName = "Fecha de uso", TypeValue = FieldValueType.Text)]
        public string JornalFecha => Jornal.DiaJornal.ToShortDateString();
        [Exportable(Position = 1, HeaderName = "Material", TypeValue = FieldValueType.Text)]
        public string MaterialDescripcion => $"{Material.Codigo} - {Material.Descripcion}";
        [Exportable(Position = 2, HeaderName = "Cantidad usada", TypeValue = FieldValueType.Text)]
        public int CantidadUsado { get; set; }
    }
}
