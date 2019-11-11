using GestionObraWPF.Model;
using GestionObraWPF.ViewModels.ABMs;
using Microsoft.Win32;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using System.Threading.Tasks;
using GestionObraWPF.Helpers;
using GestionObraWPF.Servicios;

namespace GestionObraWPF.ViewModels
{
    public class MaterialABMViewModel : BaseABMViewModel
    {
        private MaterialDto _material;
        private ObservableCollection<MaterialDto> _materiales;

        public MaterialDto Material
        {
            get { return _material; }
            set
            {
                SetProperty(ref _material, value);
            }
        }
        public ObservableCollection<MaterialDto> Materiales {
            get { return _materiales; }
            set
            {
                SetProperty(ref _materiales, value);
            }
        }
        public override async Task Inicializar()
        {
            Materiales = new ObservableCollection<MaterialDto>(await Servicios.ApiProcessor.GetApi<MaterialDto[]>("Material/GetAll"));
        }
        protected async override Task CrearNuevoElemento()
        {
            if (!string.IsNullOrWhiteSpace(Material.Descripcion) && !string.IsNullOrWhiteSpace(Material.Codigo) )
            {
                await Servicios.ApiProcessor.PostApi(Material, "Material/Insert");
                await Inicializar();
                Material = new MaterialDto();
            }
        }
        protected async override Task EliminarElemento()
        {
            await Servicios.ApiProcessor.DeleteApi($"Material/{Material.Id}");
            await Inicializar();
        }
        protected async override Task EditarElemento()
        {
            if (!string.IsNullOrWhiteSpace(Material.Descripcion) && !string.IsNullOrWhiteSpace(Material.Codigo)  )
            {
                await Servicios.ApiProcessor.PutApi(Material, $"Material/{Material.Id}");
                await Inicializar();
            }
        }

        public ICommand BuscarImagenCommand { get; set; }
        public MaterialABMViewModel()
        {
       
            Material = null;
            BuscarImagenCommand = new DelegateCommand(BuscarImagen);
            CancelarCommand = new DelegateCommand(Cancelar);
            CrearObraCommand = new DelegateCommand(Nuevo);
            EditarObraCommand = new DelegateCommand(Editar ,()=> ObjetoNull.IsNull(Material)).ObservesProperty(() => Material);
            EliminarObraCommand = new DelegateCommand(Eliminar ,()=> ObjetoNull.IsNull(Material)).ObservesProperty(() => Material);
        }

        protected override void Cancelar()
        {
            base.Cancelar();
            Material = null;
        }

     

        protected override void Nuevo()
        {
            base.Nuevo();
            Material = new MaterialDto();
        }

        private void BuscarImagen()
        {
           Material.Path = CloudImage.BuscarImagen();
        }
    }
}
