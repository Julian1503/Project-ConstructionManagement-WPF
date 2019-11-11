using GestionObraWPF.DTOs;
using GestionObraWPF.Servicios;
using GestionObraWPF.ViewModels.ABMs;
using GestionObraWPF.ViewModels.EventAgreggator;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using Prism.Commands;
using Prism.Events;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace GestionObraWPF.ViewModels
{
    public class ObraPlanificacionDetalleViewModel : BaseABMViewModel, INavigationAware
    {

        private TareaDto _tarea;
        public TareaDto Tarea { get { return _tarea; } set { SetProperty(ref _tarea, value); } }
        public bool Grafica
        {
            get { return _grafica; }
            set
            {
                SetProperty(ref _grafica, value);
            }
        }
        public ObservableCollection<DescripcionTareaDto> DescripcionTareas { get { return _descripcionTareas; } set { SetProperty(ref _descripcionTareas, value); } }
        public ObservableCollection<TareaDto> Tareas { get { return _tareas; } set { SetProperty(ref _tareas, value); } }

        private ObservableCollection<DescripcionTareaDto> _descripcionTareas;
        private IEventAggregator eventAggregator;
        private ObraDto _obra;
        public ObraDto Obra { get { return _obra; } set { SetProperty(ref _obra, value); } }
        public DateTime Fecha { get { return _fecha; } set { SetProperty(ref _fecha, value); } }

        public ICommand TerminarPlanificacion { get; set; }
        public bool DesactivarGrafica { get { return _desactivarGrafica; } set { SetProperty(ref _desactivarGrafica, value); } }

        private IRegionManager regionManager;
        private DateTime _fecha = DateTime.Now;
        private bool _grafica;
        private ObservableCollection<TareaDto> _tareas;
        private bool _desactivarGrafica;

        private void PasandoTarea(TareaDto obj)
        {
            Tarea = obj;
        }


        private void GetAbrirTareaPopUp(PopUp obj)
        {
            MostrarCrearObra = obj.ShowPopUp;
            btnDialogText = obj.ButtonTitle;
            ControlesDialog = obj.ControlersEnable;
            if (btnDialogText == "Crear")
            {
                Tarea = new TareaDto();
            }
        }

        private async void IngresarTarea()
        {
            if ((Tarea.DescripcionTarea != null || Tarea.DescripcionTareaId != 0) && Tarea.NumeroOrden != 0)
            {
                eventAggregator.GetEvent<PubSubEvent<TareaDto>>().Publish(Tarea);
                Tarea = new TareaDto();
            }
        }

        private void Cancelar()
        {
            MostrarCrearObra = !MostrarCrearObra;
            Tarea = null;
            eventAggregator.GetEvent<TareaAgreggator>().Publish(Tarea);
        }

        public ObraPlanificacionDetalleViewModel(IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            this.regionManager = regionManager;
            this.eventAggregator = eventAggregator;
            eventAggregator.GetEvent<BoolAgreggator>().Subscribe(GetAbrirTareaPopUp);
            eventAggregator.GetEvent<TareaAgreggator>().Subscribe(PasandoTarea);
            CrearCommand = new DelegateCommand(IngresarTarea);
            TerminarPlanificacion = new DelegateCommand(FinPlanificacion);
            CancelarCommand = new DelegateCommand(Cancelar);
        }

        private async void FinPlanificacion()
        {
            var tareas = await ApiProcessor.GetApi<TareaDto[]>($"Tarea/GetByObra/{Obra.Id}");
            if (tareas.Length > 0)
            {
                Obra.EstadoObra = Constantes.EstadoObra.EnProceso;
                await ApiProcessor.PutApi(Obra, $"Obra/{Obra.Id}");
                MessageBox.Show("Planificacion terminada! Listo para realizar el avance.");
                regionManager.RequestNavigate("Contenido", "Planificacion");
            }
            else
            {
                MessageBox.Show("Ingrese alguna tarea, por favor");
            }
        }

        public async void OnNavigatedTo(NavigationContext navigationContext)
        {
            Obra = (ObraDto)navigationContext.Parameters["ObraParametro"];
            await Inicializar();
        }

        public override async Task Inicializar()
        {
            regionManager.RequestNavigate("RegionTareaPlanificacion", "Tarea");
            regionManager.RequestNavigate("Gantt", "Gantt");
            eventAggregator.GetEvent<ObraAgreggator>().Publish(Obra);
            DescripcionTareas = new ObservableCollection<DescripcionTareaDto>(await ApiProcessor.GetApi<DescripcionTareaDto[]>($"DescripcionTarea/GetAll"));
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }



    }
}
