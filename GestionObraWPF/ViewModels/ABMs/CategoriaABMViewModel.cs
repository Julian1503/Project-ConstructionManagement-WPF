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
    public class CategoriaABMViewModel : BaseABMViewModel
    {
        private CategoriaDto _categoria;
        public CategoriaDto Categoria { get { return _categoria; } set { SetProperty(ref _categoria, value); } }

        private ObservableCollection<CategoriaDto> _categorias;
        public ObservableCollection<CategoriaDto> Categorias { get { return _categorias; } set { SetProperty(ref _categorias, value); } }

        private async void Buscando()
        {
            if (string.IsNullOrWhiteSpace(Busqueda))
            {
                Categorias = new ObservableCollection<CategoriaDto>(await Servicios.ApiProcessor.GetApi<CategoriaDto[]>("Categoria/GetAll"));

            }
            else
            {
                Categorias = new ObservableCollection<CategoriaDto>(await Servicios.ApiProcessor.GetApi<CategoriaDto[]>($"Categoria/GetByFilter/{Busqueda}"));
            }
        }

        public override async Task Inicializar()
        {
            Categorias = new ObservableCollection<CategoriaDto>(await Servicios.ApiProcessor.GetApi<CategoriaDto[]>("Categoria/GetAll"));
        }
        protected async override Task CrearNuevoElemento()
        {
            if (!string.IsNullOrWhiteSpace(Categoria.Descripcion))
            {
                await Servicios.ApiProcessor.PostApi(Categoria, "Categoria/Insert");
                await Inicializar();
                Categoria = null;
                Categoria = new CategoriaDto();
            }
        }
        protected async override Task EliminarElemento()
        {
            await Servicios.ApiProcessor.DeleteApi($"Categoria/{Categoria.Id}");
            await Inicializar();
        }
        protected async override Task EditarElemento()
        {
            if (!string.IsNullOrWhiteSpace(Categoria.Descripcion))
            {
                await Servicios.ApiProcessor.PutApi(Categoria, $"Categoria/{Categoria.Id}");
                await Inicializar();
            }
        }

        protected override void Nuevo()
        {
            base.Nuevo();
            Categoria = new CategoriaDto();
        }
        protected override void Cancelar()
        {
            base.Cancelar();
            Categoria = null;
        }
        public CategoriaABMViewModel()
        {
            Categoria = null;
            Buscar = new DelegateCommand(Buscando);
            CrearObraCommand = new DelegateCommand(Nuevo);
            CancelarCommand = new DelegateCommand(Cancelar);
            EditarObraCommand = new DelegateCommand(Editar, () => ObjetoNull.IsNull(Categoria)).ObservesProperty(() => Categoria);
            EliminarObraCommand = new DelegateCommand(Eliminar, () => ObjetoNull.IsNull(Categoria)).ObservesProperty(() => Categoria);
        }
    }
}
