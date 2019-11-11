using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionObraWPF.DTOs
{
    public class EmpresaDto : BaseDto
    {
        public CondicionIvaDto CondicionIva { get; set; }
        public long? CondicionIvaId { get; set; }
        public string RazonSocial { get; set; } = "";
        public string NombreFantasia { get; set; } = "";
        public string Cuit { get; set; } = "";
        public string Telefono { get; set; } = "";
        public string Mail { get; set; } = "";
        public string Path { get; set; } = "";

        public override bool Equals(object obj)
        {
            if (obj is EmpresaDto)
            {
                EmpresaDto empresa = obj as EmpresaDto;
                return empresa.Id == Id && empresa.CondicionIvaId == CondicionIvaId &&
                    empresa.RazonSocial == RazonSocial && empresa.Telefono == Telefono && empresa.Path == Path &&
                    empresa.NombreFantasia == NombreFantasia && empresa.Mail == Mail && empresa.Cuit == Cuit && empresa.EstaEliminado == EstaEliminado;
            }
            else
            {
                return false;
            }
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode() & CondicionIvaId.GetHashCode() & EstaEliminado.GetHashCode() & RazonSocial.GetHashCode() & NombreFantasia.GetHashCode() & Cuit.GetHashCode() & Telefono.GetHashCode() & Mail.GetHashCode() & Path.GetHashCode();
        }
    }
}
