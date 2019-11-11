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
using System.Windows.Input;

namespace GestionObraWPF.ViewModels
{
    public class ZonaABMViewModel : BaseABMViewModel
    {
        private ZonaDto _zona;
        public ZonaDto Zona { get { return _zona; } set { SetProperty(ref _zona, value); } }

        private ObservableCollection<ZonaDto> _zonas;
        public ObservableCollection<ZonaDto> Zonas { get { return _zonas; } set { SetProperty(ref _zonas, value); } }

        public override async Task Inicializar()
        {
            Zonas = new ObservableCollection<ZonaDto>(await Servicios.ApiProcessor.GetApi<ZonaDto[]>("Zona/GetAll"));
        }
        protected async override Task CrearNuevoElemento()
        {
            if (!string.IsNullOrWhiteSpace(Zona.Descripcion))
            {
                await Servicios.ApiProcessor.PostApi(Zona, "Zona/Insert");
                await Inicializar();
                Zona = null;
                Zona = new ZonaDto();
            }
        }
        protected async override Task EliminarElemento()
        {
            await Servicios.ApiProcessor.DeleteApi($"Zona/{Zona.Id}");
            await Inicializar();
        }
        protected async override Task EditarElemento()
        {
            if (!string.IsNullOrWhiteSpace(Zona.Descripcion))
            {
                await Servicios.ApiProcessor.PutApi(Zona, $"Zona/{Zona.Id}");
                await Inicializar();
            }
        }
        private async void Buscando()
        {
            if (string.IsNullOrWhiteSpace(Busqueda))
            {
                Zonas = new ObservableCollection<ZonaDto>(await Servicios.ApiProcessor.GetApi<ZonaDto[]>("Zona/GetAll"));

            }
            else
            {
                Zonas = new ObservableCollection<ZonaDto>(await Servicios.ApiProcessor.GetApi<ZonaDto[]>($"Zona/GetByFilter/{Busqueda}"));
            }
        }
        protected override void Nuevo()
        {
            base.Nuevo();
            Zona = new ZonaDto();
        }
        protected override void Cancelar()
        {
            base.Cancelar();
            Zona = null;
        }
        public ZonaABMViewModel()
        {
            Zona = null;
            Buscar = new DelegateCommand(Buscando);
            CrearObraCommand = new DelegateCommand(Nuevo);
            CancelarCommand = new DelegateCommand(Cancelar);
            EditarObraCommand = new DelegateCommand(Editar, ()=> ObjetoNull.IsNull(Zona)).ObservesProperty(() => Zona);
            EliminarObraCommand = new DelegateCommand(Eliminar, ()=> ObjetoNull.IsNull(Zona)).ObservesProperty(() => Zona);
        }

    }
}
