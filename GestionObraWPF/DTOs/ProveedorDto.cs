using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionObraWPF.DTOs
{
    public class ProveedorDto : BaseDto
    {
        public string RazonSocial { get; set; } = "";
        public string NombreFantasia { get;  set; } = "";
        public string Telefono { get; set; } = "";
        public string Email { get; set; } = "";
        public string Contacto { get; set; } = "";
        public long? CondicionIvaId { get; set; }
        public string Cuit { get; set; }
        public CondicionIvaDto CondicionIva { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is ProveedorDto)
            {
                ProveedorDto zona = obj as ProveedorDto;
                return zona.Id == Id && zona.RazonSocial == RazonSocial && zona.Telefono == Telefono && zona.Email == Email && zona.Contacto == Contacto && zona.NombreFantasia == NombreFantasia && zona.EstaEliminado == EstaEliminado;
            }
            return false;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode() & RazonSocial.GetHashCode() & Telefono.GetHashCode() & Email.GetHashCode() & Contacto.GetHashCode() & NombreFantasia.GetHashCode() & EstaEliminado.GetHashCode();
        }
    }
}
