using GestionObraWPF.DTOs;
using GestionObraWPF.Helpers;
using GestionObraWPF.Servicios;
using GestionObraWPF.ViewModels.ABMs;
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
    public class ProveedorABMViewModel : BaseABMViewModel
    {
        private ProveedorDto _proveedor;
        private ObservableCollection<ProveedorDto> _proveedores;
        private ObservableCollection<CondicionIvaDto> _condicionesIva;

        public ProveedorDto Proveedor { get { return _proveedor; } set { SetProperty(ref _proveedor, value); } }
        public ObservableCollection<ProveedorDto> Proveedores { get { return _proveedores; } set { SetProperty(ref _proveedores, value); } }
        public ObservableCollection<CondicionIvaDto> CondicionesIva { get { return _condicionesIva; } set { SetProperty(ref _condicionesIva, value); } }
   

        public override async Task Inicializar()
        {
            Proveedores = new ObservableCollection<ProveedorDto>(await Servicios.ApiProcessor.GetApi<ProveedorDto[]>("Proveedor/GetAll"));
            CondicionesIva = new ObservableCollection<CondicionIvaDto>(await Servicios.ApiProcessor.GetApi<CondicionIvaDto[]>("CondicionIva/GetAll"));
        }
        protected async override Task CrearNuevoElemento()
        {
            if (!string.IsNullOrWhiteSpace(Proveedor.RazonSocial) && !string.IsNullOrWhiteSpace(Proveedor.NombreFantasia))
            {
                Proveedor.CondicionIvaId = Proveedor.CondicionIva.Id;
                await Servicios.ApiProcessor.PostApi(Proveedor, "Proveedor/Insert");
                await Inicializar();
                Proveedor = new ProveedorDto();
            }
        }
        protected async override Task EliminarElemento()
        {
            await Servicios.ApiProcessor.DeleteApi($"Proveedor/{Proveedor.Id}");
            await Inicializar();
        }
        protected async override Task EditarElemento()
        {
            if (!string.IsNullOrWhiteSpace(Proveedor.RazonSocial) && !string.IsNullOrWhiteSpace(Proveedor.NombreFantasia))
            {
                Proveedor.CondicionIvaId = Proveedor.CondicionIva.Id;
                await Servicios.ApiProcessor.PutApi(Proveedor, $"Proveedor/{Proveedor.Id}");
                await Inicializar();
            }
        }
        private async void Buscando()
        {
            if (string.IsNullOrWhiteSpace(Busqueda))
            {
                Proveedores = new ObservableCollection<ProveedorDto>(await Servicios.ApiProcessor.GetApi<ProveedorDto[]>("Proveedor/GetAll"));

            }
            else
            {
                Proveedores = new ObservableCollection<ProveedorDto>(await Servicios.ApiProcessor.GetApi<ProveedorDto[]>($"Proveedor/GetByFilter/{Busqueda}"));
            }
        }
        protected override void Nuevo()
        {
            base.Nuevo();
            Proveedor = new ProveedorDto();
        }
        protected override void Cancelar()
        {
            base.Cancelar();
            Proveedor = null;
        }
       
        public ProveedorABMViewModel()
        {
            Proveedor = null;
            Buscar = new DelegateCommand(Buscando);
            CrearObraCommand = new DelegateCommand(Nuevo);
            CancelarCommand = new DelegateCommand(Cancelar);
            EditarObraCommand = new DelegateCommand(Editar, () => ObjetoNull.IsNull(Proveedor)).ObservesProperty(() => Proveedor);
            EliminarObraCommand = new DelegateCommand(Eliminar, () => ObjetoNull.IsNull(Proveedor)).ObservesProperty(() => Proveedor);
        }
    }
}
