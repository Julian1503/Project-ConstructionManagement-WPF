using GestionObraWPF.DTOs;
using GestionObraWPF.Servicios;
using GestionObraWPF.Views.Reportes;
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
    public class ReporteChequeSalidaViewModel : BindableBase
    {
        private DateTime _fechaDesde = DateTime.Now;
        private DateTime _fechaHasta = DateTime.Now;
        private ObservableCollection<ChequeSalidaDto> _chequeSalido;

        private string _pagueseA;
        public string PagueseA
        {
            get { return _pagueseA; }
            set { SetProperty(ref _pagueseA, value); }
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
        private string _numero;
        public string Numero
        {
            get { return _numero; }
            set { SetProperty(ref _numero, value); }
        }
        public ReporteChequeSalidaViewModel()
        {
            FiltrarCommand = new DelegateCommand(Filtrar);
            Concepto = "";
            PagueseA = "";
            Numero = "";
        }
        public ICommand FiltrarCommand { get; set; }
        public DateTime FechaDesde { get { return _fechaDesde; } set { SetProperty(ref _fechaDesde, value); } }
        public DateTime FechaHasta { get { return _fechaHasta; } set { SetProperty(ref _fechaHasta, value); } }

        public ObservableCollection<ChequeSalidaDto> ChequeSalida { get { return _chequeSalido; } set { SetProperty(ref _chequeSalido, value); } }

        public async Task Inicializar()
        {
            ChequeSalida = new ObservableCollection<ChequeSalidaDto>(await ApiProcessor.GetApi<ChequeSalidaDto[]>("ChequeSalida/GetAll"));
        }

        private async void Filtrar()
        {
            if (FechaDesde <= FechaHasta)
            {
                if (ActivarConcepto && !string.IsNullOrWhiteSpace(Concepto))
                {
                    ChequeSalida = new ObservableCollection<ChequeSalidaDto>(await ApiProcessor.GetApi<ChequeSalidaDto[]>($"ChequeSalida/GetByConcepto/{FechaDesde.ToString("MM-dd-yyyy")}/{FechaHasta.ToString("MM-dd-yyyy")}/{Concepto}"));
                }else if  (ActivarDePara && !string.IsNullOrWhiteSpace(DePara))
                {
                    ChequeSalida = new ObservableCollection<ChequeSalidaDto>(await ApiProcessor.GetApi<ChequeSalidaDto[]>($"ChequeSalida/GetByDePara/{FechaDesde.ToString("MM-dd-yyyy")}/{FechaHasta.ToString("MM-dd-yyyy")}/{DePara}"));
                }
                else
                {
                    ChequeSalida = new ObservableCollection<ChequeSalidaDto>(await ApiProcessor.GetApi<ChequeSalidaDto[]>($"ChequeSalida/GetByFecha/{FechaDesde.ToString("MM-dd-yyyy")}/{FechaHasta.ToString("MM-dd-yyyy")}"));
                }
            }
        }

    }
}
