using GestionObraWPF.DTOs;
using GestionObraWPF.Helpers;
using GestionObraWPF.ViewModels.ABMs;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace GestionObraWPF.ViewModels
{
    public class EmpleadoABMViewModel : BaseABMViewModel
    {
        private EmpleadoDto _empleado;
        public EmpleadoDto Empleado { get { return _empleado; } set { SetProperty(ref _empleado, value); } }
        public ObservableCollection<CategoriaDto> Categorias { get { return _categorias; } set { SetProperty(ref _categorias, value); } }
        private ObservableCollection<EmpleadoDto> _Personas;
        private ObservableCollection<CategoriaDto> _categorias;

        public ObservableCollection<EmpleadoDto> Empleados { get { return _Personas; } set { SetProperty(ref _Personas, value); } }

        public override async Task Inicializar()
        {
            Empleados = new ObservableCollection<EmpleadoDto>(await Servicios.ApiProcessor.GetApi<EmpleadoDto[]>("Empleado/GetAll"));
            Categorias = new ObservableCollection<CategoriaDto>(await Servicios.ApiProcessor.GetApi<CategoriaDto[]>("Categoria/GetAll"));
        }
        protected async override Task CrearNuevoElemento()
        {
            if (!string.IsNullOrWhiteSpace(Empleado.ApYNom) && !string.IsNullOrWhiteSpace(Empleado.Dni) && (!string.IsNullOrWhiteSpace(Empleado.Celular) || !string.IsNullOrWhiteSpace(Empleado.Telefono)) && Empleado.FechaNacimiento != null)
            {
                Empleado.CategoriaId = Empleado.Categoria.Id;
                await Servicios.ApiProcessor.PostApi(Empleado, "Empleado/Insert");
                await Inicializar();
                Empleado = new EmpleadoDto();
            }
        }
        protected async override Task EliminarElemento()
        {
            await Servicios.ApiProcessor.DeleteApi($"Empleado/{Empleado.Id}");
            await Inicializar();
        }
        protected async override Task EditarElemento()
        {
            if (!string.IsNullOrWhiteSpace(Empleado.ApYNom) && !string.IsNullOrWhiteSpace(Empleado.Dni) && (!string.IsNullOrWhiteSpace(Empleado.Celular) || !string.IsNullOrWhiteSpace(Empleado.Telefono)) && Empleado.FechaNacimiento != null)
            {
                Empleado.CategoriaId = Empleado.Categoria.Id;
                await Servicios.ApiProcessor.PutApi(Empleado, $"Empleado/{Empleado.Id}");
                await Inicializar();
            }
        }
        private async void Buscando()
        {
            if (string.IsNullOrWhiteSpace(Busqueda))
            {
                Empleados = new ObservableCollection<EmpleadoDto>(await Servicios.ApiProcessor.GetApi<EmpleadoDto[]>("Empleado/GetAll"));

            }
            else
            {
                Empleados = new ObservableCollection<EmpleadoDto>(await Servicios.ApiProcessor.GetApi<EmpleadoDto[]>($"Empleado/GetByFilter/{Busqueda}"));
            }
        }
        protected override void Nuevo()
        {
            base.Nuevo();
            Empleado = new EmpleadoDto();
        }
        protected override void Cancelar()
        {
            base.Cancelar();
            Empleado = null;
        }
        public EmpleadoABMViewModel()
        {
            Buscar = new DelegateCommand(Buscando);
            Empleado = null;
            CrearObraCommand = new DelegateCommand(Nuevo);
            CancelarCommand = new DelegateCommand(Cancelar);
            EditarObraCommand = new DelegateCommand(Editar, () => ObjetoNull.IsNull(Empleado)).ObservesProperty(() => Empleado);
            EliminarObraCommand = new DelegateCommand(Eliminar, () => ObjetoNull.IsNull(Empleado)).ObservesProperty(() => Empleado);
        }
    }
}
