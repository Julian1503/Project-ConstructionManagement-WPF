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
    public class ReporteMaterialesUsadosViewModel : BindableBase
    {
        private DateTime _fechaDesde= DateTime.Now;
        private DateTime _fechaHasta = DateTime.Now;
        private ObservableCollection<JornalMaterialDto> _jornalMateriales;
        public ReporteMaterialesUsadosViewModel()
        {
            FiltrarCommand = new DelegateCommand(Filtrar);
        }
        public ICommand FiltrarCommand { get; set; }
        public DateTime FechaDesde { get { return _fechaDesde; } set { SetProperty(ref _fechaDesde, value); } }
        public DateTime FechaHasta { get { return _fechaHasta; } set { SetProperty(ref _fechaHasta, value); } }

        public ObservableCollection<JornalMaterialDto> JornalMateriales { get { return _jornalMateriales; } set { SetProperty(ref _jornalMateriales, value); } }

        public async Task Inicializar()
        {
            JornalMateriales = new ObservableCollection<JornalMaterialDto>(await ApiProcessor.GetApi<JornalMaterialDto[]>("JornalMaterial/GetAll"));
        }

        private async void Filtrar()
        {
            if (FechaDesde <= FechaHasta)
            {
                JornalMateriales = new ObservableCollection<JornalMaterialDto>(await ApiProcessor.GetApi<JornalMaterialDto[]>($"JornalMaterial/GetByFecha/{FechaDesde.ToString("MM-dd-yyyy")}/{FechaHasta.ToString("MM-dd-yyyy")}"));
            }
        }
    }
}
