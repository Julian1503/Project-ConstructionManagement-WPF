namespace GestionObraWPF.DTOs
{
    public class CategoriaDto : BaseDto
    {
        public string Descripcion { get; set; } = "";
        public long SalarioMinimoId { get; set; }
        public SalarioMinimoDto SalarioMinimo { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is CategoriaDto)
            {
                CategoriaDto zona = obj as CategoriaDto;
                return zona.Id == Id && zona.Descripcion == Descripcion && zona.SalarioMinimoId == SalarioMinimoId &&  zona.EstaEliminado == EstaEliminado;
            }
            return false;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode() & Descripcion.GetHashCode() & SalarioMinimoId.GetHashCode() & EstaEliminado.GetHashCode();
        }
    }
}