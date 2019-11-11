using GestionObraWPF.DTOs;
using GestionObraWPF.Helpers;
using GestionObraWPF.Servicios;
using GestionObraWPF.ViewModels.ABMs;
using GestionObraWPF.ViewModels.EventAgreggator;
using Prism.Commands;
using Prism.Events;
using Prism.Regions;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace GestionObraWPF.ViewModels
{
    public class JornalViewModel : BaseABMViewModel
    {
        private IRegionManager regionManager;

        public IEventAggregator eventAggregator { get; set; }
        private ObservableCollection<JornalDto> _jornales;
        private JornalDto _jornal;
        private ObraDto _obra;

        public ObraDto Obra { get { return _obra; } set { SetProperty(ref _obra, value); } }
        public JornalDto Jornal { get { return _jornal; } set { SetProperty(ref _jornal, value); } }
        public ObservableCollection<JornalDto> Jornales
        {
            get { return _jornales; }
            set
            {
                SetProperty(ref _jornales, value);
            }
        }

        public ICommand EjecutarJornalCommand { get; set; }
        public ICommand PendienteJornalCommand { get; }
        public ICommand FinalizarJornalCommand { get; }
        private Visibility _visible = Visibility.Visible;
        public Visibility Visible { get { return _visible; } set { SetProperty(ref _visible, value); } }

        public DelegateCommand<JornalDto> DoubleClickCommand { get; }

        public JornalViewModel(IEventAggregator eventAggregator,IRegionManager regionManager)
        {
            this.regionManager = regionManager;
            this.eventAggregator = eventAggregator;
            eventAggregator.GetEvent<ObraAgreggator>().Subscribe(ObtenerObra);
            CrearObraCommand = new DelegateCommand(Nuevo);
            CancelarCommand = new DelegateCommand(Cancelar);
            DoubleClickCommand = new DelegateCommand<JornalDto>(Prueba);
            eventAggregator.GetEvent<PubSubEvent<Visibility>>().Subscribe(Ocultar);
            eventAggregator.GetEvent<PubSubEvent<JornalDto>>().Subscribe(ObtenerJornal);
            EditarObraCommand = new DelegateCommand(Editar, () => ObjetoNull.IsNull(Jornal)).ObservesProperty(() => Jornal);
            EliminarObraCommand = new DelegateCommand(Eliminar, () => ObjetoNull.IsNull(Jornal)).ObservesProperty(() => Jornal);
        }

        private void Prueba(JornalDto obj)
        {
            if (obj != null)
            {
                var nav = new NavigationParameters();
                nav.Add("Jornal", obj);
                regionManager.RequestNavigate("Contenido", "JornalDetalle", nav);
            }
        }

        private void Ocultar(Visibility obj)
        {
            Visible = obj;
        }

        private void ObtenerJornal(JornalDto obj)
        {
            Jornal = obj;
            if (Jornal != null)
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
            try
            {
                Jornales = new ObservableCollection<JornalDto>(await ApiProcessor.GetApi<JornalDto[]>($"Jornal/GetByObra/{Obra.Id}"));
            }catch(Exception e)
            {
                MessageBox.Show("Error de conexion");
            }
        }

        protected async override Task CrearNuevoElemento()
        {
            //Crear jornal
            if (Jornal.NumeroOrden != 0)
            {
                Jornal.ObraId = Obra.Id;
                await ApiProcessor.PostApi(Jornal, "Jornal/Insert");
                eventAggregator.GetEvent<BoolAgreggator>().Publish(new PopUp(btnDialogText, MostrarCrearObra, ControlesDialog));
                await Inicializar();
                Jornal = null;
                Jornal = new JornalDto();
            }
        }
    
    protected async override Task EliminarElemento()
    {
        eventAggregator.GetEvent<BoolAgreggator>().Publish(new PopUp(btnDialogText, MostrarCrearObra, ControlesDialog));
        await ApiProcessor.DeleteApi($"Jornal/{Jornal.Id}");
        await Inicializar();
        eventAggregator.GetEvent<PubSubEvent<int>>().Publish(1);
        Jornal = null;
    }

    protected async override Task EditarElemento()
    {
            if (Jornal.NumeroOrden != 0)
            {
                eventAggregator.GetEvent<BoolAgreggator>().Publish(new PopUp(btnDialogText, MostrarCrearObra, ControlesDialog));
                await Servicios.ApiProcessor.PutApi(Jornal, $"Jornal/{Jornal.Id}");
                await Inicializar();
                Jornal = null;
            }
    }

    protected override void Editar()
    {
        eventAggregator.GetEvent<PubSubEvent<JornalDto>>().Publish(Jornal);
        base.Editar();
        eventAggregator.GetEvent<BoolAgreggator>().Publish(new PopUp(btnDialogText, MostrarCrearObra, ControlesDialog));
    }
    protected override void Eliminar()
    {
        eventAggregator.GetEvent<PubSubEvent<JornalDto>>().Publish(Jornal);
        base.Eliminar();
        eventAggregator.GetEvent<BoolAgreggator>().Publish(new PopUp(btnDialogText, MostrarCrearObra, ControlesDialog));
    }
    protected override void Nuevo()
    {
        base.Nuevo();
        Jornal = new JornalDto();
        eventAggregator.GetEvent<BoolAgreggator>().Publish(new PopUp(btnDialogText, MostrarCrearObra, ControlesDialog));
    }
    protected override void Cancelar()
    {
        base.Cancelar();
        Jornal = null;
        eventAggregator.GetEvent<BoolAgreggator>().Publish(new PopUp(btnDialogText, MostrarCrearObra, ControlesDialog));
    }


}
}
