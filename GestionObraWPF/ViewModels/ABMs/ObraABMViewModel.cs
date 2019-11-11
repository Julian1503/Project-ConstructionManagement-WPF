using GestionObraWPF.DTOs;
using GestionObraWPF.Model;
using GestionObraWPF.Servicios;
using GestionObraWPF.ViewModels.ABMs;
using MaterialDesignThemes.Wpf;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace GestionObraWPF.ViewModels
{
    public class ObraABMViewModel : BaseABMViewModel
    {
        private string _title = "Prism Application";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public override async Task Inicializar()
        {
            Obras = new ObservableCollection<ObraDto>(await ApiProcessor.GetApi<ObraDto[]>("Obra/GetAll"));
            Propietarios = new ObservableCollection<EmpresaDto>(await ApiProcessor.GetApi<EmpresaDto[]>("Empresa/GetAll"));
            Zonas = new ObservableCollection<ZonaDto>(await ApiProcessor.GetApi<ZonaDto[]>("Zona/GetAll"));
            Encargados = new ObservableCollection<EmpleadoDto>(await ApiProcessor.GetApi<EmpleadoDto[]>("Empleado/GetAll"));
        }
        protected async override Task CrearNuevoElemento()
        {
            if (!string.IsNullOrWhiteSpace(Obra.Descripcion) && Obra.Encargado!=null && Obra.Propietario!= null)
            {
                Obra.Path = "";
                Obra.PropietarioId = Obra.Propietario.Id;
                if (Obra.Zona != null)
                {
                    Obra.ZonaId = Obra.Zona.Id;
                }
                Obra.EstadoObra = Constantes.EstadoObra.Pendiente;
                Obra.EncargadoId = Obra.Encargado.Id;
                await ApiProcessor.PostApi(Obra, "Obra/Insert");
                await Inicializar();
                Obra = new ObraDto();
            }
        }
        protected async override Task EliminarElemento()
        {
            await ApiProcessor.DeleteApi($"Obra/{Obra.Id}");
            await Inicializar();
        }
        protected async override Task EditarElemento()
        {
            if (!string.IsNullOrWhiteSpace(Obra.Descripcion) && Obra.Encargado != null && Obra.Propietario != null)
            {
                if (Obra.Zona != null)
                {
                    Obra.ZonaId = Obra.Zona.Id;
                }
                Obra.PropietarioId = Obra.Propietario.Id;
                Obra.EncargadoId = Obra.Encargado.Id;
                await ApiProcessor.PutApi(Obra, $"Obra/{Obra.Id}");
                await Inicializar();
            }
        }
        private async void Buscando()
        {
            if (string.IsNullOrWhiteSpace(Busqueda))
            {
                Obras = new ObservableCollection<ObraDto>(await Servicios.ApiProcessor.GetApi<ObraDto[]>("Obra/GetAll"));

            }
            else
            {
                Obras = new ObservableCollection<ObraDto>(await Servicios.ApiProcessor.GetApi<ObraDto[]>($"Obra/GetByFilter/{Busqueda}"));
            }
        }

        private PersonaDto _encargado;
        public PersonaDto Encargado
        {
            get { return _encargado; }
            set { SetProperty(ref _encargado, value); }
        }
        private ObraDto _obraDTo;
        private ObservableCollection<ZonaDto> _zonas;
        private ObservableCollection<ObraDto> _obras;
        private ObservableCollection<EmpleadoDto> _encargados;
        private ObservableCollection<EmpresaDto> _propietario;

        public ObraDto Obra
        {
            get { return _obraDTo; }
            set
            {
                SetProperty(ref _obraDTo, value);
            }
        }
        public ObservableCollection<ObraDto> Obras
        {
            get { return _obras; }
            set
            {
                SetProperty(ref _obras, value);
            }
        }
        public ICommand SeleccionarCommand { get; set; }
       
        public DelegateCommand Inicizalizar { get; set; }

        public ObraABMViewModel()
        {
            Obra = null;
            Buscar = new DelegateCommand(Buscando);
            CancelarCommand = new DelegateCommand(Cancelar);
            CrearObraCommand = new DelegateCommand(Nuevo);
            EditarObraCommand = new DelegateCommand(Editar, SeleccionarObra).ObservesProperty(() => Obra);
            EliminarObraCommand = new DelegateCommand(Eliminar, SeleccionarObra).ObservesProperty(()=> Obra);
        }

        private bool SeleccionarObra()
        {
            if (Obra != null)
            {
                return true;
            }
            return false;
        }

        protected override void Cancelar()
        {
            base.Cancelar();
            Obra = null;
        }

        protected override void Nuevo()
        {
            base.Nuevo();
            Obra = new ObraDto();
        }

        public ObservableCollection<ZonaDto> Zonas
        {
            get { return _zonas; }
            set { SetProperty(ref _zonas, value); }
        }
   
        public ObservableCollection<EmpleadoDto> Encargados
        {
            get { return _encargados; }
            set { SetProperty(ref _encargados, value); }
        }
        public ObservableCollection<EmpresaDto> Propietarios
        {
            get { return _propietario; }
            set { SetProperty(ref _propietario, value); }
        }
    }
}
