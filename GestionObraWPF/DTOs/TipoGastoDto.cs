using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionObraWPF.DTOs
{
    public class TipoGastoDto : BaseDto
    {
        public string Descripcion { get; set; } = "";

        public override bool Equals(object obj)
        {

            if (obj is TipoGastoDto)
            {
                TipoGastoDto tipoGasto = obj as TipoGastoDto;
                return tipoGasto.Id == Id && tipoGasto.Descripcion == Descripcion && tipoGasto.EstaEliminado == EstaEliminado;
            }
            return false;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode() & Descripcion.GetHashCode() & EstaEliminado.GetHashCode();
        }
    }
}
