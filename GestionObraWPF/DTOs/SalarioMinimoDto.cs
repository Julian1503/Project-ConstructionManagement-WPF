using System;

namespace GestionObraWPF.DTOs
{
    public class SalarioMinimoDto : BaseDto
    {
        public DateTime FechaActualizacion { get; set; } = DateTime.Now;
        public decimal Salario { get; set; }
        public override bool Equals(object obj)
        {
            if (obj is SalarioMinimoDto)
            {
                SalarioMinimoDto zona = obj as SalarioMinimoDto;
                return zona.Id == Id && zona.FechaActualizacion == FechaActualizacion && zona.Salario == Salario && zona.EstaEliminado == EstaEliminado;
            }
            return false;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode() & FechaActualizacion.GetHashCode() & Salario.GetHashCode() & EstaEliminado.GetHashCode();
        }
    }
}