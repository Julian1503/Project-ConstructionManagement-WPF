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
    public class SubRubroABMViewModel : BaseABMViewModel
    {
        private SubRubroDto _SubRubro;
        private ObservableCollection<SubRubroDto> _subRubros;
        private ObservableCollection<RubroDto> _rubros;
        public SubRubroDto SubRubro { get { return _SubRubro; } set { SetProperty(ref _SubRubro, value); } }
        public ObservableCollection<SubRubroDto> SubRubros { get{ return _subRubros; } set { SetProperty(ref _subRubros, value); } }
        public ObservableCollection<RubroDto> Rubros  { get{ return _rubros; } set { SetProperty(ref _rubros, value); } }

       

        public override async Task Inicializar()
        {
            Rubros = new ObservableCollection<RubroDto>(await Servicios.ApiProcessor.GetApi<RubroDto[]>("Rubro/GetAll"));
            SubRubros = new ObservableCollection<SubRubroDto>(await Servicios.ApiProcessor.GetApi<SubRubroDto[]>("SubRubro/GetAll"));
        }
        protected async override Task CrearNuevoElemento()
        {
            if (!string.IsNullOrWhiteSpace(SubRubro.Descripcion) && SubRubro.Rubro!=null)
            {
                SubRubro.RubroId = SubRubro.Rubro.Id;
                await Servicios.ApiProcessor.PostApi(SubRubro, "SubRubro/Insert");
                await Inicializar();
                SubRubro = new SubRubroDto();
            }
        }
        protected async override Task EliminarElemento()
        {
            await Servicios.ApiProcessor.DeleteApi($"SubRubro/{SubRubro.Id}");
            await Inicializar();
        }
        protected async override Task EditarElemento()
        {
            if (!string.IsNullOrWhiteSpace(SubRubro.Descripcion) && SubRubro.Rubro != null)
            {
                SubRubro.RubroId = SubRubro.Rubro.Id;
                await Servicios.ApiProcessor.PutApi(SubRubro, $"SubRubro/{SubRubro.Id}");
                await Inicializar();
            }
        }

        private async void Buscando()
        {
            if (string.IsNullOrWhiteSpace(Busqueda))
            {
                SubRubros = new ObservableCollection<SubRubroDto>(await Servicios.ApiProcessor.GetApi<SubRubroDto[]>("SubRubro/GetAll"));

            }
            else
            {
                SubRubros = new ObservableCollection<SubRubroDto>(await Servicios.ApiProcessor.GetApi<SubRubroDto[]>($"SubRubro/GetByFilter/{Busqueda}"));
            }
        }

        protected override void Nuevo()
        {
            base.Nuevo();
            SubRubro = new SubRubroDto();
        }
        protected override void Cancelar()
        {
            base.Cancelar();
            SubRubro = null;
        }
        public SubRubroABMViewModel()
        {
            SubRubro = null;
            Buscar = new DelegateCommand(Buscando);
            CrearObraCommand = new DelegateCommand(Nuevo);
            CancelarCommand = new DelegateCommand(Cancelar);
            EditarObraCommand = new DelegateCommand(Editar, ()=> ObjetoNull.IsNull(SubRubro)).ObservesProperty(() => SubRubro);
            EliminarObraCommand = new DelegateCommand(Eliminar, ()=> ObjetoNull.IsNull(SubRubro)).ObservesProperty(() => SubRubro);
        }
    }
}
