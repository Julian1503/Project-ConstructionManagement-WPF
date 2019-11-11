namespace GestionObraWPF.DTOs
{
    public class CondicionIvaDto: BaseDto
    {
        public string Descripcion { get; set; } = "";

        public override bool Equals(object obj)
        {
            if (obj is CondicionIvaDto)
            {
                CondicionIvaDto condicionIva = obj as CondicionIvaDto;
                return condicionIva.Id == Id && condicionIva.Descripcion == Descripcion && condicionIva.EstaEliminado == EstaEliminado;
            }
            else
            {
                return false;
            }
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode() & Descripcion.GetHashCode() & EstaEliminado.GetHashCode();
        }
    }
}