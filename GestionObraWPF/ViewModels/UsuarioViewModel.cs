using GestionObraWPF.DTOs;
using GestionObraWPF.Helpers;
using GestionObraWPF.Model;
using GestionObraWPF.ViewModels.ABMs;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace GestionObraWPF.ViewModels
{
    public class UsuarioViewModel : BaseABMViewModel
    {
        private UsuarioDto _usuario;
        public UsuarioDto Usuario { get { return _usuario; } set { SetProperty(ref _usuario, value); } }
        public IdentificacionDto Identificacion { get { return _identificacion; } set { SetProperty(ref _identificacion, value); } }

        private ObservableCollection<UsuarioDto> _usuarios;
        private ObservableCollection<EmpleadoDto> _empleados;
        private IdentificacionDto _identificacion;

        public ObservableCollection<EmpleadoDto> Empleados { get { return _empleados; } set { SetProperty(ref _empleados, value); } }

        public ObservableCollection<UsuarioDto> Usuarios { get { return _usuarios; } set { SetProperty(ref _usuarios, value); } }

        private async void Buscando()
        {
            if (string.IsNullOrWhiteSpace(Busqueda))
            {
                Usuarios = new ObservableCollection<UsuarioDto>(await Servicios.ApiProcessor.GetApi<UsuarioDto[]>("Usuario/GetAll"));

            }
            else
            {
                Usuarios = new ObservableCollection<UsuarioDto>(await Servicios.ApiProcessor.GetApi<UsuarioDto[]>($"Usuario/GetByFilter/{Busqueda}"));
            }
        }

        public override async Task Inicializar()
        {
            Empleados = new ObservableCollection<EmpleadoDto>(await Servicios.ApiProcessor.GetApi<EmpleadoDto[]>("Empleado/GetAll"));
            Usuarios = new ObservableCollection<UsuarioDto>(await Servicios.ApiProcessor.GetApi<UsuarioDto[]>("Usuario/GetAll"));
        }
        protected async override Task CrearNuevoElemento()
        {
            if (!string.IsNullOrWhiteSpace(Usuario.UserName) && !string.IsNullOrWhiteSpace(Usuario.Password) && Usuario.Empleado!=null)
            {
                var id = await Servicios.ApiProcessor.PostApi(Identificacion, "Identificacion/Insert");
                Usuario.IdentificacionId = long.Parse(id);
                Usuario.EmpleadoId = Usuario.Empleado.Id;
                Usuario.Password = Encriptar.Encriptador(Usuario.Password);
                await Servicios.ApiProcessor.PostApi(Usuario, "Usuario/Insert");
                await Inicializar();
                Usuario = null;
                Usuario = new UsuarioDto();
            }
            else
            {
                MessageBox.Show("Faltan llenar datos");
            }
        }
        protected async override Task EliminarElemento()
        {
            await Servicios.ApiProcessor.DeleteApi($"Usuario/{Usuario.Id}");
            await Inicializar();
        }
        protected async override Task EditarElemento()
        {
            if (!string.IsNullOrWhiteSpace(Usuario.UserName))
            {
                await Servicios.ApiProcessor.PutApi(Usuario, $"Usuario/{Usuario.Id}");
                await Inicializar();
            }
        }

        protected override void Nuevo()
        {
            base.Nuevo();
            Usuario = new UsuarioDto();
            Identificacion = new IdentificacionDto();
        }
        protected override void Cancelar()
        {
            base.Cancelar();
            Usuario = null;
            Identificacion = null;
        }
        public UsuarioViewModel()
        {
            Usuario = null;
            Buscar = new DelegateCommand(Buscando);
            CrearObraCommand = new DelegateCommand(Nuevo);
            CancelarCommand = new DelegateCommand(Cancelar);
            EditarObraCommand = new DelegateCommand(Editar, () => ObjetoNull.IsNull(Usuario)).ObservesProperty(() => Usuario);
            EliminarObraCommand = new DelegateCommand(Eliminar, () => ObjetoNull.IsNull(Usuario)).ObservesProperty(() => Usuario);
        }
    }
}
