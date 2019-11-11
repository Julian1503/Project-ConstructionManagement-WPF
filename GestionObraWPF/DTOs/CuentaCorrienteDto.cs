using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionObraWPF.DTOs
{
   public class CuentaCorrienteDto : BaseDto
    {
        public long BancoId { get; set; }
        public BancoDto Banco { get; set; }
        public decimal MontoMaximo { get; set; }
    }
}
