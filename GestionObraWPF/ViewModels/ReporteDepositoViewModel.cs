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
    public class ReporteDepositoViewModel : BindableBase
    {
        private DateTime _fechaDesde = DateTime.Now;
        private DateTime _fechaHasta = DateTime.Now;
        private ObservableCollection<DepositoDto> _deposito;
        private decimal _entrada;
        private decimal _salida;
        private decimal _total;
        private bool _activarDePara;
        public bool ActivarDePara
        {
            get { return _activarDePara; }
            set { SetProperty(ref _activarDePara, value); }
        }
        private string _dePara;
        public string DePara
        {
            get { return _dePara; }
            set { SetProperty(ref _dePara, value); }
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

        public ReporteDepositoViewModel()
        {
            FiltrarCommand = new DelegateCommand(Filtrar);
        }
        public ICommand FiltrarCommand { get; set; }
        public DateTime FechaDesde { get { return _fechaDesde; } set { SetProperty(ref _fechaDesde, value); } }
        public DateTime FechaHasta { get { return _fechaHasta; } set { SetProperty(ref _fechaHasta, value); } }

        public ObservableCollection<DepositoDto> Deposito { get { return _deposito; } set { SetProperty(ref _deposito, value); } }

        public async Task Inicializar()
        {
            Deposito = new ObservableCollection<DepositoDto>(await ApiProcessor.GetApi<DepositoDto[]>("Deposito/GetAll"));
            CalcularComprobantes();
        }



        private async void Filtrar()
        {
            if (FechaDesde <= FechaHasta)
            {
                if (ActivarConcepto && !string.IsNullOrWhiteSpace(Concepto))
                {
                    Deposito = new ObservableCollection<DepositoDto>(await ApiProcessor.GetApi<DepositoDto[]>($"Deposito/GetByFecha/{FechaDesde.ToString("MM-dd-yyyy")}/{FechaHasta.ToString("MM-dd-yyyy")}/{Concepto}"));
                }
                else if (ActivarDePara && !string.IsNullOrWhiteSpace(DePara))
                {
                    Deposito = new ObservableCollection<DepositoDto>(await ApiProcessor.GetApi<DepositoDto[]>($"Deposito/GetByDePara/{FechaDesde.ToString("MM-dd-yyyy")}/{FechaHasta.ToString("MM-dd-yyyy")}/{DePara}"));
                }
                else
                {
                    Deposito = new ObservableCollection<DepositoDto>(await ApiProcessor.GetApi<DepositoDto[]>($"Deposito/GetByFecha/{FechaDesde.ToString("MM-dd-yyyy")}/{FechaHasta.ToString("MM-dd-yyyy")}"));
                }
                CalcularComprobantes();
            }
        }
        private void CalcularComprobantes()
        {
            Entrada = Deposito.Where(x => x.Entrada).Sum(x => x.Monto);
            Salida = Deposito.Where(x => !x.Entrada).Sum(x => x.Monto);
            Total = Entrada - Salida;
        }
    }
}

