using GestionObraWPF.Constantes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionObraWPF.DTOs
{
    public class ChequeSalidaDto:BaseDto
    {
        public DateTime FechaDesde { get; set; } = DateTime.Now;
        public DateTime FechaHasta { get; set; } = DateTime.Now;
        public BancoDto Banco { get; set; }
        public UsadoEn Usado { get; set; }
        public long BancoId { get; set; }
        public int Numero { get; set; }
        public string Serie { get; set; }
        public string Concepto { get; set; }
        private decimal _monto;
        public decimal Monto
        {
            get { return _monto; }
            set { SetProperty(ref _monto, value); }
        }
        private string _pagueseA;
        public string PagueseA
        {
            get { return _pagueseA; }
            set { SetProperty(ref _pagueseA, value); }
        }
    }
}
