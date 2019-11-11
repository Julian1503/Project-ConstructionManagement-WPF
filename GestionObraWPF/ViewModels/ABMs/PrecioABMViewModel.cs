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
    public class PrecioABMViewModel : BaseABMViewModel
    {
        public PrecioABMViewModel()
        {
            Precio = null;
            CrearObraCommand = new DelegateCommand(Nuevo);
            CancelarCommand = new DelegateCommand(Cancelar);
            EditarObraCommand = new DelegateCommand(Editar, ()=> ObjetoNull.IsNull(Precio)).ObservesProperty(() => Precio);
            EliminarObraCommand = new DelegateCommand(Eliminar, ()=> ObjetoNull.IsNull(Precio)).ObservesProperty(() => Precio);
        }
        private PrecioDto _precio;
        public PrecioDto Precio { get { return _precio; } set { SetProperty(ref _precio, value); } }

        private ObservableCollection<PrecioDto> _precios;
        private ObservableCollection<MaterialDto> _materiales;

        public ObservableCollection<PrecioDto> Precios { get { return _precios; } set { SetProperty(ref _precios, value); } }

        public ObservableCollection<MaterialDto> Materiales { get { return _materiales; } set { SetProperty(ref _materiales, value); } }

        public override async Task Inicializar()
        {
            Precios = new ObservableCollection<PrecioDto>(await Servicios.ApiProcessor.GetApi<PrecioDto[]>("Precio/GetAll"));
            Materiales = new ObservableCollection<MaterialDto>(await Servicios.ApiProcessor.GetApi<MaterialDto[]>("Material/GetAll"));
        }
        protected async override Task CrearNuevoElemento()
        {
            if (Precio.Material!= null && Precio.PrecioCompra>0)
            {
                Precio.MaterialId = Precio.Material.Id;
                Precio.UsuarioId= UsuarioGral.UsuarioId;
                await Servicios.ApiProcessor.PostApi(Precio, "Precio/Insert");
                await Inicializar();
                Precio = null;
                Precio = new PrecioDto();
            }
        }
        protected async override Task EliminarElemento()
        {
            await Servicios.ApiProcessor.DeleteApi($"Precio/{Precio.Id}");
            await Inicializar();
        }
        protected async override Task EditarElemento()
        {
            if (Precio.Material != null && Precio.PrecioCompra > 0)
            {
                await Servicios.ApiProcessor.PutApi(Precio, $"Precio/{Precio.Id}");
                await Inicializar();
            }
        }
      
        protected override void Nuevo()
        {
            base.Nuevo();
            Precio = new PrecioDto();
        }
        protected override void Cancelar()
        {
            base.Cancelar();
            Precio = null;
        }
       
    }
}
