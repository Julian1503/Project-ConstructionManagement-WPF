using GestionObraWPF.DTOs;
using GestionObraWPF.Servicios;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GestionObraWPF.ViewModels
{
    public class ReporteChequeEntradaViewModel : BindableBase
    {
        private DateTime _fechaDesde = DateTime.Now;
        private DateTime _fechaHasta = DateTime.Now;
        private ObservableCollection<ChequeEntradaDto> _chequeSalido;
        public ReporteChequeEntradaViewModel()
        {
            FiltrarCommand = new DelegateCommand(Filtrar);
        }
        public ICommand FiltrarCommand { get; set; }
        public DateTime FechaDesde { get { return _fechaDesde; } set { SetProperty(ref _fechaDesde, value); } }
        public DateTime FechaHasta { get { return _fechaHasta; } set { SetProperty(ref _fechaHasta, value); } }
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
        public ObservableCollection<ChequeEntradaDto> ChequeEntrada { get { return _chequeSalido; } set { SetProperty(ref _chequeSalido, value); } }

        public async Task Inicializar()
        {
            ChequeEntrada = new ObservableCollection<ChequeEntradaDto>(await ApiProcessor.GetApi<ChequeEntradaDto[]>("ChequeEntrada/GetAll"));
        }



        private async void Filtrar()
        {
            if (FechaDesde <= FechaHasta)
            {
                if (ActivarConcepto && !string.IsNullOrWhiteSpace(Concepto))
                {
                    ChequeEntrada = new ObservableCollection<ChequeEntradaDto>(await ApiProcessor.GetApi<ChequeEntradaDto[]>($"ChequeEntrada/GetByConcepto/{FechaDesde.ToString("MM-dd-yyyy")}/{FechaHasta.ToString("MM-dd-yyyy")}/{Concepto}"));

                }
                else if (ActivarDePara && !string.IsNullOrWhiteSpace(DePara))
                {
                    ChequeEntrada = new ObservableCollection<ChequeEntradaDto>(await ApiProcessor.GetApi<ChequeEntradaDto[]>($"ChequeEntrada/GetByDePara/{FechaDesde.ToString("MM-dd-yyyy")}/{FechaHasta.ToString("MM-dd-yyyy")}/{DePara}"));
                }
                else
                {
                    ChequeEntrada = new ObservableCollection<ChequeEntradaDto>(await ApiProcessor.GetApi<ChequeEntradaDto[]>($"ChequeEntrada/GetByFecha/{FechaDesde.ToString("MM-dd-yyyy")}/{FechaHasta.ToString("MM-dd-yyyy")}"));
                }
            }
        }
    }
}
