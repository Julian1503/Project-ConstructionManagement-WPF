using GestionObraWPF.DTOs;
using GestionObraWPF.Servicios;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GestionObraWPF.ViewModels
{
    public class ReporteTransferenciaViewModel : BindableBase
    {
        private DateTime _fechaDesde = DateTime.Now;
        private DateTime _fechaHasta = DateTime.Now;
        private ObservableCollection<TransferenciaDto> _transferencia;
        private decimal _entrada;
        private decimal _salida;
        private decimal _total;
        private bool _activarCliente;
        public bool ActivarCliente
        {
            get { return _activarCliente; }
            set { SetProperty(ref _activarCliente, value); }
        }
        private string _cliente;
        public string Cliente
        {
            get { return _cliente; }
            set { SetProperty(ref _cliente, value); }
        }
        private string _concepto;
        public string Concepto
        {
            get { return _concepto; }
            set { SetProperty(ref _concepto, value); }
        }
        private bool _activarConcepto;
        public bool ActivarConcepto
        {
            get { return _activarConcepto; }
            set { SetProperty(ref _activarConcepto, value); }
        }
        public decimal Entrada { get { return _entrada; } set { SetProperty(ref _entrada, value); } }
        public decimal Salida { get { return _salida; } set { SetProperty(ref _salida, value); } }
        public decimal Total { get { return _total; } set { SetProperty(ref _total, value); } }

        public ReporteTransferenciaViewModel()
        {
            FiltrarCommand = new DelegateCommand(Filtrar);
        }
        public ICommand FiltrarCommand { get; set; }
        public DateTime FechaDesde { get { return _fechaDesde; } set { SetProperty(ref _fechaDesde, value); } }
        public DateTime FechaHasta { get { return _fechaHasta; } set { SetProperty(ref _fechaHasta, value); } }

        public ObservableCollection<TransferenciaDto> Transferencia { get { return _transferencia; } set { SetProperty(ref _transferencia, value); } }

        public async Task Inicializar()
        {
            Transferencia = new ObservableCollection<TransferenciaDto>(await ApiProcessor.GetApi<TransferenciaDto[]>("Transferencia/GetAll"));
            CalcularComprobantes();
        }



        private async void Filtrar()
        {
            if (FechaDesde <= FechaHasta)
            {
                if (ActivarCliente && !string.IsNullOrWhiteSpace(Cliente))
                {
                    Transferencia = new ObservableCollection<TransferenciaDto>(await ApiProcessor.GetApi<TransferenciaDto[]>($"Transferencia/GetByPaguese/{FechaDesde.ToString("MM-dd-yyyy")}/{FechaHasta.ToString("MM-dd-yyyy")}/{Cliente}"));
                }
                else if (ActivarConcepto &&  !string.IsNullOrWhiteSpace(Concepto))
                {
                    Transferencia = new ObservableCollection<TransferenciaDto>(await ApiProcessor.GetApi<TransferenciaDto[]>($"Transferencia/GetByConcepto/{FechaDesde.ToString("MM-dd-yyyy")}/{FechaHasta.ToString("MM-dd-yyyy")}/{Concepto}"));
                }
                else
                {
                    Transferencia = new ObservableCollection<TransferenciaDto>(await ApiProcessor.GetApi<TransferenciaDto[]>($"Transferencia/GetByFecha/{FechaDesde.ToString("MM-dd-yyyy")}/{FechaHasta.ToString("MM-dd-yyyy")}"));
                }
                CalcularComprobantes();
            }
        }
        private void CalcularComprobantes()
        {
            Entrada = Transferencia.Where(x => x.Entrada).Sum(x => x.Monto);
            Salida = Transferencia.Where(x => !x.Entrada).Sum(x => x.Monto);
            Total = Entrada - Salida;

        }
    }
}

