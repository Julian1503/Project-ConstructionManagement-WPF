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
    public class DescripcionTareaDtoABMViewModel : BaseABMViewModel
    {
        private DescripcionTareaDto _DescripcionTarea;
        private ObservableCollection<DescripcionTareaDto> _descripcionesTarea;

        public DescripcionTareaDto DescripcionTarea { get { return _DescripcionTarea; } set { SetProperty(ref _DescripcionTarea, value); } }
        public ObservableCollection<DescripcionTareaDto> DescripcionesTarea { get { return _descripcionesTarea; } set { SetProperty(ref _descripcionesTarea, value); } }

        public override async Task Inicializar()
        {
            DescripcionesTarea = new ObservableCollection<DescripcionTareaDto>(await Servicios.ApiProcessor.GetApi<DescripcionTareaDto[]>("DescripcionTarea/GetAll"));
        }
        protected async override Task CrearNuevoElemento()
        {
            if (!string.IsNullOrWhiteSpace(DescripcionTarea.Descripcion))
            {
                await Servicios.ApiProcessor.PostApi(DescripcionTarea, "DescripcionTarea/Insert");
                await Inicializar();
                DescripcionTarea = new DescripcionTareaDto();
            }
        }
        protected async override Task EliminarElemento()
        {
            await Servicios.ApiProcessor.DeleteApi($"DescripcionTarea/{DescripcionTarea.Id}");
            await Inicializar();
        }
        protected async override Task EditarElemento()
        {
            if (!string.IsNullOrWhiteSpace(DescripcionTarea.Descripcion))
            {
                await Servicios.ApiProcessor.PutApi(DescripcionTarea, $"DescripcionTarea/{DescripcionTarea.Id}");
                await Inicializar();
            }
        }
        private async void Buscando()
        {
            if (string.IsNullOrWhiteSpace(Busqueda))
            {
                DescripcionesTarea = new ObservableCollection<DescripcionTareaDto>(await Servicios.ApiProcessor.GetApi<DescripcionTareaDto[]>("DescripcionTarea/GetAll"));

            }
            else
            {
                DescripcionesTarea = new ObservableCollection<DescripcionTareaDto>(await Servicios.ApiProcessor.GetApi<DescripcionTareaDto[]>($"DescripcionTarea/GetByFilter/{Busqueda}"));
            }
        }
        protected override void Nuevo()
        {
            base.Nuevo();
            DescripcionTarea = new DescripcionTareaDto();
        }
        protected override void Cancelar()
        {
            base.Cancelar();
            DescripcionTarea = null;
        }
        public DescripcionTareaDtoABMViewModel()
        {
            Buscar = new DelegateCommand(Buscando);
            DescripcionTarea = null;
            CrearObraCommand = new DelegateCommand(Nuevo);
            CancelarCommand = new DelegateCommand(Cancelar);
            EditarObraCommand = new DelegateCommand(Editar, ()=> ObjetoNull.IsNull(DescripcionTarea)).ObservesProperty(() => DescripcionTarea);
            EliminarObraCommand = new DelegateCommand(Eliminar, ()=> ObjetoNull.IsNull(DescripcionTarea)).ObservesProperty(() => DescripcionTarea);
        }

    }
}
