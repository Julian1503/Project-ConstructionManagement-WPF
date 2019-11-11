using GestionObraWPF.Constantes;
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
    public class RubroABMViewModel : BaseABMViewModel
    {
        private RubroDto _rubro;
        private ObservableCollection<RubroDto> _rubros;
        private TipoRubro _tipoRubro;

        public RubroDto Rubro { get { return _rubro; } set { SetProperty(ref _rubro, value); } }
        public TipoRubro TipoRubro { get { return _tipoRubro; } set { SetProperty(ref _tipoRubro, value); } }
        public ObservableCollection<RubroDto> Rubros { get { return _rubros; } set { SetProperty(ref _rubros, value); } }
        public override async Task Inicializar()
        {
            Rubros = new ObservableCollection<RubroDto>(await Servicios.ApiProcessor.GetApi<RubroDto[]>("Rubro/GetAll"));
        }
        protected async override Task CrearNuevoElemento()
        {
            Rubro.TipoRubro = TipoRubro;
            if (!string.IsNullOrWhiteSpace(Rubro.Descripcion))
            {
                await Servicios.ApiProcessor.PostApi(Rubro, "Rubro/Insert");
                await Inicializar();
                Rubro = new RubroDto();
            }
        }
        protected async override Task EliminarElemento()
        {
            await Servicios.ApiProcessor.DeleteApi($"Rubro/{Rubro.Id}");
            await Inicializar();
        }
        protected async override Task EditarElemento()
        {
            Rubro.TipoRubro = TipoRubro;
            if (!string.IsNullOrWhiteSpace(Rubro.Descripcion))
            {
                await Servicios.ApiProcessor.PutApi(Rubro, $"Rubro/{Rubro.Id}");
                await Inicializar();
            }
        }
        private async void Buscando()
        {
            if (string.IsNullOrWhiteSpace(Busqueda))
            {
                Rubros = new ObservableCollection<RubroDto>(await Servicios.ApiProcessor.GetApi<RubroDto[]>("Rubro/GetAll"));

            }
            else
            {
                Rubros = new ObservableCollection<RubroDto>(await Servicios.ApiProcessor.GetApi<RubroDto[]>($"Rubro/GetByFilter/{Busqueda}"));
            }
        }
        protected override void Editar()
        {
            TipoRubro = Rubro.TipoRubro;
            base.Editar();
        }
        protected override void Nuevo()
        {
            base.Nuevo();
            Rubro = new RubroDto();
        }
        protected override void Cancelar()
        {
            base.Cancelar();
            Rubro = null;
        }
        public RubroABMViewModel()
        {
            Rubro = null;
            Buscar = new DelegateCommand(Buscando);
            CrearObraCommand = new DelegateCommand(Nuevo);
            CancelarCommand = new DelegateCommand(Cancelar);
            EditarObraCommand = new DelegateCommand(Editar, ()=> ObjetoNull.IsNull(Rubro)).ObservesProperty(() => Rubro);
            EliminarObraCommand = new DelegateCommand(Eliminar, ()=> ObjetoNull.IsNull(Rubro)).ObservesProperty(() => Rubro);
        }

    }
}
