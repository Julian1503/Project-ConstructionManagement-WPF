using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionObraWPF.Model
{
    public class ObraModel
    {
        public string Path { get; set; }
        public string Descripcion { get; set; }
        public string Codigo { get; set; }
        public string Observiacion { get; set; }
        public DateTime FechaEstimadaInicio { get; set; }
        public long PropietarioId { get; set; }
        public long EncargadoId { get; set; } //Persona
        public long? ZonaId { get; set; }
    }
}
