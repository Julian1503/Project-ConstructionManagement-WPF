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
    public class CondicionIvaABMViewModel : BaseABMViewModel
    {
        private CondicionIvaDto _condicionIva;
        private ObservableCollection<CondicionIvaDto> _condicionesIva;

        public CondicionIvaDto CondicionIva { get { return _condicionIva; } set { SetProperty(ref _condicionIva, value); } }
        public ObservableCollection<CondicionIvaDto> CondicionesIva { get { return _condicionesIva; } set { SetProperty(ref _condicionesIva, value); } }

        public override async Task Inicializar()
        {
            CondicionesIva = new ObservableCollection<CondicionIvaDto>(await Servicios.ApiProcessor.GetApi<CondicionIvaDto[]>("CondicionIva/GetAll"));
        }
        protected async override Task CrearNuevoElemento()
        {
            if (!string.IsNullOrWhiteSpace(CondicionIva.Descripcion))
            {
                await Servicios.ApiProcessor.PostApi(CondicionIva, "CondicionIva/Insert");
                await Inicializar();
                CondicionIva = new CondicionIvaDto();
            }
        }
        protected async override Task EliminarElemento()
        {
            await Servicios.ApiProcessor.DeleteApi($"CondicionIva/{CondicionIva.Id}");
            await Inicializar();
        }
        protected async override Task EditarElemento()
        {
            if (!string.IsNullOrWhiteSpace(CondicionIva.Descripcion))
            {
                await Servicios.ApiProcessor.PutApi(CondicionIva, $"CondicionIva/{CondicionIva.Id}");
                await Inicializar();
            }
        }
        private async void Buscando()
        {
            if (string.IsNullOrWhiteSpace(Busqueda))
            {
                CondicionesIva = new ObservableCollection<CondicionIvaDto>(await Servicios.ApiProcessor.GetApi<CondicionIvaDto[]>("CondicionIva/GetAll"));

            }
            else
            {
                CondicionesIva = new ObservableCollection<CondicionIvaDto>(await Servicios.ApiProcessor.GetApi<CondicionIvaDto[]>($"CondicionIva/GetByFilter/{Busqueda}"));
            }
        }

        protected override void Nuevo()
        {
            base.Nuevo();
            CondicionIva = new CondicionIvaDto();
        }
        protected override void Cancelar()
        {
            base.Cancelar();
            CondicionIva = null;
        }
        public CondicionIvaABMViewModel()
        {
            CondicionIva = null;
            Buscar = new DelegateCommand(Buscando);
            CrearObraCommand = new DelegateCommand(Nuevo);
            CancelarCommand = new DelegateCommand(Cancelar);
            EditarObraCommand = new DelegateCommand(Editar, ()=> ObjetoNull.IsNull(CondicionIva)).ObservesProperty(() => CondicionIva);
            EliminarObraCommand = new DelegateCommand(Eliminar, ()=> ObjetoNull.IsNull(CondicionIva)).ObservesProperty(() => CondicionIva);
        }
    }
}
