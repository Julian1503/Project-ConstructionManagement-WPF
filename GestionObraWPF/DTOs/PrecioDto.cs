using GestionObraWPF.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionObraWPF.DTOs
{
    public class PrecioDto : BaseDto
    {
        public UsuarioDto Usuario { get; set; }
        public long UsuarioId { get; set; }
        public DateTime FechaActualizacion { get; set; } = DateTime.Now;
        public decimal PrecioCompra { get; set; }
        public long MaterialId { get; set; }
        public MaterialDto Material { get; set; }
        public override bool Equals(object obj)
        {
            if (obj is PrecioDto)
            {
                PrecioDto precio = obj as PrecioDto;
                return precio.Id == Id && precio.MaterialId == MaterialId && precio.FechaActualizacion == FechaActualizacion && precio.PrecioCompra == PrecioCompra && precio.UsuarioId == UsuarioId && precio.EstaEliminado == EstaEliminado;
            }
            return false;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode() & UsuarioId.GetHashCode() & EstaEliminado.GetHashCode() & FechaActualizacion.GetHashCode() & PrecioCompra.GetHashCode() & MaterialId.GetHashCode();
        }
    }
}
