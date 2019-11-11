using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionObraWPF.DTOs
{
    public class ZonaDto : BaseDto
    {
        public string Descripcion { get; set; } = "";

        public override bool Equals(object obj)
        {
            if (obj is ZonaDto)
            {
                ZonaDto zona = obj as ZonaDto;
                return zona.Id == Id && zona.Descripcion == Descripcion && zona.EstaEliminado == EstaEliminado;
            }
            return false;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode() & Descripcion.GetHashCode() & EstaEliminado.GetHashCode();
        }
    }
}
