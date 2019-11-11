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
    public class CajaCerradasViewModel : BindableBase
    {
        private DateTime _fechaDesde = DateTime.Now;
        private DateTime _fechaHasta = DateTime.Now;
        private ObservableCollection<CajaDto> _cajas;


        public CajaCerradasViewModel()
        {
            FiltrarCommand = new DelegateCommand(Filtrar);

        }
        public ICommand FiltrarCommand { get; set; }
        public ObservableCollection<CajaDto> Cajas { get { return _cajas; } set { SetProperty(ref _cajas, value); } }
        public DateTime FechaDesde { get { return _fechaDesde; } set { SetProperty(ref _fechaDesde, value); } }
        public DateTime FechaHasta { get { return _fechaHasta; } set { SetProperty(ref _fechaHasta, value); } }

        private async void Filtrar()
        {
            if (FechaDesde < FechaHasta)
            {
                Cajas = new ObservableCollection<CajaDto>(await ApiProcessor.GetApi<CajaDto[]>($"Caja/GetDesde/{FechaDesde.ToString("MM-dd-yyyy")}/{FechaHasta.ToString("MM-dd-yyyy")}"));
            }
        }
    }
}
