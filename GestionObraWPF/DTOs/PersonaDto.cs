using GestionObraWPF.Constantes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionObraWPF.DTOs
{
    public class PersonaDto : BaseDto
    {
        public string Nombre { get; set; } = "";
        public string Apellido { get; set; } = "";
        public string ApYNom => $"{Apellido} {Nombre}";
        public string Dni { get; set; } = "";
        public DateTime FechaNacimiento { get; set; }
        public string Telefono { get; set; } = "";
        public string Celular { get; set; } = "";
        public string Email { get; set; } = "";
        public TipoSexo Sexo { get; set; }
        public PersonaDto()
        {
            FechaNacimiento = DateTime.Now;
        }

        public override bool Equals(object obj)
        {
            if (obj is PersonaDto)
            {
                PersonaDto persona = obj as PersonaDto;
                return persona.Id == Id && persona.ApYNom == ApYNom && persona.Dni == Dni && persona.Email == Email &&
                    persona.FechaNacimiento == FechaNacimiento && persona.Celular == Celular && persona.Sexo == Sexo &&
                    persona.Telefono == Telefono && persona.EstaEliminado == EstaEliminado;
            }
            return false;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode() & ApYNom.GetHashCode() & EstaEliminado.GetHashCode() & Dni.GetHashCode() & Dni.GetHashCode() & FechaNacimiento.GetHashCode() & Telefono.GetHashCode()
                & Celular.GetHashCode() & Email.GetHashCode() & Sexo.GetHashCode();
        }
    }
}
