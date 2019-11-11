using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionObraWPF.DTOs
{
    public class ObraEmpleadoDto : BaseDto
    {
        public ObraDto Obra { get; set; }
        public long ObraId { get; set; }
        public long EmpleadoId { get; set; }
        public EmpleadoDto Empleado { get; set; }
    }
}
