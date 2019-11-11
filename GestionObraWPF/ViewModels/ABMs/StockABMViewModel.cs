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

namespace GestionObraWPF.ViewModels
{
    public class StockABMViewModel : BaseABMViewModel
    {
        private StockDto _stock;
        public StockDto Stock { get { return _stock; } set { SetProperty(ref _stock, value); } }

        private ObservableCollection<StockDto> _stocks;
        private ObservableCollection<MaterialDto> _materiales;

        public ObservableCollection<StockDto> Stocks { get { return _stocks; } set { SetProperty(ref _stocks, value); } }
        public ObservableCollection<MaterialDto> Materiales { get { return _materiales; } set { SetProperty(ref _materiales, value); } }

        private async void Buscando()
        {
            if (string.IsNullOrWhiteSpace(Busqueda))
            {
                Stocks = new ObservableCollection<StockDto>(await Servicios.ApiProcessor.GetApi<StockDto[]>("Stock/GetAll"));

            }
            else
            {
                Stocks = new ObservableCollection<StockDto>(await Servicios.ApiProcessor.GetApi<StockDto[]>($"Stock/GetByFilter/{Busqueda}"));
            }
        }

        public override async Task Inicializar()
        {
            Stocks = new ObservableCollection<StockDto>(await Servicios.ApiProcessor.GetApi<StockDto[]>("Stock/GetAll"));
            Materiales = new ObservableCollection<MaterialDto>(await Servicios.ApiProcessor.GetApi<MaterialDto[]>("Material/GetAll"));
        }
        protected async override Task CrearNuevoElemento()
        {
            if (Stock.Material != null)
            {
                Stock.FechaActualizacion = DateTime.Now;
                Stock.UsuarioId = UsuarioGral.UsuarioId;
                Stock.MaterialId = Stock.Material.Id;
                await Servicios.ApiProcessor.PostApi(Stock, "Stock/Insert");
                await Inicializar();
                Stock = null;
                Stock = new StockDto();
            }
        }
        protected async override Task EliminarElemento()
        {
            await Servicios.ApiProcessor.DeleteApi($"Stock/{Stock.Id}");
            await Inicializar();
        }
        protected async override Task EditarElemento()
        {
            if (Stock.Material!=null)
            {
                Stock.FechaActualizacion = DateTime.Now;
                Stock.MaterialId = Stock.Material.Id;
                Stock.UsuarioId = UsuarioGral.UsuarioId;
                await Servicios.ApiProcessor.PutApi(Stock, $"Stock/{Stock.Id}");
                await Inicializar();
            }
        }

        protected override void Nuevo()
        {
            base.Nuevo();
            Stock = new StockDto();
        }
        protected override void Cancelar()
        {
            base.Cancelar();
            Stock = null;
        }
        public StockABMViewModel()
        {
            Stock = null;
            Buscar = new DelegateCommand(Buscando);
            CrearObraCommand = new DelegateCommand(Nuevo);
            CancelarCommand = new DelegateCommand(Cancelar);
            EditarObraCommand = new DelegateCommand(Editar, () => ObjetoNull.IsNull(Stock)).ObservesProperty(() => Stock);
            EliminarObraCommand = new DelegateCommand(Eliminar, () => ObjetoNull.IsNull(Stock)).ObservesProperty(() => Stock);
        }
    }
}
