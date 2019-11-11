using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionObraWPF.DTOs
{
    public class CuentaDto : BaseDto
    {
        private string _descripcionSubrubro;
        public string DescripcionSubRubro
        {
            get { return _descripcionSubrubro; }
            set { SetProperty(ref _descripcionSubrubro, value); }
        }
        private decimal? _saldoSubrubro;
        public decimal? SaldoSubRubro
        {
            get { return _saldoSubrubro; }
            set { SetProperty(ref _saldoSubrubro, value); }
        }
        private string _descripcion;
        public string DescripcionCuenta
        {
            get { return _descripcion; }
            set { SetProperty(ref _descripcion, value); }
        }
        private decimal? _saldo;
        public decimal? SaldoCuenta
        {
            get { return _saldo; }
            set { SetProperty(ref _saldo, value); }
        }
    }
}
