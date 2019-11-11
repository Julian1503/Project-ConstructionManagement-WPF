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
    public class ListadoAsistenciaContratistaViewModel : BindableBase
    {
        private ObservableCollection<ContratistaDto> _contratistas;
        private ContratistaDto _contratista;
        private DateTime _fechaDesde = DateTime.Now;
        private DateTime _fechaHasta = DateTime.Now;
        private ObservableCollection<AsistenciaContratistaDto> _asistenciaContratista;


        public ListadoAsistenciaContratistaViewModel()
        {
            FiltrarCommand = new DelegateCommand(Filtrar);
        }

        public ICommand FiltrarCommand { get; set; }
        public ObservableCollection<ContratistaDto> Contratistas { get { return _contratistas; } set { SetProperty(ref _contratistas, value); } }
        public ObservableCollection<AsistenciaContratistaDto> AsistenciaContratistas { get { return _asistenciaContratista; } set { SetProperty(ref _asistenciaContratista, value); } }
        public ContratistaDto Contratista { get { return _contratista; } set { SetProperty(ref _contratista, value); } }
        public DateTime FechaDesde { get { return _fechaDesde; } set { SetProperty(ref _fechaDesde, value);} }
        public DateTime FechaHasta { get { return _fechaHasta; } set { SetProperty(ref _fechaHasta, value); }  }

        public async Task Inicializar()
        {
            Contratistas = new ObservableCollection<ContratistaDto>(await ApiProcessor.GetApi<ContratistaDto[]>("Contratista/GetAll"));
        }

        private async void Filtrar()
        {
            if(FechaDesde<=FechaHasta && Contratista != null)
            {
                AsistenciaContratistas = new ObservableCollection<AsistenciaContratistaDto>(await ApiProcessor.GetApi<AsistenciaContratistaDto[]>($"AsistenciaContratista/GetDesde/{FechaDesde.ToString("MM-dd-yyyy")}/{FechaHasta.ToString("MM-dd-yyyy")}/{Contratista.Id}"));
            }
        }
    }
}
