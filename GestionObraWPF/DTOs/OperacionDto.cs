using GestionObraWPF.Constantes;
using System;

namespace GestionObraWPF.DTOs
{
    public class OperacionDto : BaseDto
    {
        private string _concepto = "";
        private decimal? _debe;
        private decimal? _haber;
        private CuentaCorrienteDto _cuentaCorriente;
        private decimal? _descontado;

        public long CuentaCorrienteId { get; set; }
        public CuentaCorrienteDto CuentaCorriente { get { return _cuentaCorriente; } set { SetProperty(ref _cuentaCorriente, value); } }
        public decimal? Debe { get { return _debe; } set { SetProperty(ref _debe, value); } }
        public decimal? Haber { get { return _haber; } set { SetProperty(ref _haber, value); } }
        public decimal? Descontado { get { return _descontado; } set { SetProperty(ref _descontado, value); } }
        private string dePara="";
        public string DePara
        {
            get { return dePara; }
            set { SetProperty(ref dePara, value); }
        }
        public string Concepto
        {
            get { return _concepto; }
            set
            {
                SetProperty(ref _concepto, value);
            }
        } 
        public long Referencia { get; set; }
        public string CodigoCausal { get; set; } = "";
        public DateTime? FechaEmision { get; set; }
        public DateTime? FechaVencimiento { get; set; }
        public string ReferenciaPlus { get; set; } = "";
        public bool EstaEnResumen { get; set; }
        public TipoOperacion TipoOperacion { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is OperacionDto)
            {
                OperacionDto zona = obj as OperacionDto;
                return zona.Id == Id && zona.CuentaCorrienteId == CuentaCorrienteId && zona.Referencia == Referencia && zona.CodigoCausal == CodigoCausal && zona.ReferenciaPlus == ReferenciaPlus && zona.EstaEnResumen == EstaEnResumen && zona.EstaEliminado == EstaEliminado;
            }
            return false;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode() & CuentaCorrienteId.GetHashCode() & Referencia.GetHashCode() & CodigoCausal.GetHashCode() & ReferenciaPlus.GetHashCode() & EstaEnResumen.GetHashCode() & EstaEliminado.GetHashCode();
        }
    }
}
