using GestionObraWPF.DTOs;
using GestionObraWPF.Helpers;
using GestionObraWPF.ViewModels.ABMs;
using GestionObraWPF.ViewModels.EventAgreggator;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
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
    public class PresupuestoDetalleViewModel : BaseABMViewModel , INavigationAware
    {
        private ObservableCollection<GastoDto> _gastos;
        private PresupuestoDto _presupuesto;
        private ObservableCollection<TipoGastoDto> _tipoGastos;
        private GastoDto _gasto;
        private decimal _dineroTotalGastos = 0;
        private decimal _dineroImprevistoPorcentual = 0;
        private decimal _sumaGastos;
        private decimal _dineroBeneficio;
        private decimal _dineroImpuesto;
        private decimal _totalObra;
        private decimal _subtotal;

        public DelegateCommand<string> ImpuestoChanged { get; set; }
        public DelegateCommand<string> TextChanged { get; set; }
        public DelegateCommand<string> BeneficioChanged { get; set; }
        public ICommand CargarPresupusto { get; set; }
        public decimal DineroImprevistoPorcentual {
            get { return _dineroImprevistoPorcentual; }
            set { SetProperty(ref _dineroImprevistoPorcentual, value);
               

            }
        }
        public ObservableCollection<GastoDto> Gastos { get { return _gastos; } set { SetProperty(ref _gastos, value);
                DineroTotalGastos = Gastos.Sum(x => x.Monto);
            }
        }

        public ObservableCollection<TipoGastoDto> TipoGastos { get { return _tipoGastos; } set { SetProperty(ref _tipoGastos, value); } }

        public PresupuestoDto Presupuesto { get { return _presupuesto; } set {

                SetProperty(ref _presupuesto, value);
            } }

        private IEventAggregator eventAggregator;
        private ObraDto _obra;

        public GastoDto Gasto { get { return _gasto; } set { SetProperty(ref _gasto, value); } }


        public decimal DineroTotalGastos { get { return _dineroTotalGastos; } set { SetProperty(ref _dineroTotalGastos, value);
                CalcularAspectos(Presupuesto.ImprevistoPorcentual);
            } }

        public decimal DineroImpuesto {
            get { return _dineroImpuesto; }
            set { SetProperty(ref _dineroImpuesto, value); }
        }
        public decimal TotalObra
        {
            get { return _totalObra; }
            set { SetProperty(ref _totalObra, value); }
        }

        public decimal SumaGastos
        {
            get {
                return _sumaGastos;
                }
            set { SetProperty(ref _sumaGastos, value);
            }
        }

        public decimal Subtotal
        {
            get { return _subtotal; }
            set { SetProperty(ref _subtotal, value);
            Presupuesto.SubTotal = Subtotal;
            }
        }

        public decimal DineroBeneficio {
            get { return _dineroBeneficio; }
            set { SetProperty(ref _dineroBeneficio, value); }
        }

        public ObraDto Obra
        {
            get { return _obra; }
            set { SetProperty(ref _obra, value); }
        }

        public ICommand AprobarPresupuesto { get; set; }
        public ICommand PendientePresupuesto { get;set; }
        public ICommand CancelarPresupuesto { get; set;}

        public PresupuestoDetalleViewModel(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            Gasto = null;
            TextChanged = new DelegateCommand<string>(PasandoTexto);
            ImpuestoChanged = new DelegateCommand<string>(ImpuestoAlMando);
            BeneficioChanged = new DelegateCommand<string>(BeneficioTexto);
            CrearObraCommand = new DelegateCommand(Nuevo);
            CargarPresupusto = new DelegateCommand(PresupuestoPut);
            CancelarCommand = new DelegateCommand(Cancelar);
            EditarObraCommand = new DelegateCommand(Editar, ()=> ObjetoNull.IsNull(Gasto)).ObservesProperty(() => Gasto);
            EliminarObraCommand = new DelegateCommand(Eliminar, ()=> ObjetoNull.IsNull(Gasto)).ObservesProperty(() => Gasto);
            AprobarPresupuesto = new DelegateCommand(Aprobar);
            PendientePresupuesto = new DelegateCommand(Pendiente);
            CancelarPresupuesto = new DelegateCommand(CancelarPresu);
        }

        private async void CancelarPresu()
        {
            Presupuesto.EstadoPresupuesto = Constantes.EstadoPresupuesto.Cancelado;
                await Servicios.ApiProcessor.PutApi(Presupuesto, $"Presupuesto/{Presupuesto.Id}");
              var obra=  await Servicios.ApiProcessor.GetApi<ObraDto>($"Obra/GetById/{Presupuesto.ObraId}");
                obra.EstadoObra = Constantes.EstadoObra.Pendiente;
                await Servicios.ApiProcessor.PutApi(obra, $"Obra/{obra.Id}");
            MessageBox.Show("El presupuesto fue cancelado");
            
        }

        private async void Pendiente()
        {
         
            Presupuesto.EstadoPresupuesto = Constantes.EstadoPresupuesto.Pendiente;
                await Servicios.ApiProcessor.PutApi(Presupuesto, $"Presupuesto/{Presupuesto.Id}");
                var obra = await Servicios.ApiProcessor.GetApi<ObraDto>($"Obra/GetById/{Presupuesto.ObraId}");
                obra.EstadoObra = Constantes.EstadoObra.Pendiente;
                await Servicios.ApiProcessor.PutApi(obra, $"Obra/{obra.Id}");
            MessageBox.Show("El presupuesto fue puesto en pendiente");

        }

        private async void Aprobar()
        {
            if (Presupuesto.PrecioCliente > 0)
            {
                Presupuesto.EstadoPresupuesto = Constantes.EstadoPresupuesto.Aprobado;
                await Servicios.ApiProcessor.PutApi(Presupuesto, $"Presupuesto/{Presupuesto.Id}");
                var obra = await Servicios.ApiProcessor.GetApi<ObraDto>($"Obra/GetById/{Presupuesto.ObraId}");
                obra.EstadoObra = Constantes.EstadoObra.Planificacion;
                await Servicios.ApiProcessor.PutApi(obra, $"Obra/{obra.Id}");
                MessageBox.Show("El presupuesto fue aprobado! la obra esta lista para ser planificada");
            }
            else
            {
                MessageBox.Show("Faltan cargar datos");
            }
        }

        private async void PresupuestoPut()
        {
            if (TotalObra > 0 && Presupuesto.PrecioCliente>0)
            {
                try
                {
                    await Servicios.ApiProcessor.PutApi(Presupuesto, $"Presupuesto/{Presupuesto.Id}");
                }
                catch (Exception)
                {

                    MessageBox.Show("Error de conexion");
                }
                MessageBox.Show("Datos cargados.");
            }
            else
            {
                MessageBox.Show("Faltan cargar datos");
            }
        }

        

        //TODO
        private void ImpuestoAlMando(string obj)
        {
            if (decimal.TryParse(obj, out decimal a))
            {
                DineroImpuesto = Subtotal * (a / 100m);
                TotalObra = DineroImpuesto + Subtotal;
            }
        }

        //TODO
        private void BeneficioTexto(string obj)
        {
            if (decimal.TryParse(obj, out decimal a))
            {
                DineroBeneficio = SumaGastos * (a / 100m);
                Subtotal = DineroBeneficio + SumaGastos;
                DineroImpuesto = Subtotal * (Presupuesto.Impuestos/ 100m);
                TotalObra = DineroImpuesto + Subtotal;
            }
        }

        private void PasandoTexto(string obj)
        {
            if(decimal.TryParse(obj,out decimal a))
            {
                CalcularAspectos(a);
            }
        }

        private void CalcularAspectos(decimal a)
        {
            DineroImprevistoPorcentual = DineroTotalGastos * (a / 100m);
            SumaGastos = DineroImprevistoPorcentual + DineroTotalGastos;
            DineroBeneficio = SumaGastos * (Presupuesto.Beneficio / 100m);
            Subtotal = DineroBeneficio + SumaGastos;
            DineroImpuesto = Subtotal * (Presupuesto.Impuestos / 100m);
            TotalObra = DineroImpuesto + Subtotal;
        }

        public async override Task Inicializar()
        {
            Gastos = new ObservableCollection<GastoDto>(await Servicios.ApiProcessor.GetApi<GastoDto[]>($"Gasto/GetByPresupuesto/{Presupuesto.Id}"));
            TipoGastos = new ObservableCollection<TipoGastoDto>(await Servicios.ApiProcessor.GetApi<TipoGastoDto[]>($"TipoGasto/GetAll"));
            eventAggregator.GetEvent<ObraAgreggator>().Publish(Presupuesto.Obra);
        }


        protected async override Task CrearNuevoElemento()
        {
            if (Gasto.TipoGasto!=null)
            {
                Gasto.PresupuestoId = Presupuesto.Id;
                Gasto.TipoGastoId = Gasto.TipoGasto.Id;
                await Servicios.ApiProcessor.PostApi(Gasto, "Gasto/Insert");
                await Inicializar();
                Gasto = new GastoDto();
            }
        }
        protected async override Task EliminarElemento()
        {
            await Servicios.ApiProcessor.DeleteApi($"Gasto/{Gasto.Id}");
            await Inicializar();
        }
        protected async override Task EditarElemento()
        {
            if (Gasto.TipoGasto != null)
            {
                await Servicios.ApiProcessor.PutApi(Gasto, $"Gasto/{Gasto.Id}");
                await Inicializar();
            }
        }

        protected override void Nuevo()
        {
            base.Nuevo();
            Gasto = new GastoDto();
        }
        protected override void Cancelar()
        {
            base.Cancelar();
            Gasto = null;
        }
      

        public async void OnNavigatedTo(NavigationContext navigationContext)
        {
            Presupuesto = (PresupuestoDto)navigationContext.Parameters["Presupuesto"];
            
            await Inicializar();
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public  void OnNavigatedFrom(NavigationContext navigationContext)
        {
            
        }
    }
}
