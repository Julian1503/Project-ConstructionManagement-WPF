using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using GestionObraWPF.DTOs;
using GestionObraWPF.Helpers;
using GestionObraWPF.Servicios;
using GestionObraWPF.ViewModels.ABMs;
using Microsoft.Win32;
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
    public class EmpresaABMViewModel : BaseABMViewModel
    {
       private EmpresaDto _empresa;
        private ObservableCollection<EmpresaDto> _empresas;
        private ObservableCollection<CondicionIvaDto> _condicionesIva;

        public EmpresaDto Empresa { get { return _empresa; } set { SetProperty(ref _empresa, value); } }
        public ObservableCollection<EmpresaDto> Empresas { get { return _empresas; } set { SetProperty(ref _empresas, value); } }
        public ObservableCollection<CondicionIvaDto> CondicionesIva { get { return _condicionesIva; } set { SetProperty(ref _condicionesIva, value); } }
        public ICommand BuscarImagenCommand { get; set; }

        public override async Task Inicializar()
        {
            Empresas = new ObservableCollection<EmpresaDto>(await Servicios.ApiProcessor.GetApi<EmpresaDto[]>("Empresa/GetAll"));
            CondicionesIva = new ObservableCollection<CondicionIvaDto>(await Servicios.ApiProcessor.GetApi<CondicionIvaDto[]>("CondicionIva/GetAll"));
        }
        protected async override Task CrearNuevoElemento()
        {
            if (!string.IsNullOrWhiteSpace(Empresa.RazonSocial) && !string.IsNullOrWhiteSpace(Empresa.NombreFantasia))
            {
                if (Empresa.CondicionIva != null)
                {
                    Empresa.CondicionIvaId = Empresa.CondicionIva.Id;
                }
                await Servicios.ApiProcessor.PostApi(Empresa, "Empresa/Insert");
                await Inicializar();
                Empresa = new EmpresaDto();
            }
        }
        protected async override Task EliminarElemento()
        {
            await Servicios.ApiProcessor.DeleteApi($"Empresa/{Empresa.Id}");
            await Inicializar();
        }
        protected async override Task EditarElemento()
        {
            if (!string.IsNullOrWhiteSpace(Empresa.RazonSocial) && !string.IsNullOrWhiteSpace(Empresa.NombreFantasia))
            {
                if (Empresa.CondicionIva != null)
                {
                    Empresa.CondicionIvaId = Empresa.CondicionIva.Id;
                }
                await Servicios.ApiProcessor.PutApi(Empresa, $"Empresa/{Empresa.Id}");
                await Inicializar();
            }
        }
        private async void Buscando()
        {
            if (string.IsNullOrWhiteSpace(Busqueda))
            {
                Empresas = new ObservableCollection<EmpresaDto>(await Servicios.ApiProcessor.GetApi<EmpresaDto[]>("Empresa/GetAll"));

            }
            else
            {
                Empresas = new ObservableCollection<EmpresaDto>(await Servicios.ApiProcessor.GetApi<EmpresaDto[]>($"Empresa/GetByFilter/{Busqueda}"));
            }
        }
        protected override void Nuevo()
        {
            base.Nuevo();
            Empresa = new EmpresaDto();
        }
        protected override void Cancelar()
        {
            base.Cancelar();
            Empresa = null;
        }
        private void BuscarImagen()
        {
            Empresa.Path = CloudImage.BuscarImagen();
        }
        public EmpresaABMViewModel()
        {
            Empresa = null;
            Buscar = new DelegateCommand(Buscando);
            BuscarImagenCommand = new DelegateCommand(BuscarImagen);
            CrearObraCommand = new DelegateCommand(Nuevo);
            CancelarCommand = new DelegateCommand(Cancelar);
            EditarObraCommand = new DelegateCommand(Editar, ()=> ObjetoNull.IsNull(Empresa)).ObservesProperty(() => Empresa);
            EliminarObraCommand = new DelegateCommand(Eliminar, ()=> ObjetoNull.IsNull(Empresa)).ObservesProperty(() => Empresa);
        }
    }
}
