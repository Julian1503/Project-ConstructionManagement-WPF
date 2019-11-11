using GestionObraWPF.Constantes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionObraWPF.DTOs
{
   public class TransferenciaDto : BaseDto
    {
        public UsadoEn Usado { get; set; }
        public long BancoId { get; set; }
        public string PagueseA{ get; set; }
        public long Numero { get; set; }
        public decimal Monto { get; set; }
        public bool Entrada { get; set; }
        public decimal ImpuestoBancario { get; set; }
        public string Concepto { get; set; }
        public DateTime Fecha { get; set; }

        public virtual BancoDto Banco { get; set; }
    }
}
