using GestionObraWPF.DTOs;
using GestionObraWPF.Helpers;
using GestionObraWPF.Model;
using GestionObraWPF.Servicios;
using GestionObraWPF.ViewModels.ABMs;
using Prism.Commands;
using Prism.Regions;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace GestionObraWPF.ViewModels
{
    public class JornalDetalleViewModel : BaseABMViewModel, INavigationAware
    {
        private JornalDto _jornal;
        public JornalDto Jornal { get { return _jornal; } set { SetProperty(ref _jornal, value); } }
        private ObservableCollection<JornalMaterialDto> _jornalMateriales;
        public bool MostrarAsistencia { get { return _mostrarAsistencia; } set { SetProperty(ref _mostrarAsistencia, value); } }
        public bool Asistio
        {
            get { return _asistio; }
            set
            {
                SetProperty(ref _asistio, value);
                if (Asistio)
                {
                    Asistencia = new AsistenciaDiariaDto();
                    Visibilidad = Visibility.Collapsed;
                }
                else
                {
                    Asistencia = new AsistenciaDiariaDto();
                    Visibilidad = Visibility.Visible;
                }
            }
        }

        public ObservableCollection<ContratistaDto> Contratistas { get { return _contratista; } set { SetProperty(ref _contratista, value); } }

        public ObservableCollection<MaterialDto> MaterialesPorObra { get { return _materialesPorObra; } set { SetProperty(ref _materialesPorObra, value); } }

        public ObservableCollection<CausaFaltaDto> Causas { get { return _causas; } set { SetProperty(ref _causas, value); } }

        public ObservableCollection<JornalMaterialDto> JornalMateriales { get { return _jornalMateriales; } set { SetProperty(ref _jornalMateriales, value); } }

        public ObservableCollection<AsistenciaDiariaDto> Asistencias { get { return _asistenciaDiaria; } set { SetProperty(ref _asistenciaDiaria, value); } }

        public ObservableCollection<AsistenciaContratistaDto> AsistenciaContratistas { get { return _asistenciaContratistas; } set { SetProperty(ref _asistenciaContratistas, value); } }

        private ObservableCollection<EmpleadoDto> _empleados;
        private ObservableCollection<MaterialDto> _materialesPorObra;
        private JornalMaterialDto _jornalMaterial;
        private bool _mostrarAsistencia;
        private ObservableCollection<AsistenciaDiariaDto> _asistenciaDiaria;
        private bool _asistio;
        private ObservableCollection<CausaFaltaDto> _causas;
        private AsistenciaContratistaDto _asistenciaContratista;
        private bool _mostrarContratista;
        private AsistenciaDiariaDto _asistencia;
        private ObservableCollection<AsistenciaContratistaDto> _asistenciaContratistas;
        private ObservableCollection<ContratistaDto> _contratista;

        public AsistenciaContratistaDto AsistenciaContratista { get { return _asistenciaContratista; } set { SetProperty(ref _asistenciaContratista, value); } }
        public ObservableCollection<EmpleadoDto> Empleados { get { return _empleados; } set { SetProperty(ref _empleados, value); } }
        public JornalMaterialDto JornalMaterial { get { return _jornalMaterial; } set { SetProperty(ref _jornalMaterial, value); } }
        public AsistenciaDiariaDto Asistencia { get { return _asistencia; } set { SetProperty(ref _asistencia, value); } }

        public ICommand CrearContratistaCommand { get; set; }
        public ICommand CrearContratistaNuevo { get; set; }
        public ICommand CancelarContratistaCommand { get; }
        public ICommand EditarContratistaCommand { get; set; }
        public ICommand CrearAsistenciaCommand { get; set; }
        public ICommand EliminarContratistaCommand { get; set; }
        public ICommand EditarAsistenciaCommand { get; set; }
        public ICommand EliminarAsistenciaCommand { get; set; }
        public ICommand CrearCommando { get; set; }
        public ICommand CancelarAsistenciaCommand { get; set; }
        public Visibility Visibilidad { get; set; }
        public bool MostrarContratista { get { return _mostrarContratista; } set { SetProperty(ref _mostrarContratista, value); } }

        public JornalDetalleViewModel()
        {
            Asistencia = null;
            JornalMaterial = null;
            EditarContratistaCommand = new DelegateCommand(EditarContratista, () => ObjetoNull.IsNull(AsistenciaContratista)).ObservesProperty(() => AsistenciaContratista);
            CrearContratistaCommand = new DelegateCommand(CrearContratista);
            CrearContratistaNuevo = new DelegateCommand(AbrirPopUpContratista);
            CancelarContratistaCommand = new DelegateCommand(CancelarContratista);
            EliminarContratistaCommand = new DelegateCommand(EliminarContratista, () => ObjetoNull.IsNull(AsistenciaContratista)).ObservesProperty(() => AsistenciaContratista);
            EditarAsistenciaCommand = new DelegateCommand(EditarAsistencia, () => ObjetoNull.IsNull(Asistencia)).ObservesProperty(() => Asistencia);
            EliminarAsistenciaCommand = new DelegateCommand(EliminarAsistencia, () => ObjetoNull.IsNull(Asistencia)).ObservesProperty(() => Asistencia);
            CancelarAsistenciaCommand = new DelegateCommand(CancelarAsistencia);
            CrearCommando = new DelegateCommand(NuevoAsistencia);
            CrearAsistenciaCommand = new DelegateCommand(CrearAsistencia);
            CrearObraCommand = new DelegateCommand(Nuevo);
            CancelarCommand = new DelegateCommand(Cancelar);
            EditarObraCommand = new DelegateCommand(Editar, () => ObjetoNull.IsNull(JornalMaterial)).ObservesProperty(() => JornalMaterial);
            EliminarObraCommand = new DelegateCommand(Eliminar, () => ObjetoNull.IsNull(JornalMaterial)).ObservesProperty(() => JornalMaterial);
        }

        private async void CancelarContratista()
        {
            MostrarContratista = false;
            await Inicializar();
            AsistenciaContratista = null;
        }

        private void AbrirPopUpContratista()
        {
            MostrarContratista = true;
            btnDialogText = "Crear";
            ControlesDialog = true;
            AsistenciaContratista = new AsistenciaContratistaDto();
        }

        private async void CrearContratista()
        {
            try
            {
                switch (btnDialogText)
                {
                    case "Editar":
                        MostrarContratista = false;
                        await EditarContratistaApi();
                        break;

                    case "Eliminar":
                        MostrarContratista = false;
                        await EliminarContratistapi();
                        break;

                    case "Crear":
                        await CrearNuevoContratistaApi();
                        break;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error de conexion");
            }
        }

        private async Task EditarContratistaApi()
        {
            if (AsistenciaContratista.Contratista != null)
            {
                await ApiProcessor.PutApi(AsistenciaContratista, $"AsistenciaContratista/{AsistenciaContratista.Id}");
                await Inicializar();
            }
        }

        private async Task EliminarContratistapi()
        {
            await ApiProcessor.DeleteApi($"AsistenciaContratista/{AsistenciaContratista.Id}");
            await Inicializar();
        }

        private async Task CrearNuevoContratistaApi()
        {
            if (AsistenciaContratista.Contratista != null)
            {
                AsistenciaContratista.ContratistaId = AsistenciaContratista.Contratista.Id;
                AsistenciaContratista.JornalId = Jornal.Id;
                await ApiProcessor.PostApi(AsistenciaContratista, "AsistenciaContratista/Insert");
                await Inicializar();
                AsistenciaContratista = null;
                AsistenciaContratista = new AsistenciaContratistaDto();
            }
        }

        private void EditarContratista()
        {
            MostrarContratista = true;
            btnDialogText = "Editar";
            ControlesDialog = true;
        }

        private void EliminarContratista()
        {
            MostrarContratista = true;
            btnDialogText = "Eliminar";
            ControlesDialog = false;
        }

        private async void CrearAsistencia()
        {
            try
            {
                switch (btnDialogText)
                {
                    case "Editar":
                        MostrarAsistencia = false;
                        await EditarAsistenciaApi();
                        break;

                    case "Eliminar":
                        MostrarAsistencia = false;
                        await EliminarAsistenciaApi();
                        break;

                    case "Crear":
                        await CrearNuevoAsistenciaApi();
                        break;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error de conexion");
            }
        }


        private async Task EditarAsistenciaApi()
        {
            if (Asistencia.Entrada < Asistencia.Salida)
            {
                if (Asistencia.Empleado != null)
                {
                    if (!Asistencia.Asistio)
                    {
                        Asistencia.CausaId = Asistencia.Causa.Id;
                    }
                    Asistencia.EmpleadoId = Asistencia.Empleado.Id;
                    Asistencia.JornalId = Jornal.Id;
                    await ApiProcessor.PutApi(Asistencia, $"AsistenciaDiaria/{Asistencia.Id}");
                    await Inicializar();
                }
            }
            else
            {
                MessageBox.Show("La hora de entrada debe ser menor a la de salida");
            }
        }

        private async Task EliminarAsistenciaApi()
        {
            await ApiProcessor.DeleteApi($"AsistenciaDiaria/{Asistencia.Id}");
            await Inicializar();
        }

        private async Task CrearNuevoAsistenciaApi()
        {
            if (Asistencia.Entrada < Asistencia.Salida || !Asistencia.Asistio)
            {
                if (Asistencia.Empleado != null)
                {
                    if (!Asistencia.Asistio)
                    {
                        Asistencia.CausaId = Asistencia.Causa.Id;
                    }
                    Asistencia.EmpleadoId = Asistencia.Empleado.Id;
                    Asistencia.JornalId = Jornal.Id;
                    await ApiProcessor.PostApi(Asistencia, "AsistenciaDiaria/Insert");
                    await Inicializar();
                    Asistencia = null;
                    Asistencia = new AsistenciaDiariaDto();
                    Asistio = false;
                }
            }
            else
            {
                MessageBox.Show("La hora de entrada debe ser menor a la de salida");
            }
        }

        protected async override Task EliminarElemento()
        {
            await ApiProcessor.DeleteApi($"JornalMaterial/{JornalMaterial.Id}");
            StockHelper.AgregarStock(JornalMaterial.MaterialId, JornalMaterial.CantidadUsado);
            await Inicializar();
        }
        protected async override Task EditarElemento()
        {
            if (JornalMaterial.Material != null && JornalMaterial.CantidadUsado > 0)
            {
                await ApiProcessor.PutApi(JornalMaterial, $"JornalMaterial/{JornalMaterial.Id}");
                await Inicializar();

            }
        }
        protected async override Task CrearNuevoElemento()
        {
            if (await StockHelper.ConsultarStock(JornalMaterial.Material.Id, JornalMaterial.CantidadUsado))
            {
                if (JornalMaterial.Material != null && JornalMaterial.CantidadUsado > 0)
                {
                    JornalMaterial.JornalId = Jornal.Id;
                    JornalMaterial.MaterialId = JornalMaterial.Material.Id;
                    await ApiProcessor.PostApi(JornalMaterial, "JornalMaterial/Insert");
                    StockHelper.QuitarStock(JornalMaterial.MaterialId, JornalMaterial.CantidadUsado);
                    await Inicializar();
                    JornalMaterial = null;
                    JornalMaterial = new JornalMaterialDto();
                }
            }
            else
            {
                MessageBox.Show("Material sin stock");
            }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }

        protected void EliminarAsistencia()
        {
            MostrarAsistencia = true;
            btnDialogText = "Eliminar";
            ControlesDialog = false;
        }

        protected async void CancelarAsistencia()
        {
            MostrarAsistencia = false;
            await Inicializar(); 
            Asistencia = new AsistenciaDiariaDto();
            Asistio = false;
        }

        protected void EditarAsistencia()
        {
            MostrarAsistencia = true;
            btnDialogText = "Editar";
            ControlesDialog = true;
        }

        protected void NuevoAsistencia()
        {
            MostrarAsistencia = true;
            btnDialogText = "Crear";
            ControlesDialog = true;
            Asistencia = new AsistenciaDiariaDto();
        }

        protected override void Nuevo()
        {
            base.Nuevo();
            JornalMaterial = new JornalMaterialDto();
        }
        protected override void Cancelar()
        {
            base.Cancelar();
            JornalMaterial = null;
        }
        public async void OnNavigatedTo(NavigationContext navigationContext)
        {
            Jornal = (JornalDto)navigationContext.Parameters["Jornal"];
            await Inicializar();
        }

        public override async Task Inicializar()
        {
            Contratistas = new ObservableCollection<ContratistaDto>(await ApiProcessor.GetApi<ContratistaDto[]>($"Contratista/GetAll"));
            MaterialesPorObra = new ObservableCollection<MaterialDto>(await ApiProcessor.GetApi<MaterialDto[]>($"Material/GetMateriales"));
            Causas = new ObservableCollection<CausaFaltaDto>(await ApiProcessor.GetApi<CausaFaltaDto[]>($"CausaFalta/GetAll"));
            Empleados = new ObservableCollection<EmpleadoDto>(await ApiProcessor.GetApi<EmpleadoDto[]>($"Empleado/GetAll"));
            JornalMateriales = new ObservableCollection<JornalMaterialDto>(await ApiProcessor.GetApi<JornalMaterialDto[]>($"JornalMaterial/GetByJornal/{Jornal.Id}"));
            Asistencias = new ObservableCollection<AsistenciaDiariaDto>(await ApiProcessor.GetApi<AsistenciaDiariaDto[]>($"AsistenciaDiaria/GetByJornal/{Jornal.Id}"));
            AsistenciaContratistas = new ObservableCollection<AsistenciaContratistaDto>(await ApiProcessor.GetApi<AsistenciaContratistaDto[]>($"AsistenciaContratista/GetByJornal/{Jornal.Id}"));
        }
    }
}
