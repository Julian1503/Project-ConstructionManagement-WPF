using GestionObraWPF.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionObraWPF.DTOs
{
    public class StockDto : BaseDto
    {
        public long UsuarioId { get; set; }
        public DateTime FechaActualizacion { get; set; }
        public int StockActual { get; set; }
        public int StockMinimo { get; set; }
        public long MaterialId { get; set; }
        public UsuarioDto Usuario { get; set; }
        public MaterialDto Material { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is StockDto)
            {
                StockDto stock = obj as StockDto;
                return stock.Id == Id && stock.UsuarioId == UsuarioId && stock.FechaActualizacion == FechaActualizacion && stock.MaterialId == MaterialId
                    && stock.StockActual == StockActual && stock.StockMinimo == StockMinimo && stock.EstaEliminado == EstaEliminado;
            }
            return false;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode() & UsuarioId.GetHashCode() & FechaActualizacion.GetHashCode() & StockActual.GetHashCode() & StockMinimo.GetHashCode() & MaterialId.GetHashCode() & EstaEliminado.GetHashCode();
        }
    }
}
