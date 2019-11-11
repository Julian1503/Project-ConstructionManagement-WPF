using GestionObraWPF.DTOs;
using GestionObraWPF.Model;
using GestionObraWPF.Servicios;
using GestionObraWPF.ViewModels.EventAgreggator;
using GestionObraWPF.Views.ViewControls.Obra;
using Prism.Commands;
using Prism.Events;
using Prism.Regions;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace GestionObraWPF.ViewModels
{
    public class ObraDetalleViewModel : BaseViewModel, INavigationAware
    {
        private IngresoMaterialDto _ingresoMaterial;
        private bool _mostrar = false;
        private TareaDto _tarea;
        private ObservableCollection<TareaDto> _tareas;
        private ObservableCollection<DescripcionTareaDto> _descripcionTareas;
        private ObservableCollection<EmpresaDto> _propietarios;
        private ObservableCollection<MaterialDto> _materiales;
        private ObservableCollection<IngresoMaterialDto> _ingresoMateriales;
        private ObraDto _obra;
        private string _botonTarea;
        private bool _bloquearControlesTarea;
        private bool _bloquearControlesMaterial;
        private TimeSpan _tiempoEmpleado = new TimeSpan(0,0,0);
        private bool _mostrarFinalizarTarea;
        private string _botonMaterial;
        private ObservableCollection<MaterialDto> _materialesPorObra;
        public ObservableCollection<ObraDto> Obras { get { return _obras; } set { SetProperty(ref _obras, value); } }
        public ObservableCollection<EmpleadoDto> Encargados { get { return _encargados; } set { SetProperty(ref _encargados, value); } }
        public ObservableCollection<MaterialDto> MaterialesPorObra { get { return _materialesPorObra; } set { SetProperty(ref _materialesPorObra, value); } }
        public ObraDto Obra { get { return _obra; } set { SetProperty(ref _obra, value); } }
        public bool MostrarPopMaterial { get { return _mostrar; } set { SetProperty(ref _mostrar, value); } }

        public bool MostrarPopUpJornal { get { return _mostrarPopUpJornal; } set { SetProperty(ref _mostrarPopUpJornal, value); } }

        public string BotonJornal { get { return _botonJornal; } set { SetProperty(ref _botonJornal, value); } }

        public string BotonMaterial { get { return _botonMaterial; } set { SetProperty(ref _botonMaterial, value); } }

        public bool BloquearControlesJornal { get { return _bloquearControlesJornal; } set { SetProperty(ref _bloquearControlesJornal, value); } }

        public bool BloquearControlesMaterial { get { return _bloquearControlesMaterial; } set { SetProperty(ref _bloquearControlesMaterial, value); } }

        public double TiempoEstimado { get { return _tiempoEstimado; } set { SetProperty(ref _tiempoEstimado, value); } }
        public double TiempoReal { get { return _tiempoReal; } set { SetProperty(ref _tiempoReal, value); } }
        public double TiempoPerdidaGanancia { get { return _tiempoPerdidaGanancia; } set { SetProperty(ref _tiempoPerdidaGanancia, value); } }

        public TimeSpan TiempoEmpleado { get { return _tiempoEmpleado; } set { SetProperty(ref _tiempoEmpleado, value); } }
        public bool MostrarPopTarea { get { return _mostrar; } set { SetProperty(ref _mostrar, value); } }
        public IngresoMaterialDto IngresoMaterial { get { return _ingresoMaterial; } set { SetProperty(ref _ingresoMaterial, value); } }
        public TareaDto Tarea { get { return _tarea; } set { SetProperty(ref _tarea, value); } }
        public ObservableCollection<TareaDto> Tareas { get { return _tareas; } set { SetProperty(ref _tareas, value); } }
        public ObservableCollection<DescripcionTareaDto> DescripcionTareas { get { return _descripcionTareas; } set { SetProperty(ref _descripcionTareas, value); } }
        public ObservableCollection<IngresoMaterialDto> IngresoMateriales { get { return _ingresoMateriales; } set { SetProperty(ref _ingresoMateriales, value); } }
        public ObservableCollection<MaterialDto> Materiales { get { return _materiales; } set { SetProperty(ref _materiales, value); } }
        public ObservableCollection<EmpresaDto> Propietarios { get { return _propietarios; } set { SetProperty(ref _propietarios, value); } }
        public int CantidadUsado { get { return _cantidadUsado; } set { SetProperty(ref _cantidadUsado, value); } }
        public bool MostrarPopSalidaMaterial { get { return _mostrarPopSalidaMaterial; } set { SetProperty(ref _mostrarPopSalidaMaterial, value); } }
        public string BotonSalidaMaterial { get { return _botonSalidaMaterial; } set { SetProperty(ref _botonSalidaMaterial, value); } }
        public bool BloquearControlesSalidaMaterial { get { return _bloquearControlesSalidaMaterial; } set { SetProperty(ref _bloquearControlesSalidaMaterial, value); } }
        public SalidaMaterialDto SalidaMaterial { get { return _salidaMaterial; } set { SetProperty(ref _salidaMaterial, value); RaisePropertyChanged("SalidaMaterial.Material"); } }

        private IRegionManager regionManager { get; set; }
        public IEventAggregator eventAggregator { get; set; }
        public ICommand IngresarJornalCommand { get; }

        //Material
        public ICommand IngresarMaterialesCommand { get; set; }
        public ICommand CancelarJornalCommand { get; }
        public ICommand CancelarTareaCommand { get; set; }
        public ICommand CancelarMaterialUsado { get; set; }
        public ICommand CancelarMaterialEditar { get; set; }
        public ICommand Atras { get; set; }
        public ICommand MaterialUsadoCommand { get; set; }
        public ICommand CancelarSalidaMaterial { get; set; }
        public ICommand CrearSalidaMaterial { get; set; }
        public ICommand EditarMaterialUsadoCommand { get; set; }
        public ICommand FinalizarTareaCommand { get; set; }
        public ICommand CancelarFinalizarTareaCommand { get; set; }
        public ICommand IngresarTareaCommand { get; set; }
        public ICommand MostrarCommand { get; set; }
        public string BotonTarea { get { return _botonTarea; } set { SetProperty(ref _botonTarea, value); } }
        public bool BloquearControlesTarea { get { return _bloquearControlesTarea; } set { SetProperty(ref _bloquearControlesTarea, value); } }
        public double DuracionObra
        {
            get { return _duracionObra; }
            set
            {
                SetProperty(ref _duracionObra, value);
                if (value == 100)
                {
                    TiempoResultado = Visibility.Visible;
                }
                else
                {
                    TiempoResultado = Visibility.Collapsed;
                }
            }
        }
        public string TextoPerdidaGanancia { get { return _textoPerdidaGanancia; } set { SetProperty(ref _textoPerdidaGanancia, value); } }
        public int TareasTotal { get { return _tareasTotal; } set { SetProperty(ref _tareasTotal, value); } }
        public int TareasCompletadas { get { return _tareasCompletadas; } set { SetProperty(ref _tareasCompletadas, value); } }
        public int TareasFaltantes { get { return _tareasFaltantes; } set { SetProperty(ref _tareasFaltantes, value); } }
        public bool MostrarFinalizarTarea { get { return _mostrarFinalizarTarea; } set { SetProperty(ref _mostrarFinalizarTarea, value); } }
        public bool MostrarEditarMaterialesUsados { get { return _mostrarEditarMaterialesUsados; } set { SetProperty(ref _mostrarEditarMaterialesUsados, value); } }
        public bool MostrarMaterialesUsados { get { return _mostrarMaterialesUsados; } set { SetProperty(ref _mostrarMaterialesUsados, value); } }
        private bool _tareaTab;
        public Visibility TiempoResultado { get { return _tiempoResultado; } set { SetProperty(ref _tiempoResultado, value); } }
        public bool TareaTab
        {
            get { return _tareaTab; }
            set
            {
                SetProperty(ref _tareaTab, value);
                if (value)
                {
                    eventAggregator.GetEvent<BoolAgreggator>().Subscribe(GetAbrirTareaPopUp);
                    eventAggregator.GetEvent<TareaAgreggator>().Subscribe(PasandoTarea);
                    eventAggregator.GetEvent<PubSubEvent<bool>>().Unsubscribe(AbrirMaterialesUsados);
                    eventAggregator.GetEvent<PubSubEvent<bool>>().Subscribe(AbrirFinalizarTarea);
                    eventAggregator.GetEvent<PubSubEvent<int>>().Subscribe(Refrescar);

                }
                else
                {
                    eventAggregator.GetEvent<PubSubEvent<int>>().Unsubscribe(Refrescar);
                    eventAggregator.GetEvent<BoolAgreggator>().Unsubscribe(GetAbrirTareaPopUp);
                    eventAggregator.GetEvent<TareaAgreggator>().Unsubscribe(PasandoTarea);
                    eventAggregator.GetEvent<PubSubEvent<bool>>().Unsubscribe(AbrirFinalizarTarea);
                }
            }
        }
        private bool _mostrarMaterialesUsados;
        private int _cantidadUsado;
        private bool _mostrarEditarMaterialesUsados;
        private ObservableCollection<ObraDto> _obras;

        private bool _materialTab;
        public bool MaterialTab
        {
            get { return _materialTab; }
            set
            {
                SetProperty(ref _materialTab, value);
                if (value)
                {
                    eventAggregator.GetEvent<PubSubEvent<PopUp>>().Subscribe(GetAbrirMaterialPopUp);
                    eventAggregator.GetEvent<PubSubEvent<PopUp>>().Unsubscribe(GetAbrirSalidaMaterial);
                    eventAggregator.GetEvent<IngresoMaterialAggregator>().Subscribe(PasandoMaterial);
                    eventAggregator.GetEvent<PubSubEvent<bool>>().Subscribe(AbrirMaterialesUsados);
                    eventAggregator.GetEvent<BulAgreggator>().Subscribe(AbrirEditarMaterialesUsados);
                    eventAggregator.GetEvent<PubSubEvent<bool>>().Unsubscribe(AbrirFinalizarTarea);
                }
                else
                {
                    eventAggregator.GetEvent<PubSubEvent<bool>>().Unsubscribe(AbrirMaterialesUsados);
                    eventAggregator.GetEvent<PubSubEvent<PopUp>>().Unsubscribe(GetAbrirMaterialPopUp);
                    eventAggregator.GetEvent<BulAgreggator>().Unsubscribe(AbrirEditarMaterialesUsados);
                    eventAggregator.GetEvent<IngresoMaterialAggregator>().Unsubscribe(PasandoMaterial);
                }
            }
        }
        private bool _jornalTab;
        public bool JornalTab
        {
            get { return _jornalTab; }
            set
            {
                SetProperty(ref _jornalTab, value);
                if (value)
                {
                    eventAggregator.GetEvent<BoolAgreggator>().Unsubscribe(GetAbrirTareaPopUp);
                    eventAggregator.GetEvent<BoolAgreggator>().Subscribe(GetAbrirJornalPopUp);
                    eventAggregator.GetEvent<PubSubEvent<JornalDto>>().Subscribe(PasandoJornal);
                }
                else
                {
                    eventAggregator.GetEvent<BoolAgreggator>().Unsubscribe(GetAbrirJornalPopUp);
                    eventAggregator.GetEvent<PubSubEvent<JornalDto>>().Unsubscribe(PasandoJornal);
                }
            }
        }

        private void PasandoJornal(JornalDto obj)
        {
            Jornal = obj;
        }

        private void GetAbrirJornalPopUp(PopUp obj)
        {
            MostrarPopUpJornal = obj.ShowPopUp;
            BotonJornal = obj.ButtonTitle;
            BloquearControlesJornal = obj.ControlersEnable;
            if (BotonJornal == "Crear")
            {
                Jornal = new JornalDto();
            }
        }

        public JornalDto Jornal { get{ return _jornal; } set { SetProperty(ref _jornal, value); } }
        private bool _materialSalidaTab;
        private ObservableCollection<EmpleadoDto> _encargados;
        private bool _bloquearControlesSalidaMaterial;
        private string _botonSalidaMaterial;
        private bool _mostrarPopSalidaMaterial;
        private SalidaMaterialDto _salidaMaterial;
        private double _duracionObra;
        private int _tareasTotal;
        private int _tareasCompletadas;
        private int _tareasFaltantes;
        private Visibility _tiempoResultado;
        private double _tiempoEstimado;
        private double _tiempoReal;
        private double _tiempoPerdidaGanancia;
        private string _textoPerdidaGanancia;
        private bool _bloquearControlesJornal;
        private bool _mostrarPopUpJornal;
        private string _botonJornal;
        private JornalDto _jornal;

        public bool MaterialSalidaTab
        {
            get { return _materialSalidaTab; }
            set
            {
                SetProperty(ref _materialSalidaTab, value);
                if (value)
                {
                    eventAggregator.GetEvent<PubSubEvent<PopUp>>().Unsubscribe(GetAbrirMaterialPopUp);
                    eventAggregator.GetEvent<PubSubEvent<PopUp>>().Subscribe(GetAbrirSalidaMaterial);
                    eventAggregator.GetEvent<PubSubEvent<SalidaMaterialDto>>().Subscribe(ObtenerSalidaMaterial);
                }
                else
                {
                    eventAggregator.GetEvent<PubSubEvent<SalidaMaterialDto>>().Unsubscribe(ObtenerSalidaMaterial);
                    eventAggregator.GetEvent<PubSubEvent<PopUp>>().Unsubscribe(GetAbrirSalidaMaterial);

                }
            }
        }


        private void ObtenerSalidaMaterial(SalidaMaterialDto obj)
        {
            SalidaMaterial = obj;
        }

        private void GetAbrirSalidaMaterial(PopUp obj)
        {
            MostrarPopSalidaMaterial = obj.ShowPopUp;
            BotonSalidaMaterial = obj.ButtonTitle;
            BloquearControlesSalidaMaterial = obj.ControlersEnable;
            if (BotonSalidaMaterial == "Crear")
            {
                SalidaMaterial = new SalidaMaterialDto();
            }
        }

        private void AbrirEditarMaterialesUsados(bool obj)
        {
            MostrarEditarMaterialesUsados = obj;
        }


        private void CancelarEditar()
        {
            MostrarEditarMaterialesUsados = !MostrarEditarMaterialesUsados;
            CantidadUsado = 0;
            IngresoMaterial = null;
            eventAggregator.GetEvent<PubSubEvent<IngresoMaterialDto>>().Publish(IngresoMaterial);
        }

        private void AbrirMaterialesUsados(bool obj)
        {
            MostrarMaterialesUsados = obj;
        }

        public ObraDetalleViewModel(IEventAggregator eventAggregator, IRegionManager regionManager)
        {
            this.regionManager = regionManager;
            this.eventAggregator = eventAggregator;

            //Material
            IngresarJornalCommand = new DelegateCommand(IngresarJornal);
            CancelarJornalCommand = new DelegateCommand(CancelarJornal);
            IngresarTareaCommand = new DelegateCommand(IngresarTarea);
            MostrarCommand = new DelegateCommand(MostrarDialog);
            IngresarMaterialesCommand = new DelegateCommand(IngresarMateriales);
            CancelarTareaCommand = new DelegateCommand(Cancelar);
            CancelarMaterialUsado = new DelegateCommand(CancelarMaterial);
            CancelarMaterialEditar = new DelegateCommand(CancelarEditar);
            EditarMaterialUsadoCommand = new DelegateCommand(EditarMaterial);
            MaterialUsadoCommand = new DelegateCommand(MaterialUsado);
            CancelarSalidaMaterial = new DelegateCommand(CancelarSalida);
            CrearSalidaMaterial = new DelegateCommand(CrearSalida);
            #region FinalizarTarea
            FinalizarTareaCommand = new DelegateCommand(FinalizarTarea);
            CancelarFinalizarTareaCommand = new DelegateCommand(CancelarFinalizarTarea);
            #endregion
        }

        private void IngresarJornal()
        {
            if(Jornal.NumeroOrden>0)
            {
                eventAggregator.GetEvent<PubSubEvent<JornalDto>>().Publish(Jornal);
                Jornal = new JornalDto();
            }
        }

        private void CancelarJornal()
        {
            MostrarPopUpJornal = false;
            Jornal = null;
            eventAggregator.GetEvent<PubSubEvent<JornalDto>>().Publish(Jornal);
        }

        private void CrearSalida()
        {
            if ((SalidaMaterial.Material != null || SalidaMaterial.MaterialId != 0) && (SalidaMaterial.Encargado != null || SalidaMaterial.EncargadoId != 0) && (SalidaMaterial.ParaObra != null || SalidaMaterial.ParaObraId != 0) && SalidaMaterial.Cantidad > 0)
            {
                eventAggregator.GetEvent<SalidaMaterialAgreggator>().Publish(SalidaMaterial);
                SalidaMaterial = new SalidaMaterialDto();
            }
        }

        private void CancelarSalida()
        {
            MostrarPopSalidaMaterial = !MostrarPopSalidaMaterial;
            SalidaMaterial = null;
            eventAggregator.GetEvent<PubSubEvent<SalidaMaterialDto>>().Publish(SalidaMaterial);
        }

        private void MaterialUsado()
        {
            if (IngresoMaterial.Cantidad >= CantidadUsado+IngresoMaterial.CantidadDevuelta)
            {
                eventAggregator.GetEvent<PubSubEvent<int>>().Publish(CantidadUsado);
                CantidadUsado = 0;
            }
        }

        private void EditarMaterial()
        {
            if (IngresoMaterial.Cantidad >= CantidadUsado)
            {
                eventAggregator.GetEvent<PubSubEvent<int>>().Publish(CantidadUsado);
                CantidadUsado = 0;
            }
        }

        private void CancelarMaterial()
        {
            MostrarMaterialesUsados = !MostrarMaterialesUsados;
            CantidadUsado = 0;
            IngresoMaterial = null;
            eventAggregator.GetEvent<PubSubEvent<IngresoMaterialDto>>().Publish(IngresoMaterial);
        }

        public async Task Inicializar()
        {
            regionManager.RequestNavigate("RegionTarea", "Tarea");
            regionManager.RequestNavigate("RegionIngresoMaterial", "IngresoMaterial");
            regionManager.RequestNavigate("RegionSalidaMaterial", "SalidaMaterial");
            regionManager.RequestNavigate("RegionJornal", "Jornal");
            eventAggregator.GetEvent<ObraAgreggator>().Publish(Obra);
            Propietarios = new ObservableCollection<EmpresaDto>(await ApiProcessor.GetApi<EmpresaDto[]>("Empresa/GetAll"));
            DescripcionTareas = new ObservableCollection<DescripcionTareaDto>(await ApiProcessor.GetApi<DescripcionTareaDto[]>($"DescripcionTarea/GetAll"));
            Materiales = new ObservableCollection<MaterialDto>(await ApiProcessor.GetApi<MaterialDto[]>("Material/GetInsumos"));
            Tareas = new ObservableCollection<TareaDto>(await ApiProcessor.GetApi<TareaDto[]>($"Tarea/GetByObra/{Obra.Id}"));
            Obras = new ObservableCollection<ObraDto>(await ApiProcessor.GetApi<ObraDto[]>("Obra/GetAll"));
            Obras.Remove(Obras.FirstOrDefault(x => x.Id == Obra.Id));
            Encargados = new ObservableCollection<EmpleadoDto>(await ApiProcessor.GetApi<EmpleadoDto[]>("Empleado/GetAll"));
            MaterialesPorObra = new ObservableCollection<MaterialDto>(await ApiProcessor.GetApi<MaterialDto[]>($"IngresoMaterial/GetMaterialByObra/{Obra.Id}"));
            IngresoMateriales = new ObservableCollection<IngresoMaterialDto>(await ApiProcessor.GetApi<IngresoMaterialDto[]>($"IngresoMaterial/GetByObra/{Obra.Id}"));
            CalcularDuracion();
            CalcularTiempo();
            ImBuzy = false;
            if (Tareas.All(x => x.Estado == Constantes.EstadoTarea.Finalizada))
            {
                Obra.EstadoObra = Constantes.EstadoObra.Finalizada;
                await ApiProcessor.PutApi(Obra,$"Obra/{Obra.Id}");
            }
        }

        private async void CalcularTiempo()
        {
            TiempoEstimado = await ApiProcessor.GetApi<double>($"Tarea/TiempoEstimado/{Obra.Id}");
            TiempoReal = await ApiProcessor.GetApi<double>($"Tarea/TiempoEmpleado/{Obra.Id}");
            TiempoPerdidaGanancia = TiempoEstimado - TiempoReal;
            if (TiempoPerdidaGanancia < 0)
            {
                TextoPerdidaGanancia = "Tiempo de retraso :";
                TiempoPerdidaGanancia *= -1;
            }
            else
            {
                TextoPerdidaGanancia = "Tiempo adelantado :";
            }
        }

        private void Refrescar(int obj)
        {
            CalcularDuracion();
            CalcularTiempo();
        }

        private async void CalcularDuracion()
        {
            DuracionObra = await ApiProcessor.GetApi<double>($"Tarea/DuracionObra/{Obra.Id}");
            TareasCompletadas = await ApiProcessor.GetApi<int>($"Tarea/CantidadCompletadas/{Obra.Id}");
            TareasFaltantes = await ApiProcessor.GetApi<int>($"Tarea/CantidadFaltantes/{Obra.Id}");
            TareasTotal = TareasCompletadas + TareasFaltantes;
        }

        private void PasandoMaterial(IngresoMaterialDto obj)
        {
            IngresoMaterial = obj;
        }

        private void GetAbrirMaterialPopUp(PopUp obj)
        {
            MostrarPopMaterial = obj.ShowPopUp;
            BotonMaterial = obj.ButtonTitle;
            BloquearControlesMaterial = obj.ControlersEnable;
            if (BotonMaterial == "Crear")
            {
                IngresoMaterial = new IngresoMaterialDto();
            }
        }

        #region FinalizarTarea
        private void FinalizarTarea()
        {
            eventAggregator.GetEvent<PubSubEvent<TimeSpan>>().Publish(TiempoEmpleado);
            MostrarFinalizarTarea = false;
            TiempoEmpleado = new TimeSpan();
        }

        private void AbrirFinalizarTarea(bool obj)
        {
            MostrarFinalizarTarea = obj;
        }

        private void CancelarFinalizarTarea()
        {
            MostrarFinalizarTarea = false;
            TiempoEmpleado = new TimeSpan();
        }

        #endregion

        private void PasandoTarea(TareaDto obj)
        {
            Tarea = obj;
        }

        private void GetAbrirTareaPopUp(PopUp obj)
        {
            MostrarPopTarea = obj.ShowPopUp;
            BotonTarea = obj.ButtonTitle;
            BloquearControlesTarea = obj.ControlersEnable;
            if (BotonTarea == "Crear")
            {
                Tarea = new TareaDto();
            }
        }

        private void IngresarTarea()
        {
            if ((Tarea.DescripcionTarea != null || Tarea.DescripcionTareaId != 0) && Tarea.NumeroOrden != 0)
            {
                eventAggregator.GetEvent<PubSubEvent<TareaDto>>().Publish(Tarea);
                Tarea = new TareaDto();
            }
        }


        private void Cancelar()
        {
            MostrarPopTarea = !MostrarPopTarea;
            Tarea = null;
            eventAggregator.GetEvent<PubSubEvent<TareaDto>>().Publish(Tarea);
        }

        private void MostrarDialog()
        {
            MostrarPopMaterial = !MostrarPopMaterial;
            IngresoMaterial = new IngresoMaterialDto();
        }


        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }

        public async void OnNavigatedTo(NavigationContext navigationContext)
        {
            ImBuzy = true;
            Obra = (ObraDto)navigationContext.Parameters["Obra"];
            await Inicializar();
        }

        private void IngresarMateriales()
        {
            if ((IngresoMaterial.Material != null || IngresoMaterial.MaterialId != 0) && IngresoMaterial.Cantidad != 0)
            {
                eventAggregator.GetEvent<PubSubEvent<IngresoMaterialDto>>().Publish(IngresoMaterial);
                IngresoMaterial = new IngresoMaterialDto();
            }
        }
    }
}
