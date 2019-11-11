using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionObraWPF.DTOs
{
    public class EmpleadoDto: BaseDto
    {
        public long Legajo { get; set; }
        public DateTime FechaIncio { get; set; } = DateTime.Now;
        public long CategoriaId { get; set; }
        public CategoriaDto Categoria { get; set; }
        public string Nombre { get; set; } = "";
        public string Apellido { get; set; } = "";
        public string CUIT { get; set; } = "";
        public string ApYNom => $"{Apellido} {Nombre}";
        public string Telefono { get; set; } = "";
        public string Celular { get; set; } = "";
        public DateTime FechaNacimiento { get; set; } = DateTime.Now;
        public string Dni { get; set; } = "";

        public override bool Equals(object obj)
        {
            if (obj is EmpleadoDto)
            {
                EmpleadoDto persona = obj as EmpleadoDto;
                return persona.Id == Id && persona.ApYNom == ApYNom && persona.Dni == Dni && persona.Legajo == Legajo &&
                    persona.FechaNacimiento == FechaNacimiento && persona.Celular == Celular && persona.FechaIncio == FechaIncio &&
                    persona.Telefono == Telefono && persona.CategoriaId == CategoriaId && persona.CUIT == CUIT && persona.EstaEliminado == EstaEliminado;
            }
            return false;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode() & ApYNom.GetHashCode() & EstaEliminado.GetHashCode() & Dni.GetHashCode() & Dni.GetHashCode() & FechaNacimiento.GetHashCode() & Telefono.GetHashCode()
                & Celular.GetHashCode() & Legajo.GetHashCode() & FechaIncio.GetHashCode() & CategoriaId.GetHashCode() & CUIT.GetHashCode();
        }
    }
}
