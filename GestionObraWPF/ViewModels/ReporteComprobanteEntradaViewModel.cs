using GestionObraWPF.DTOs;
using GestionObraWPF.Servicios;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GestionObraWPF.ViewModels
{
    public class ReporteComprobanteEntradaViewModel : BindableBase
    {
        
        private ObservableCollection<ComprobanteEntradaDto> _comprobantesEntrada;
        private decimal _iva;
        private decimal _intereses;
        private decimal _descuentos;
        private decimal _total;
        private int _blanco;
        private int _negro;
        private RubroDto _rubro;
        private SubRubroDto _subRubro;
        private ObservableCollection<RubroDto> _rubros;
        private ObservableCollection<SubRubroDto> _subRubros;
        private bool _activarRubro;
        private bool _activarSubRubro;
        private RubroDto _rubroDos;

        public decimal Iva { get { return _iva; } set { SetProperty(ref _iva, value); } }
        public decimal Intereses { get { return _intereses; } set { SetProperty(ref _intereses, value); } }
        public decimal Descuentos { get { return _descuentos; } set { SetProperty(ref _descuentos, value); } }
        public decimal Total { get { return _total; } set { SetProperty(ref _total, value); } }
        public int Blanco { get { return _blanco; } set { SetProperty(ref _blanco, value); } }
        public int Negro { get { return _negro; } set { SetProperty(ref _negro, value); } }
        public ReporteComprobanteEntradaViewModel()
        {
            FiltrarCommand = new DelegateCommand(Filtrar);
        }
        public ICommand FiltrarCommand { get; set; }
        private DateTime _fechaDesde = DateTime.Now;
        private DateTime _fechaHasta = DateTime.Now;
        public DateTime FechaDesde { get { return _fechaDesde; } set { SetProperty(ref _fechaDesde, value); } }
        public DateTime FechaHasta { get { return _fechaHasta; } set { SetProperty(ref _fechaHasta, value); } }

        public ObservableCollection<ComprobanteEntradaDto> ComprobantesEntrada { get { return _comprobantesEntrada; } set { SetProperty(ref _comprobantesEntrada, value); } }

        public async Task Inicializar()
        {
            ComprobantesEntrada = new ObservableCollection<ComprobanteEntradaDto>(await ApiProcessor.GetApi<ComprobanteEntradaDto[]>("ComprobanteEntrada/GetAll"));
            Rubros = new ObservableCollection<RubroDto>(await ApiProcessor.GetApi<RubroDto[]>("Rubro/GetByEntrada"));
                CalcularComprobantes();
        }



        private async void Filtrar()
        {
            if (FechaDesde <= FechaHasta)
            {
                if (ActivarRubro)
                {
                    ComprobantesEntrada = new ObservableCollection<ComprobanteEntradaDto>(await ApiProcessor.GetApi<ComprobanteEntradaDto[]>($"ComprobanteEntrada/GetByRubro/{FechaDesde.ToString("MM-dd-yyyy")}/{FechaHasta.ToString("MM-dd-yyyy")}/{Rubro.Id}"));
                }
                else if (ActivarSubRubro)
                {
                    ComprobantesEntrada = new ObservableCollection<ComprobanteEntradaDto>(await ApiProcessor.GetApi<ComprobanteEntradaDto[]>($"ComprobanteEntrada/GetBySubRubro/{FechaDesde.ToString("MM-dd-yyyy")}/{FechaHasta.ToString("MM-dd-yyyy")}/{SubRubro.Id}"));
                }
                else
                {
                    ComprobantesEntrada = new ObservableCollection<ComprobanteEntradaDto>(await ApiProcessor.GetApi<ComprobanteEntradaDto[]>($"ComprobanteEntrada/GetByFecha/{FechaDesde.ToString("MM-dd-yyyy")}/{FechaHasta.ToString("MM-dd-yyyy")}"));
                }
                CalcularComprobantes();
            }
        }
        private void CalcularComprobantes()
        {
            Iva = ComprobantesEntrada.Sum(x => x.Iva);
            Intereses = ComprobantesEntrada.Sum(x => x.Interes);
            Descuentos = ComprobantesEntrada.Sum(x => x.Descuento);
            Total = ComprobantesEntrada.Sum(x => x.Monto) + Iva +  + Intereses - Descuentos ;
            Blanco = ComprobantesEntrada.Where(x => x.TipoComprobanteEntrada==Constantes.TipoComprobanteEntrada.Ninguno).Count();
            Negro = ComprobantesEntrada.Count() - Blanco;
        }

        public bool ActivarRubro { get { return _activarRubro; } set { SetProperty(ref _activarRubro, value); if (ActivarRubro) { ActivarSubRubro = false; RubroDos = null; } } }
        public bool ActivarSubRubro { get { return _activarSubRubro; } set { SetProperty(ref _activarSubRubro, value); if (ActivarSubRubro) { ActivarRubro = false; Rubro = null; SubRubro = null; } }  }
        public RubroDto Rubro { get { return _rubro; } set { SetProperty(ref _rubro, value); } }
        public RubroDto RubroDos { get { return _rubroDos; } set { SetProperty(ref _rubroDos, value);Manejar(); } }
        public SubRubroDto SubRubro { get { return _subRubro; } set { SetProperty(ref _subRubro, value); } }
        public ObservableCollection<SubRubroDto> SubRubros { get { return _subRubros; } set { SetProperty(ref _subRubros, value); } }
        public ObservableCollection<RubroDto> Rubros { get { return _rubros; } set { SetProperty(ref _rubros, value); } }

        private async void Manejar()
        {
            if (RubroDos != null)
                SubRubros = new ObservableCollection<SubRubroDto>(await Servicios.ApiProcessor.GetApi<SubRubroDto[]>($"Subrubro/GetByRubro/{RubroDos.Id}"));

        }
    }
}
