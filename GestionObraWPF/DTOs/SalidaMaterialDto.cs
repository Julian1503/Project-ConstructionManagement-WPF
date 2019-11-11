using GestionObraWPF.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionObraWPF.DTOs
{
    public class SalidaMaterialDto : BaseDto
    {
        public DateTime FechaEgreso { get; set; } = DateTime.Now;
        public long MaterialId { get; set; }
        public long DeObraId { get; set; }
        public long EncargadoId { get; set; }
        public EmpleadoDto Encargado { get; set; }
        public int Cantidad { get; set; }
        public long ParaObraId { get; set; }

        public  ObraDto ParaObra { get; set; }
        public ObraDto DeObra { get; set; }
        public  MaterialDto Material { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is SalidaMaterialDto)
            {
                SalidaMaterialDto salidaMaterial = obj as SalidaMaterialDto;
                return salidaMaterial.Id == Id && salidaMaterial.FechaEgreso == FechaEgreso && salidaMaterial.MaterialId == MaterialId && salidaMaterial.DeObraId == DeObraId
                    && salidaMaterial.ParaObraId == ParaObraId && salidaMaterial.EncargadoId == EncargadoId && salidaMaterial.Cantidad == Cantidad && salidaMaterial.EstaEliminado == EstaEliminado;
            }
            return false;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode() & FechaEgreso.GetHashCode() & MaterialId.GetHashCode() & DeObraId.GetHashCode() & ParaObraId.GetHashCode() & EncargadoId.GetHashCode() & Cantidad.GetHashCode() & EstaEliminado.GetHashCode();
        }
    }
}
