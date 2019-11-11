using GestionObraWPF.Constantes;
using GestionObraWPF.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionObraWPF.Model
{
public class MaterialDto : BaseDto
    {
        public string Codigo { get; set; } = "";
        public TipoMaterial TipoMaterial { get; set; }
        public string Descripcion { get; set; } = "";
        public string Detalle { get; set; } = "";
        public string Path { get; set; } = "";

        public override bool Equals(object obj)
        {
            if (obj is MaterialDto)
            {
                MaterialDto material = obj as MaterialDto;
                return material.Id == Id && material.Descripcion == Descripcion && material.EstaEliminado == EstaEliminado && material.Codigo == Codigo && material.TipoMaterial == TipoMaterial;
            }
            return false;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode() & Descripcion.GetHashCode() & EstaEliminado.GetHashCode() & Codigo.GetHashCode() & TipoMaterial.GetHashCode();
        }
    }
}
