using Exportable.Attribute;
using Exportable.Models;
using GestionObraWPF.Constantes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionObraWPF.DTOs
{
    public class TareaDto : BaseDto
    {
        [Exportable(IsIgnored =true)]
        public ObraDto Obra { get; set; }
        [Exportable(IsIgnored =true)]
        public DescripcionTareaDto DescripcionTarea { get; set; }
        [Exportable(IsIgnored =true)]
        public long DescripcionTareaId { get; set; }
        [Exportable(Position = 0, HeaderName = "N° de orden", Format ="#0", TypeValue = FieldValueType.Numeric)]
        public int NumeroOrden { get; set; }
        [Exportable(Position = 1, HeaderName = "Tarea", TypeValue = FieldValueType.Text)]
        public string DescripcionTareaStr => DescripcionTarea.Descripcion;
        [Exportable(Position = 2, HeaderName = "Precede", TypeValue = FieldValueType.Bool)]
        public bool Precede { get; set; }
        public TimeSpan Duracion { get; set; }
        public TimeSpan TiempoEmpleado { get; set; }
        [Exportable(Position = 5, HeaderName = "Observacion", TypeValue = FieldValueType.Text)]
        public string Observacion { get; set; } = "";
        [Exportable(Position = 6, HeaderName = "Estado", TypeValue = FieldValueType.Text)]
        public string EstadoStr => Estado == EstadoTarea.Finalizada ? "Finalizada" : Estado == EstadoTarea.Iniciada ? "Iniciada" : "En proceso";
        [Exportable(IsIgnored =true)]
        public EstadoTarea Estado { get; set; }
        [Exportable(IsIgnored =true)]
        public long ObraId { get; set; }
        public override bool Equals(object obj)
        {
            if (obj is TareaDto)
            {
                TareaDto banco = obj as TareaDto;
                return banco.Id == Id && banco.NumeroOrden == NumeroOrden && banco.EstaEliminado == EstaEliminado && banco.DescripcionTareaId == DescripcionTareaId && banco.ObraId == ObraId && banco.Estado == Estado && banco.Duracion == Duracion && banco.TiempoEmpleado == TiempoEmpleado && banco.Observacion == Observacion;
            }
            return false;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode() & DescripcionTareaId.GetHashCode() & EstaEliminado.GetHashCode() & NumeroOrden.GetHashCode() & ObraId.GetHashCode() & Estado.GetHashCode() & Duracion.GetHashCode() & TiempoEmpleado.GetHashCode() & Observacion.GetHashCode();
        }
    }
}
