using GestionObraWPF.DTOs;
using GestionObraWPF.Servicios;
using LiveCharts;
using LiveCharts.Defaults;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GestionObraWPF.ViewModels
{
    public class ListaAsistenciaEmpleadoViewModel : BindableBase
    {
        private ObservableCollection<EmpleadoDto> _empleados;
        private EmpleadoDto _empleado;
        private DateTime _fechaDesde = DateTime.Now;
        private DateTime _fechaHasta = DateTime.Now;
        private ObservableCollection<AsistenciaDiariaDto> _asistenciaDiaria;
        private ChartValues<HeatPoint> _values;
        private string[] _days;
        private string[] _nombres;
        private decimal _asistencia;
        private decimal _ausensia;
        private decimal _porcentaje;

        public ListaAsistenciaEmpleadoViewModel()
        {
            FiltrarCommand = new DelegateCommand(Filtrar);
        }
        public ICommand FiltrarCommand { get; set; }
        public ObservableCollection<EmpleadoDto> Empleados { get { return _empleados; } set { SetProperty(ref _empleados, value); } }
        public ObservableCollection<AsistenciaDiariaDto> AsistenciaDiaria { get { return _asistenciaDiaria; } set { SetProperty(ref _asistenciaDiaria, value); } }
        public EmpleadoDto Empleado { get { return _empleado; } set { SetProperty(ref _empleado, value); } }
        public DateTime FechaDesde { get { return _fechaDesde; } set { SetProperty(ref _fechaDesde, value); } }
        public decimal Asistencia { get { return _asistencia; } set { SetProperty(ref _asistencia, value); } }
        public decimal Ausensia { get { return _ausensia; } set { SetProperty(ref _ausensia, value); } }
        public decimal Porcentaje { get { return _porcentaje; } set { SetProperty(ref _porcentaje, value); } }
        public DateTime FechaHasta { get { return _fechaHasta; } set { SetProperty(ref _fechaHasta, value); } }

        public async Task Inicializar()
        {
            Empleados = new ObservableCollection<EmpleadoDto>(await ApiProcessor.GetApi<EmpleadoDto[]>("Empleado/GetAll"));
        }

        private async void Filtrar()
        {
            if (FechaDesde <= FechaHasta && Empleado != null)
            {
                AsistenciaDiaria = new ObservableCollection<AsistenciaDiariaDto>(await ApiProcessor.GetApi<AsistenciaDiariaDto[]>($"AsistenciaDiaria/GetDesde/{FechaDesde.ToString("MM-dd-yyyy")}/{FechaHasta.ToString("MM-dd-yyyy")}/{Empleado.Id}"));
                foreach (var item in AsistenciaDiaria)
                {
                    if (item.Asistio)
                    {
                        Asistencia++;
                    }
                    else
                    {
                        Ausensia++;
                    }
                }
                Porcentaje = (Asistencia/(Asistencia + Ausensia));
            }
        }
    }
}
