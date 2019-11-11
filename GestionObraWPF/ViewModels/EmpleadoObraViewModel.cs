using GestionObraWPF.DTOs;
using GestionObraWPF.Helpers;
using GestionObraWPF.Servicios;
using GestionObraWPF.ViewModels.ABMs;
using GestionObraWPF.ViewModels.EventAgreggator;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace GestionObraWPF.ViewModels
{
    public class EmpleadoObraViewModel : BaseABMViewModel
    {
        public IEventAggregator eventAggregator { get; set; }
        private ObservableCollection<EmpleadoDto> _tareas;
        private ObraEmpleadoDto _empleadoObra;
        private ObraDto _obra;

        public ObraDto Obra { get { return _obra; } set { SetProperty(ref _obra, value); } }
        public ObraEmpleadoDto ObraEmpleado { get { return _empleadoObra; } set { SetProperty(ref _empleadoObra, value); } }
        public ObservableCollection<EmpleadoDto> Empleados
        {
            get { return _tareas; }
            set
            {
                SetProperty(ref _tareas, value);
            }
        }

        public ICommand EjecutarTareaCommand { get; set; }
        public ICommand PendienteTareaCommand { get; }
        public ICommand FinalizarTareaCommand { get; }
        private Visibility _visible = Visibility.Visible;
        private EmpleadoDto _empleado;

        public Visibility Visible { get { return _visible; } set { SetProperty(ref _visible, value); } }

        public EmpleadoDto Empleado { get { return _empleado; } set { SetProperty(ref _empleado, value); } }

        public EmpleadoObraViewModel(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            eventAggregator.GetEvent<ObraAgreggator>().Subscribe(ObtenerObra);
            CrearObraCommand = new DelegateCommand(Nuevo);
            CancelarCommand = new DelegateCommand(Cancelar);
            eventAggregator.GetEvent<PubSubEvent<Visibility>>().Subscribe(Ocultar);
            eventAggregator.GetEvent<PubSubEvent<ObraEmpleadoDto>>().Subscribe(ObtenerEmpleado);
            EliminarObraCommand = new DelegateCommand(Eliminar, () => ObjetoNull.IsNull(Empleado)).ObservesProperty(() => Empleado);
        }

        private void Ocultar(Visibility obj)
        {
            Visible = obj;
        }
    

        private void ObtenerEmpleado(ObraEmpleadoDto obj)
        {
            ObraEmpleado = obj;
            if (ObraEmpleado != null)
            {
                if (ObraEmpleado.Empleado != null)
                {
                    BotonABM();
                }
            }
        }

        private async void ObtenerObra(ObraDto obj)
        {
            Obra = obj;
            await Inicializar();
        }

        public override async Task Inicializar()
        {
            Empleados = new ObservableCollection<EmpleadoDto>(await ApiProcessor.GetApi<EmpleadoDto[]>($"ObraEmpleado/GetByObra/{Obra.Id}"));
        }

        protected async override Task CrearNuevoElemento()
        {
            ObraEmpleado.ObraId = Obra.Id;
            ObraEmpleado.EmpleadoId = ObraEmpleado.Empleado.Id;
            await ApiProcessor.PostApi(ObraEmpleado, "ObraEmpleado/Insert");
            eventAggregator.GetEvent<BoolAgreggator>().Publish(new PopUp(btnDialogText, MostrarCrearObra, ControlesDialog));
            await Inicializar();
            ObraEmpleado = null;
            ObraEmpleado = new ObraEmpleadoDto();
        }
        protected async override Task EliminarElemento()
        {
            eventAggregator.GetEvent<BoolAgreggator>().Publish(new PopUp(btnDialogText, MostrarCrearObra, ControlesDialog));
            var a = await ApiProcessor.GetApi<ObraEmpleadoDto>($"ObraEmpleado/GetByObraEmpleado/{ObraEmpleado.ObraId}/{ObraEmpleado.EmpleadoId}");
            await ApiProcessor.DeleteApi($"ObraEmpleado/{a.Id}");
            ObraEmpleado = null;
            await Inicializar();
            eventAggregator.GetEvent<PubSubEvent<int>>().Publish(1);
        }

        protected override void Eliminar()
        {
            eventAggregator.GetEvent<PubSubEvent<ObraEmpleadoDto>>().Unsubscribe(ObtenerEmpleado);
            ObraEmpleado = new ObraEmpleadoDto();
            ObraEmpleado.ObraId = Obra.Id;
            ObraEmpleado.Obra = Obra;
            ObraEmpleado.EmpleadoId = Empleado.Id;
            ObraEmpleado.Empleado = Empleado;
            eventAggregator.GetEvent<PubSubEvent<ObraEmpleadoDto>>().Publish(ObraEmpleado);
            base.Eliminar();
            eventAggregator.GetEvent<BoolAgreggator>().Publish(new PopUp(btnDialogText, MostrarCrearObra, ControlesDialog));
            eventAggregator.GetEvent<PubSubEvent<ObraEmpleadoDto>>().Subscribe(ObtenerEmpleado);
        }

        protected override void Nuevo()
        {
            base.Nuevo();
            ObraEmpleado = new ObraEmpleadoDto();
            eventAggregator.GetEvent<BoolAgreggator>().Publish(new PopUp(btnDialogText, MostrarCrearObra, ControlesDialog));
        }
        protected override void Cancelar()
        {
            base.Cancelar();
            ObraEmpleado = null;
            eventAggregator.GetEvent<BoolAgreggator>().Publish(new PopUp(btnDialogText, MostrarCrearObra, ControlesDialog));
        }

    }
}
