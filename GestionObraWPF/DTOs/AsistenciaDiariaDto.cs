using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionObraWPF.DTOs
{
    public class AsistenciaDiariaDto : BaseDto
    {
        public long EmpleadoId { get; set; }
        public long JornalId { get; set; }
        public JornalDto Jornal { get; set; }
        public bool Asistio { get; set; }
        public TimeSpan Entrada { get; set; } = new TimeSpan(0, 0, 0);
        public TimeSpan Salida { get; set; } = new TimeSpan(0, 0, 0);
        public long? CausaId { get; set; }
        public string Observacion { get; set; } = "";
        public EmpleadoDto Empleado { get; set; }
        public CausaFaltaDto Causa { get; set; }
    }
}
