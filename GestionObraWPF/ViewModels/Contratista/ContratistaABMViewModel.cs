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
    public class ContratistaABMViewModel : BaseABMViewModel
    {
        private ContratistaDto _contratista;
        private ObservableCollection<ContratistaDto> _contratistas;
        private ObservableCollection<CondicionIvaDto> _condicionesIva;

        public ContratistaDto Contratista { get { return _contratista; } set { SetProperty(ref _contratista, value); } }
        public ObservableCollection<ContratistaDto> Contratistas { get { return _contratistas; } set { SetProperty(ref _contratistas, value); } }
        public ObservableCollection<CondicionIvaDto> CondicionesIva { get { return _condicionesIva; } set { SetProperty(ref _condicionesIva, value); } }
        public ICommand BuscarImagenCommand { get; set; }

        public override async Task Inicializar()
        {
            Contratistas = new ObservableCollection<ContratistaDto>(await Servicios.ApiProcessor.GetApi<ContratistaDto[]>("Contratista/GetAll"));
            CondicionesIva = new ObservableCollection<CondicionIvaDto>(await Servicios.ApiProcessor.GetApi<CondicionIvaDto[]>("CondicionIva/GetAll"));
        }
        protected async override Task CrearNuevoElemento()
        {
            if (!string.IsNullOrWhiteSpace(Contratista.RazonSocial) && !string.IsNullOrWhiteSpace(Contratista.NombreFantasia))
            {
                await Servicios.ApiProcessor.PostApi(Contratista, "Contratista/Insert");
                await Inicializar();
                Contratista = new ContratistaDto();
            }
        }
        protected async override Task EliminarElemento()
        {
            await Servicios.ApiProcessor.DeleteApi($"Contratista/{Contratista.Id}");
            await Inicializar();
        }
        protected async override Task EditarElemento()
        {
            if (!string.IsNullOrWhiteSpace(Contratista.RazonSocial) && !string.IsNullOrWhiteSpace(Contratista.NombreFantasia))
            {
                await Servicios.ApiProcessor.PutApi(Contratista, $"Contratista/{Contratista.Id}");
                await Inicializar();
            }
        }
        private async void Buscando()
        {
            if (string.IsNullOrWhiteSpace(Busqueda))
            {
                Contratistas = new ObservableCollection<ContratistaDto>(await Servicios.ApiProcessor.GetApi<ContratistaDto[]>("Contratista/GetAll"));

            }
            else
            {
                Contratistas = new ObservableCollection<ContratistaDto>(await Servicios.ApiProcessor.GetApi<ContratistaDto[]>($"Contratista/GetByFilter/{Busqueda}"));
            }
        }
        protected override void Nuevo()
        {
            base.Nuevo();
            Contratista = new ContratistaDto();
        }
        protected override void Cancelar()
        {
            base.Cancelar();
            Contratista = null;
        }
        private void BuscarImagen()
        {
            Contratista.Path = CloudImage.BuscarImagen();
        }
        public ContratistaABMViewModel()
        {
            Contratista = null;
            Buscar = new DelegateCommand(Buscando);
            BuscarImagenCommand = new DelegateCommand(BuscarImagen);
            CrearObraCommand = new DelegateCommand(Nuevo);
            CancelarCommand = new DelegateCommand(Cancelar);
            EditarObraCommand = new DelegateCommand(Editar, () => ObjetoNull.IsNull(Contratista)).ObservesProperty(() => Contratista);
            EliminarObraCommand = new DelegateCommand(Eliminar, () => ObjetoNull.IsNull(Contratista)).ObservesProperty(() => Contratista);
        }
    }
}
