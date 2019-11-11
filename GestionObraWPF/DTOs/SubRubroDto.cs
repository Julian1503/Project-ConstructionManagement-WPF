using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionObraWPF.DTOs
{
    public class SubRubroDto : BaseDto
    {
        public string Descripcion { get; set; } = "";
        public long RubroId { get; set; }
        public RubroDto Rubro { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is SubRubroDto)
            {
                SubRubroDto subRubro = obj as SubRubroDto;
                return subRubro.Id == Id && subRubro.Descripcion == Descripcion && subRubro.EstaEliminado == EstaEliminado && subRubro.RubroId == RubroId;
            }
            else
            {
                return false;
            }
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode() & Descripcion.GetHashCode() & EstaEliminado.GetHashCode() & RubroId.GetHashCode();
        }
    }
}
