using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionObraWPF.DTOs
{
    public class AsistenciaContratistaDto : BaseDto
    {
        public long ContratistaId { get; set; }
        public long JornalId { get; set; }
        public TimeSpan Entrada { get; set; } = new TimeSpan(0, 0, 0);
        public TimeSpan Salida { get; set; } = new TimeSpan(0, 0, 0);
        public string Observacion { get; set; }

        public virtual JornalDto Jornal { get; set; }
        public virtual ContratistaDto Contratista { get; set; }
    }
}
