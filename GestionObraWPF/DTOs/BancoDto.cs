using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionObraWPF.DTOs
{
   public class BancoDto : BaseDto
    {
        public string Descripcion { get; set; } = "";
        public string RazonSocial { get; set; } = "";
        public string NombreFantasia { get; set; } = "";
        public string CBU { get; set; } = "";
        public string Cuit { get; set; } = "";
        public string Telefono { get; set; } = "";
        public string Mail { get; set; } = "";
        public string Sucursal { get; set; } = "";
        public string Path { get;  set; } = "";

        public override bool Equals(object obj)
        {
            if (obj is BancoDto)
            {
                BancoDto banco = obj as BancoDto;
                return banco.CBU == CBU && banco.Id == Id && banco.Descripcion == Descripcion && banco.EstaEliminado == EstaEliminado && banco.RazonSocial == RazonSocial && banco.NombreFantasia == NombreFantasia && banco.Cuit == Cuit && banco.Telefono == Telefono && banco.Mail == Mail && banco.Sucursal == Sucursal && banco.Path== Path;
            }
            else
            {
                return false;
            }
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode() & EstaEliminado.GetHashCode() & Sucursal.GetHashCode() & RazonSocial.GetHashCode() & NombreFantasia.GetHashCode() & Cuit.GetHashCode() &  Path.GetHashCode();
        }
    }
}
