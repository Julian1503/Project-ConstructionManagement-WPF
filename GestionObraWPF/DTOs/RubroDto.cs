using GestionObraWPF.Constantes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionObraWPF.DTOs
{
    public class RubroDto : BaseDto
    {
        public string Descripcion { get; set; } = "";
        public TipoRubro TipoRubro { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is RubroDto)
            {
                RubroDto rubro = obj as RubroDto;
                return rubro.Id == Id && rubro.Descripcion == Descripcion && rubro.EstaEliminado == EstaEliminado;
            }
            return false;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode() & Descripcion.GetHashCode() & EstaEliminado.GetHashCode() & TipoRubro.GetHashCode();
        }
    }
}
