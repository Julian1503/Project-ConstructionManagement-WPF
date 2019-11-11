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
    public class ReporteComprobanteSalidaViewModel : BindableBase
    {
        private DateTime _fechaDesde = DateTime.Now;
        private DateTime _fechaHasta = DateTime.Now;
        private ObservableCollection<ComprobanteSalidaDto> _comprobantesSalida;
        private decimal _iva;
        private decimal _intereses;
        private decimal _descuentos;
        private decimal _percepciones;
        private decimal _retenciones;
        private decimal _diferencia;
        private decimal _total;
        private int _blanco;
        private int _negro;
        private bool _activarRubro;
        private bool _activarSubRubro;
        private RubroDto _rubro;
        private RubroDto _rubroDos;
        private SubRubroDto _subRubro;
        private ObservableCollection<RubroDto> _rubros;
        private ObservableCollection<SubRubroDto> _subRubros;

        public decimal Iva { get { return _iva; } set { SetProperty(ref _iva, value); } }
        public decimal Intereses { get { return _intereses; } set { SetProperty(ref _intereses, value); } }
        public decimal Descuentos { get { return _descuentos; } set { SetProperty(ref _descuentos, value); } }
        public decimal Percepciones { get { return _percepciones; } set { SetProperty(ref _percepciones, value); } }
        public decimal Retenciones { get { return _retenciones; } set { SetProperty(ref _retenciones, value); } }
        public decimal Total { get { return _total; } set { SetProperty(ref _total, value); } }
        public int Blanco { get { return _blanco; } set { SetProperty(ref _blanco, value); } }
        public int Negro { get { return _negro; } set { SetProperty(ref _negro, value); } }

        public ReporteComprobanteSalidaViewModel()
        {
            FiltrarCommand = new DelegateCommand(Filtrar);
        }
        public ICommand FiltrarCommand { get; set; }
        public DateTime FechaDesde { get { return _fechaDesde; } set { SetProperty(ref _fechaDesde, value); } }
        public DateTime FechaHasta { get { return _fechaHasta; } set { SetProperty(ref _fechaHasta, value); } }

        public ObservableCollection<ComprobanteSalidaDto> ComprobantesSalida { get { return _comprobantesSalida; } set { SetProperty(ref _comprobantesSalida, value); } }

        public async Task Inicializar()
        {
            ComprobantesSalida = new ObservableCollection<ComprobanteSalidaDto>(await ApiProcessor.GetApi<ComprobanteSalidaDto[]>("ComprobanteSalida/GetAll"));
            Rubros = new ObservableCollection<RubroDto>(await ApiProcessor.GetApi<RubroDto[]>("Rubro/GetBySalida"));
            CalcularComprobantes();
        }



        private async void Filtrar()
        {
            if (FechaDesde <= FechaHasta)
            {
                if (ActivarRubro)
                {
                    ComprobantesSalida = new ObservableCollection<ComprobanteSalidaDto>(await ApiProcessor.GetApi<ComprobanteSalidaDto[]>($"ComprobanteSalida/GetByRubro/{FechaDesde.ToString("MM-dd-yyyy")}/{FechaHasta.ToString("MM-dd-yyyy")}/{Rubro.Id}"));
                }
                else if (ActivarSubRubro)
                {
                    ComprobantesSalida = new ObservableCollection<ComprobanteSalidaDto>(await ApiProcessor.GetApi<ComprobanteSalidaDto[]>($"ComprobanteSalida/GetBySubRubro/{FechaDesde.ToString("MM-dd-yyyy")}/{FechaHasta.ToString("MM-dd-yyyy")}/{SubRubro.Id}"));
                }
                else
                {
                    ComprobantesSalida = new ObservableCollection<ComprobanteSalidaDto>(await ApiProcessor.GetApi<ComprobanteSalidaDto[]>($"ComprobanteSalida/GetByFecha/{FechaDesde.ToString("MM-dd-yyyy")}/{FechaHasta.ToString("MM-dd-yyyy")}"));
                }
                CalcularComprobantes();
            }
        }
        private void CalcularComprobantes()
        {
            Iva = ComprobantesSalida.Sum(x => x.Iva);
            Retenciones = ComprobantesSalida.Sum(x => x.Retenciones);
            Intereses = ComprobantesSalida.Sum(x => x.Interes);
            Descuentos = ComprobantesSalida.Sum(x => x.Descuento);
            Percepciones = ComprobantesSalida.Sum(x => x.Percepciones);
            Retenciones = ComprobantesSalida.Sum(x => x.Retenciones);
            Total = ComprobantesSalida.Sum(x => x.Monto) + Iva + Retenciones + Intereses - Descuentos + Percepciones;
            Blanco = ComprobantesSalida.Where(x => x.TipoComprobanteSalida==Constantes.TipoComprobanteSalida.Ninguno).Count();
            Negro = ComprobantesSalida.Count() - Blanco;
        }
        public bool ActivarRubro { get { return _activarRubro; } set { SetProperty(ref _activarRubro, value); if (ActivarRubro) { ActivarSubRubro = false; RubroDos = null; } } }
        public bool ActivarSubRubro { get { return _activarSubRubro; } set { SetProperty(ref _activarSubRubro, value); if (ActivarSubRubro) { ActivarRubro = false; Rubro = null; SubRubro = null; } } }
        public RubroDto Rubro { get { return _rubro; } set { SetProperty(ref _rubro, value); } }
        public RubroDto RubroDos { get { return _rubroDos; } set { SetProperty(ref _rubroDos, value); Manejar(); } }
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
