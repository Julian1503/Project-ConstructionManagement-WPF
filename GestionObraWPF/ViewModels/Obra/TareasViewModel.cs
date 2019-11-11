using GestionObraWPF.DTOs;
using GestionObraWPF.Helpers;
using GestionObraWPF.Servicios;
using GestionObraWPF.ViewModels.ABMs;
using GestionObraWPF.ViewModels.EventAgreggator;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace GestionObraWPF.ViewModels
{
    public class TareasViewModel : BaseABMViewModel
    {
        public IEventAggregator eventAggregator { get; set; }
        private ObservableCollection<TareaDto> _tareas;
        private TareaDto _tarea;
        private ObraDto _obra;
        public ObraDto Obra { get { return _obra; } set { SetProperty(ref _obra, value); } }
        public TareaDto Tarea { get { return _tarea; } set { SetProperty(ref _tarea, value); } }
        public ObservableCollection<TareaDto> Tareas
        {
            get { return _tareas; }
            set
            {
                SetProperty(ref _tareas, value);
            }
        }

        public ICommand EjecutarTareaCommand { get; set; }
        public ICommand PendienteTareaCommand { get; set; }
        public ICommand FinalizarTareaCommand { get; set; }
        private Visibility _visible = Visibility.Visible;
        public Visibility Visible { get { return _visible; } set { SetProperty(ref _visible, value); } }
        public TareasViewModel(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            eventAggregator.GetEvent<ObraAgreggator>().Subscribe(ObtenerObra);
            CrearObraCommand = new DelegateCommand(Nuevo);
            CancelarCommand = new DelegateCommand(Cancelar);
            eventAggregator.GetEvent<PubSubEvent<Visibility>>().Subscribe(Ocultar);
            eventAggregator.GetEvent<PubSubEvent<TareaDto>>().Subscribe(ObtenerTarea);
            eventAggregator.GetEvent<PubSubEvent<TimeSpan>>().Subscribe(FinalizarTareaPut);
            EditarObraCommand = new DelegateCommand(Editar, () => ObjetoNull.IsNull(Tarea)).ObservesProperty(() => Tarea);
            EliminarObraCommand = new DelegateCommand(Eliminar, () => ObjetoNull.IsNull(Tarea)).ObservesProperty(() => Tarea);
            EjecutarTareaCommand = new DelegateCommand(EjecutarTarea, () => ObjetoNull.IsNull(Tarea)).ObservesProperty(() => Tarea);
            PendienteTareaCommand = new DelegateCommand(PendienteTarea, () => ObjetoNull.IsNull(Tarea)).ObservesProperty(() => Tarea);
            FinalizarTareaCommand = new DelegateCommand(FinalizarTarea, () => ObjetoNull.IsNull(Tarea)).ObservesProperty(() => Tarea);
        }

        private void Ocultar(Visibility obj)
        {
            Visible = obj;
        }
        private async void FinalizarTareaPut(TimeSpan obj)
        {
            if (Tarea != null && Tarea.DescripcionTarea != null)
            {
                Tarea.Estado = Constantes.EstadoTarea.Finalizada;
                Tarea.TiempoEmpleado = obj;
                await ApiProcessor.PutApi(Tarea, $"Tarea/{Tarea.Id}");
                await Inicializar();
                eventAggregator.GetEvent<PubSubEvent<int>>().Publish(1);
            }
        }

        private void FinalizarTarea()
        {
            if (Tarea != null && Tarea.DescripcionTarea != null)
            {
                Tarea = Tarea;
                eventAggregator.GetEvent<PubSubEvent<bool>>().Publish(true);
            }
        }

        private async void PendienteTarea()
        {
            if (Tarea != null && Tarea.DescripcionTarea != null)
            {
                Tarea.Estado = Constantes.EstadoTarea.Pendiente;
                Tarea.TiempoEmpleado = new TimeSpan();
                await ApiProcessor.PutApi(Tarea, $"Tarea/{Tarea.Id}");
                Tarea = null;
                await Inicializar();
                eventAggregator.GetEvent<PubSubEvent<int>>().Publish(1);
            }
        }

        private async void EjecutarTarea()
        {
            Tarea.Estado = Constantes.EstadoTarea.Iniciada;
            Tarea.TiempoEmpleado = new TimeSpan();
            await ApiProcessor.PutApi(Tarea, $"Tarea/{Tarea.Id}");
            Tarea = null;
            await Inicializar();
            eventAggregator.GetEvent<PubSubEvent<int>>().Publish(1);
        }

        private void ObtenerTarea(TareaDto obj)
        {
            Tarea = obj;
            if (Tarea != null)
            {
                BotonABM();
            }
        }

        private async void ObtenerObra(ObraDto obj)
        {
            Obra = obj;
            await Inicializar();
        }

        public override async Task Inicializar()
        {
            Tareas = new ObservableCollection<TareaDto>(await ApiProcessor.GetApi<TareaDto[]>($"Tarea/GetByObra/{Obra.Id}"));
            if (Tareas.Count>0)
            {
                eventAggregator.GetEvent<PubSubEvent<ObservableCollection<TareaDto>>>().Publish(Tareas);
            }
        }

        protected async override Task CrearNuevoElemento()
        {
            Tarea.Estado = Constantes.EstadoTarea.Pendiente;
            Tarea.ObraId = Obra.Id;
            Tarea.Obra = Obra;
            Tarea.DescripcionTareaId = Tarea.DescripcionTarea.Id;
            Tarea.TiempoEmpleado = new TimeSpan();
            await ApiProcessor.PostApi(Tarea, "Tarea/Insert");
            eventAggregator.GetEvent<BoolAgreggator>().Publish(new PopUp(btnDialogText, MostrarCrearObra, ControlesDialog));
            await Inicializar();
            eventAggregator.GetEvent<PubSubEvent<int>>().Publish(1);
            Tarea = null;
        }
        protected async override Task EliminarElemento()
        {
            eventAggregator.GetEvent<BoolAgreggator>().Publish(new PopUp(btnDialogText, MostrarCrearObra, ControlesDialog));
            await ApiProcessor.DeleteApi($"Tarea/{Tarea.Id}");
            Tarea = null;
            await Inicializar();
            eventAggregator.GetEvent<PubSubEvent<int>>().Publish(1);
        }
        protected async override Task EditarElemento()
        {
            eventAggregator.GetEvent<BoolAgreggator>().Publish(new PopUp(btnDialogText, MostrarCrearObra, ControlesDialog));
            await Servicios.ApiProcessor.PutApi(Tarea, $"Tarea/{Tarea.Id}");
            Tarea = null;
            await Inicializar();
            eventAggregator.GetEvent<PubSubEvent<int>>().Publish(1);
        }

        protected override void Editar()
        {
            eventAggregator.GetEvent<TareaAgreggator>().Publish(Tarea);
            base.Editar();
            eventAggregator.GetEvent<BoolAgreggator>().Publish(new PopUp(btnDialogText, MostrarCrearObra, ControlesDialog));
        }
        protected override void Eliminar()
        {
            eventAggregator.GetEvent<TareaAgreggator>().Publish(Tarea);
            base.Eliminar();
            eventAggregator.GetEvent<BoolAgreggator>().Publish(new PopUp(btnDialogText, MostrarCrearObra, ControlesDialog));
        }
        protected override void Nuevo()
        {
            base.Nuevo();
            Tarea = new TareaDto();
            eventAggregator.GetEvent<BoolAgreggator>().Publish(new PopUp(btnDialogText, MostrarCrearObra, ControlesDialog));
        }
        protected override void Cancelar()
        {
            base.Cancelar();
            Tarea = null;
            eventAggregator.GetEvent<BoolAgreggator>().Publish(new PopUp(btnDialogText, MostrarCrearObra, ControlesDialog));
        }



    }
}
