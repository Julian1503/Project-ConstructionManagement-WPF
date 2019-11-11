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
    public class TipoGastoABMViewModel : BaseABMViewModel
    {
        private TipoGastoDto _tipoGasto;
        private ObservableCollection<TipoGastoDto> _tipoGastos;
        public TipoGastoDto TipoGasto { get { return _tipoGasto; } set { SetProperty(ref _tipoGasto, value); } }
        public ObservableCollection<TipoGastoDto> TiposGasto { get { return _tipoGastos; } set { SetProperty(ref _tipoGastos, value); } }

        public override async Task Inicializar()
        {
            TiposGasto = new ObservableCollection<TipoGastoDto>(await Servicios.ApiProcessor.GetApi<TipoGastoDto[]>("TipoGasto/GetAll"));
        }
        protected async override Task CrearNuevoElemento()
        {
            if (!string.IsNullOrWhiteSpace(TipoGasto.Descripcion))
            {
                await Servicios.ApiProcessor.PostApi(TipoGasto, "TipoGasto/Insert");
                await Inicializar();
                TipoGasto = new TipoGastoDto();
            }
        }
        protected async override Task EliminarElemento()
        {
            await Servicios.ApiProcessor.DeleteApi($"TipoGasto/{TipoGasto.Id}");
            await Inicializar();
        }
        protected async override Task EditarElemento()
        {
            if (!string.IsNullOrWhiteSpace(TipoGasto.Descripcion))
            {
                await Servicios.ApiProcessor.PutApi(TipoGasto, $"TipoGasto/{TipoGasto.Id}");
                await Inicializar();
            }
        }
        private async void Buscando()
        {
            if (string.IsNullOrWhiteSpace(Busqueda))
            {
                TiposGasto = new ObservableCollection<TipoGastoDto>(await Servicios.ApiProcessor.GetApi<TipoGastoDto[]>("TipoGasto/GetAll"));

            }
            else
            {
                TiposGasto = new ObservableCollection<TipoGastoDto>(await Servicios.ApiProcessor.GetApi<TipoGastoDto[]>($"TipoGasto/GetByFilter/{Busqueda}"));
            }
        }
        protected override void Nuevo()
        {
            base.Nuevo();
            TipoGasto = new TipoGastoDto();
        }
        protected override void Cancelar()
        {
            base.Cancelar();
            TipoGasto = null;
        }
        public TipoGastoABMViewModel()
        {
            TipoGasto = null;
            Buscar = new DelegateCommand(Buscando);
            CrearObraCommand = new DelegateCommand(Nuevo);
            CancelarCommand = new DelegateCommand(Cancelar);
            EditarObraCommand = new DelegateCommand(Editar, ()=> ObjetoNull.IsNull(TipoGasto)).ObservesProperty(() => TipoGasto);
            EliminarObraCommand = new DelegateCommand(Eliminar, ()=> ObjetoNull.IsNull(TipoGasto)).ObservesProperty(() => TipoGasto);
        }
    }
}
