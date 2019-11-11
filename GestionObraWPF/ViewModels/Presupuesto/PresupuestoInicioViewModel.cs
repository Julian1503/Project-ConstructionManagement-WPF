using GestionObraWPF.Constantes;
using GestionObraWPF.DTOs;
using GestionObraWPF.Helpers;
using GestionObraWPF.Servicios;
using GestionObraWPF.ViewModels.ABMs;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace GestionObraWPF.ViewModels
{
    public class PresupuestoInicioViewModel : BaseABMViewModel
    {
        public ObservableCollection<EmpresaDto> Empresas
        {
            get { return _empresas; }
            set
            {
                SetProperty(ref _empresas, value);
            }
        }

        public ObservableCollection<ObraDto> Obras
        {
            get { return _obra; }
            set
            {
                _obra = value;
                RaisePropertyChanged();
            }
        }

        public PresupuestoInicioViewModel(IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            this.regionManager = regionManager;
            Command = new DelegateCommand<PresupuestoDto>(DobleClick);
        }
        private PresupuestoDto _presupuesto;
        private ObservableCollection<PresupuestoDto> _presupuestos;
        private Visibility _sinPresupuesto;
        private ObservableCollection<EmpresaDto> _empresas;
        private ObservableCollection<ObraDto> _obra;
        private IEventAggregator eventAggregator;

        public PresupuestoDto Presupuesto
        {
            get { return _presupuesto; }
            set
            {
                _presupuesto = value;
                RaisePropertyChanged();
            }
        }

        public DelegateCommand<PresupuestoDto> Command { get; set; }
        public DelegateCommand<PresupuestoDto> CancelarPresupuesto { get; set; }
        public ICommand AprobarPresupuesto { get; set; } 
        public DelegateCommand<PresupuestoDto> PendientePrespuesto { get; set; }
        public IRegionManager regionManager { get; set; }


        private void DobleClick(PresupuestoDto obj)
        {
            if (obj != null)
            {
                var nav = new Prism.Regions.NavigationParameters();
                nav.Add("Presupuesto", obj);
                regionManager.RequestNavigate("Contenido", "PresupuestoDetalle", nav);
                 eventAggregator.GetEvent<PubSubEvent<Visibility>>().Publish(Visibility.Collapsed);
            }
        }

        public override async Task Inicializar()
        {
            Presupuestos = new ObservableCollection<PresupuestoDto>(await ApiProcessor.GetApi<PresupuestoDto[]>("Presupuesto/GetAll"));
            Empresas = new ObservableCollection<EmpresaDto>(await ApiProcessor.GetApi<EmpresaDto[]>("Empresa/GetAll"));
            Obras = new ObservableCollection<ObraDto>(await ApiProcessor.GetApi<ObraDto[]>("Obra/GetAllN"));
        }
        protected async override Task CrearNuevoElemento()
        {
            if (!string.IsNullOrWhiteSpace(Presupuesto.Descripcion))
            {
                Presupuesto.Beneficio = 0m;
                Presupuesto.ImprevistoPorcentual = 0m;
                Presupuesto.Impuestos = 0m;
                Presupuesto.PrecioCliente = 0m;
                Presupuesto.SubTotal = 0m;
                Presupuesto.EstadoPresupuesto = Constantes.EstadoPresupuesto.Pendiente;
                Presupuesto.EmpresaId = Presupuesto.Empresa.Id;
                Presupuesto.ObraId = Presupuesto.Obra.Id;
                Presupuesto.EstadoDeCobro = EstadoDeCobro.SinCobrar;
                Presupuesto.FechaPresupuesto = DateTime.Now;
                Presupuesto.Numero = await ApiProcessor.GetApi<int>("Presupuesto/UltimoNumero");
                await ApiProcessor.PostApi(Presupuesto, "Presupuesto/Insert");
                await Inicializar();
                Presupuesto = new PresupuestoDto();
            }
        }
        protected async override Task EliminarElemento()
        {
            await Servicios.ApiProcessor.DeleteApi($"Presupuesto/{Presupuesto.Id}");
            await Inicializar();
        }
        protected async override Task EditarElemento()
        {
            if (!string.IsNullOrWhiteSpace(Presupuesto.Descripcion))
            {
                Presupuesto.EmpresaId = Presupuesto.Empresa.Id;
                await Servicios.ApiProcessor.PutApi(Presupuesto, $"Presupuesto/{Presupuesto.Id}");
                await Inicializar();
            }
        }

        protected override void Nuevo()
        {
            base.Nuevo();
            Presupuesto = new PresupuestoDto();
        }
        protected override void Cancelar()
        {
            base.Cancelar();
            Presupuesto = null;
        }
       
        public ObservableCollection<PresupuestoDto> Presupuestos
        {
            get { return _presupuestos; }
            set
            {
                SetProperty(ref _presupuestos, value);
                if (_presupuestos.Count == 0)
                {
                    SinPresupuesto = Visibility.Visible;
                }
                else
                {
                    SinPresupuesto = Visibility.Collapsed;
                }
            }
        }


        public Visibility SinPresupuesto { get { return _sinPresupuesto; }
            set
            {
                SetProperty(ref _sinPresupuesto, value);
            }
        }
        public PresupuestoInicioViewModel()
        {
            Presupuesto = null;
            CrearObraCommand = new DelegateCommand(Nuevo);
            CancelarCommand = new DelegateCommand(Cancelar);
            EditarObraCommand = new DelegateCommand(Editar, ()=> ObjetoNull.IsNull(Presupuesto)).ObservesProperty(() => Presupuesto);
            EliminarObraCommand = new DelegateCommand(Eliminar, ()=> ObjetoNull.IsNull(Presupuesto)).ObservesProperty(() => Presupuesto);
            CancelarPresupuesto= new DelegateCommand<PresupuestoDto>(CancelarPres);
            AprobarPresupuesto = new DelegateCommand(AprobarPres);
            PendientePrespuesto = new DelegateCommand<PresupuestoDto>(PendientePres);
        }

        private void CancelarPres(PresupuestoDto obj)
        {
            throw new NotImplementedException();
        }

        private void AprobarPres()
        {
            MessageBox.Show("Titulo");
        }

        private void PendientePres(PresupuestoDto obj)
        {
            throw new NotImplementedException();
        }
    }
}
