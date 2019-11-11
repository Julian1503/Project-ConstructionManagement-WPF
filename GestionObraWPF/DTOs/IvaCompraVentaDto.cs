using GestionObraWPF.Constantes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionObraWPF.DTOs
{
    public class IvaCompraVentaDto : BaseDto
    {
        private DateTime _fecha;
        public DateTime Fecha
        {
            get { return _fecha; }
            set { SetProperty(ref _fecha, value); }
        }
        private long _cae;
        public long Cae
        {
            get { return _cae; }
            set { SetProperty(ref _cae, value); }
        }
        private long _numeroComprobante;
        public long NumeroComprobante
        {
            get { return _numeroComprobante; }
            set { SetProperty(ref _numeroComprobante, value); }
        }
        private string _cuit;
        public string CUIT
        {
            get { return _cuit; }
            set { SetProperty(ref _cuit, value); }
        }

        private decimal _monto;
        public decimal Monto
        {
            get { return _monto; }
            set { SetProperty(ref _monto, value); }
        }

        private decimal? _debe;
        public decimal? Debe
        {
            get { return _debe; }
            set { SetProperty(ref _debe, value); }
        }
        private decimal? _haber;
        public decimal? Haber
        {
            get { return _haber; }
            set { SetProperty(ref _haber, value); }
        }
        private string _condicionIva;
        public string CondicionIva
        {
            get { return _condicionIva; }
            set { SetProperty(ref _condicionIva, value); }
        }
        private string _razonSocial;
        public string RazonSocial
        {
            get { return _razonSocial; }
            set { SetProperty(ref _razonSocial, value); }
        }
    }
}
