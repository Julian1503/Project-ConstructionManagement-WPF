using GestionObraWPF.DTOs;
using GestionObraWPF.Servicios;
using LiveCharts;
using LiveCharts.Wpf;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GestionObraWPF.ViewModels
{
    public class PresupuestosViewModel : BindableBase
    {
        private ObservableCollection<PresupuestoDto> _presupuestos;
        private PresupuestoDto _presupuesto;
        private DateTime _fechaDesde = DateTime.Now;
        private DateTime _fechaHasta = DateTime.Now;
        private decimal _iva;
        private decimal _intereses;
        private decimal _descuentos;
        private decimal _percepciones;
        private decimal _retenciones;
        private decimal _cobrado;
        private decimal _diferencia;
        private decimal _total;
        private int _blanco;
        private int _negro;
        private SeriesCollection _series;
        private SeriesCollection _composicion;

        public PresupuestosViewModel(IEventAggregator eventAggregator)
        {
            FiltrarCommand = new DelegateCommand(Filtrar);
            PointLabel = chartPoint =>
             string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation);
            Composicion = new SeriesCollection();
            Series = new SeriesCollection();
        }
        private bool _activarClientes;
        public bool ActivarClientes
        {
            get { return _activarClientes; }
            set { SetProperty(ref _activarClientes, value); }
        }
        private ObservableCollection<EmpresaDto> _clientes;
        public ObservableCollection<EmpresaDto> Clientes
        {
            get { return _clientes; }
            set { SetProperty(ref _clientes, value); }
        }
        private EmpresaDto _cliente;
        public EmpresaDto Cliente
        {
            get { return _cliente; }
            set { SetProperty(ref _cliente, value); }
        }

        public ICommand FiltrarCommand { get; set; }
        public ObservableCollection<PresupuestoDto> Presupuestos { get { return _presupuestos; } set { SetProperty(ref _presupuestos, value); } }
        public PresupuestoDto Presupuesto { get { return _presupuesto; } set { SetProperty(ref _presupuesto, value); } }
        public DateTime FechaDesde { get { return _fechaDesde; } set { SetProperty(ref _fechaDesde, value); } }
        public DateTime FechaHasta { get { return _fechaHasta; } set { SetProperty(ref _fechaHasta, value); } }
        public decimal Iva { get { return _iva; } set { SetProperty(ref _iva, value); } }
        public decimal Intereses { get { return _intereses; } set { SetProperty(ref _intereses, value); } }
        public decimal Descuentos { get { return _descuentos; } set { SetProperty(ref _descuentos, value); } }
        public decimal Percepciones { get { return _percepciones; } set { SetProperty(ref _percepciones, value); } }
        public decimal Retenciones { get { return _retenciones; } set { SetProperty(ref _retenciones, value); } }
        public decimal Cobrado { get { return _cobrado; } set { SetProperty(ref _cobrado, value); } }
        public decimal Total { get { return _total; } set { SetProperty(ref _total, value); } }
        public int Blanco { get { return _blanco; } set { SetProperty(ref _blanco, value); } }
        public int Negro { get { return _negro; } set { SetProperty(ref _negro, value); } }
        public decimal Diferencia { get { return _diferencia; } set { SetProperty(ref _diferencia, value); } }

        public Func<ChartPoint, string> PointLabel { get; set; }
        public SeriesCollection Series { get { return _series; } set { SetProperty(ref _series, value); } }
        public SeriesCollection Composicion { get { return _composicion; } set { SetProperty(ref _composicion, value); } }
        public async Task Inicializar()
        {
            Presupuestos = new ObservableCollection<PresupuestoDto>(await ApiProcessor.GetApi<PresupuestoDto[]>("Presupuesto/GetAprobado"));
            Clientes = new ObservableCollection<EmpresaDto>(await Servicios.ApiProcessor.GetApi<EmpresaDto[]>("Empresa/GetAll"));
            CalcularComprobantes();
        }



        private async void Filtrar()
        {
            if (FechaDesde <= FechaHasta)
            {
                if (ActivarClientes)
                {
                    Presupuestos = new ObservableCollection<PresupuestoDto>(await ApiProcessor.GetApi<PresupuestoDto[]>($"Presupuesto/GetByCliente/{FechaDesde.ToString("MM-dd-yyyy")}/{FechaHasta.ToString("MM-dd-yyyy")}/{Cliente.Id}"));
                }
                else
                {
                    Presupuestos = new ObservableCollection<PresupuestoDto>(await ApiProcessor.GetApi<PresupuestoDto[]>($"Presupuesto/GetByFecha/{FechaDesde.ToString("MM-dd-yyyy")}/{FechaHasta.ToString("MM-dd-yyyy")}"));
                }
                CalcularComprobantes();
            }
        }

        private void CalcularComprobantes()
        {
            Iva = Presupuestos.Sum(x => x.Iva);
            Retenciones = Presupuestos.Sum(x => x.Retenciones);
            Intereses = Presupuestos.Sum(x => x.Interes);
            Descuentos = Presupuestos.Sum(x => x.Descuento);
            Percepciones = Presupuestos.Sum(x => x.Percepciones);
            Retenciones = Presupuestos.Sum(x => x.Retenciones);
            Total = Presupuestos.Sum(x => x.Total);
            var subtotal = Presupuestos.Sum(x => x.SubTotal);
            Cobrado = Presupuestos.Sum(x => x.Cobrado);
            Diferencia = Total - Cobrado;
            Blanco = Presupuestos.Where(x => x.Iva > 0 || x.Percepciones > 0 || x.Retenciones > 0).Count();
            Negro = Presupuestos.Count() - Blanco;
            //    Series.Clear();
            //    Series.Add(new PieSeries { Title = "Blanco", Values = new ChartValues<decimal>(new decimal[] { Blanco }), DataLabels = true, LabelPoint = PointLabel });
            //    Series.Add(new PieSeries { Title = "Negro", Values = new ChartValues<decimal>(new decimal[] { Negro }), DataLabels = true, LabelPoint = PointLabel });
            //    Composicion.Add(new PieSeries { Title = "SubTotal", Values = new ChartValues<decimal>(new decimal[] { subtotal }), DataLabels = true, LabelPoint = PointLabel });
            //    Composicion.Add(new PieSeries { Title = "Iva", Values = new ChartValues<decimal>(new decimal[] { Iva }), DataLabels = true, LabelPoint = PointLabel });
            //    Composicion.Add(new PieSeries { Title = "Retenciones", Values = new ChartValues<decimal>(new decimal[] { Retenciones }), DataLabels = true, LabelPoint = PointLabel });
            //    Composicion.Add(new PieSeries { Title = "Intereses", Values = new ChartValues<decimal>(new decimal[] { Intereses }), DataLabels = true, LabelPoint = PointLabel });
            //    Composicion.Add(new PieSeries { Title = "Descuentos", Values = new ChartValues<decimal>(new decimal[] { Descuentos }), DataLabels = true, LabelPoint = PointLabel });
            //    Composicion.Add(new PieSeries { Title = "Percepciones", Values = new ChartValues<decimal>(new decimal[] { Percepciones }), DataLabels = true, LabelPoint = PointLabel });
            //    Composicion.Add(new PieSeries { Title = "Retenciones", Values = new ChartValues<decimal>(new decimal[] { Retenciones }), DataLabels = true, LabelPoint = PointLabel });
        }
    }
}
