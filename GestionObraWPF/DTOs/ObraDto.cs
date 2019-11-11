using GestionObraWPF.Constantes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionObraWPF.DTOs
{
    public class ObraDto : BaseDto
    {
        public string Descripcion { get; set; } = "";
        public string Codigo { get; set; } = "";
        public string Observacion { get; set; } = "";
        public DateTime FechaEstimadaInicio { get; set; } = DateTime.Now;
        public string FechaEstimadaInicioStr => FechaEstimadaInicio.ToShortDateString();
        public EmpresaDto Propietario { get; set; }
        public long? PropietarioId { get; set; }
        public EmpleadoDto Encargado { get; set; } //Persona
        public long EncargadoId { get; set; }
        public ZonaDto Zona { get; set; }
        public EstadoObra EstadoObra { get; set; } = EstadoObra.Finalizada;
        public long? ZonaId { get; set; }
        public string Path { get; internal set; }

        public override bool Equals(object obj)
        {
            if (obj is ObraDto)
            {
                ObraDto obra = obj as ObraDto;
                return obra.Id == Id && obra.Descripcion == Descripcion && obra.PropietarioId == PropietarioId && obra.ZonaId == ZonaId && obra.Path == Path && obra.FechaEstimadaInicio == FechaEstimadaInicio && obra.EncargadoId == EncargadoId && obra.Codigo == Codigo && obra.Observacion == Observacion && obra.EstaEliminado == EstaEliminado;
            }
            return false;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode() & Descripcion.GetHashCode() & EstaEliminado.GetHashCode() & Codigo.GetHashCode()  & FechaEstimadaInicio.GetHashCode() ;
        }
    }
}
