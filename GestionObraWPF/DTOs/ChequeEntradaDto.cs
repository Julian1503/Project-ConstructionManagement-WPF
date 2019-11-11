using GestionObraWPF.Constantes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionObraWPF.DTOs
{
    public class ChequeEntradaDto : BaseDto
    {
        private decimal _monto;
        private string _concepto;

        public DateTime FechaDesde { get; set; } = DateTime.Now;
        public DateTime FechaHasta { get; set; } = DateTime.Now;
        public BancoDto Banco { get; set; }
        public UsadoEn Usado { get; set; }
        public long BancoId { get; set; }
        public int Numero { get; set; }
        public string Serie { get; set; }
        public string Concepto { get { return _concepto; } set { SetProperty(ref _concepto, value); } }
        public decimal Monto { get { return _monto; } set { SetProperty(ref _monto,value); } }
        public string EmitidoPor { get; set; }
        public bool Contado { get; set; }
        public decimal MontoContado { get; set; }
    }
}
