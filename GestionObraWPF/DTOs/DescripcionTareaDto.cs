namespace GestionObraWPF.DTOs
{
    public class DescripcionTareaDto : BaseDto
    {
        public string Descripcion { get; set; } = "";

        public override bool Equals(object obj)
        {
            if (obj is DescripcionTareaDto)
            {
                DescripcionTareaDto descripcionTarea = obj as DescripcionTareaDto;
                return descripcionTarea.Id == Id && descripcionTarea.Descripcion == Descripcion && descripcionTarea.EstaEliminado == EstaEliminado;
            }
            return false;

        }
        public override int GetHashCode()
        {
            return Id.GetHashCode() & Descripcion.GetHashCode() & EstaEliminado.GetHashCode();
        }
    }
}