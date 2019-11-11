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
   public class IngresoMaterialDto : BaseDto
    {
        [Exportable(Position = 0, HeaderName = "Fecha de ingreso", TypeValue = FieldValueType.Date)]
        public DateTime FechaIngreso { get; set; } = DateTime.Now;
        [Exportable(IsIgnored =true)]
        public long MaterialId { get; set; }
        [Exportable(IsIgnored =true)]
        public MaterialDto Material { get; set; }
        [Exportable(Position = 1, HeaderName = "Utilitario", TypeValue = FieldValueType.Text)]
        public string MaterialDescripcion => Material.Descripcion;
        [Exportable(IsIgnored =true)]
        public long ObraId { get; set; }
        [Exportable(IsIgnored =true)]
        public ObraDto Obra { get; set; }
        [Exportable(IsIgnored =true)]
        public long EncargadoId { get; set; }
        [Exportable(IsIgnored =true)]
        public EmpleadoDto Encargado { get; set; }
        [Exportable(Position = 2, HeaderName = "Encargado", TypeValue = FieldValueType.Text)]
        public string EncargadoStr => Encargado.ApYNom;
        [Exportable(Position = 3, HeaderName = "Cantidad", TypeValue = FieldValueType.Numeric)]
        public int Cantidad { get; set; } 
        [Exportable(Position = 4, HeaderName = "Cantidad devuelta", TypeValue = FieldValueType.Numeric)]
        public int CantidadDevuelta { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is IngresoMaterialDto)
            {
                IngresoMaterialDto ingresoMaterial = obj as IngresoMaterialDto;
                return ingresoMaterial.Id == Id && ingresoMaterial.MaterialId == MaterialId &&
                    ingresoMaterial.ObraId == ObraId && ingresoMaterial.EncargadoId == EncargadoId && ingresoMaterial.CantidadDevuelta == CantidadDevuelta
                    && ingresoMaterial.Cantidad == Cantidad && ingresoMaterial.FechaIngreso == FechaIngreso && ingresoMaterial.EstaEliminado == EstaEliminado;
            }
            else
            {
                return false;
            }
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode() & FechaIngreso.GetHashCode() & EstaEliminado.GetHashCode() & MaterialId.GetHashCode() & ObraId.GetHashCode() & EncargadoId.GetHashCode() & Cantidad.GetHashCode() & CantidadDevuelta.GetHashCode();
        }
    }
}
