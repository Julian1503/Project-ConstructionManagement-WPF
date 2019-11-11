using GestionObraWPF.Constantes;
using GestionObraWPF.DTOs;
using GestionObraWPF.Servicios;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace GestionObraWPF.ViewModels
{
    public class VentasAdministracionViewModel : BindableBase
    {
        public VentasAdministracionViewModel()
        {

            this.eventAggregator = eventAggregator;
            this.regionManager = regionManager;
            Command = new DelegateCommand<PresupuestoDto>(DobleClick);
            Presupuesto = null;
            SacarPantalla = new DelegateCommand(SacarPantallas);
            CrearCommand = new DelegateCommand(Facturar);
            Cerrar = new DelegateCommand(CerrarOpc);
            CancelarBlancoCommand = new DelegateCommand(CancelarBlanco);
            CancelarNegroCommand = new DelegateCommand(CancelarNegro);
            PagoSinComprobante = new DelegateCommand(SinComprobanteAbrir);
            PagoConComprobante = new DelegateCommand(ConComprobanteAbrir);
        }

     
        private void CerrarOpc()
        {
            MostrarOpc = false;
        }

        private void CancelarNegro()
        {
            MostarNegro = false;
            Descuento = 0; Recargos = 0;
            Presupuesto = new PresupuestoDto();
        }

        private void CancelarBlanco()
        {
            MostrarBlanco = false;
            Descuento = 0; Recargos = 0; Retencion = 0; IVA = 0; Percepcion = 0;
            Presupuesto = new PresupuestoDto();
        }


        private async void Facturar()
        {
            
            Presupuesto.Iva = IVA;
            Presupuesto.Descuento = Descuento;
            Presupuesto.Interes = Recargos;
            Presupuesto.Percepciones = Percepcion;
            Presupuesto.Retenciones = Retencion;
            Presupuesto.FechaFacturacion = Fecha;
            Presupuesto.Cae = Cae;
            Presupuesto.Facturado = true;
            Presupuesto.NumeroFacturacion = NumeroComprobante;
            await ApiProcessor.PutApi(Presupuesto, $"Presupuesto/{Presupuesto.Id}");
            await Inicializar();
            MostarNegro = false;
            MostrarBlanco = false;
           
        }

        private PresupuestoDto _presupuesto;
        private ObservableCollection<PresupuestoDto> _presupuestos;
        private Visibility _sinPresupuesto;
        private IEventAggregator eventAggregator;
        private bool _mostrarOpc;

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
        public ICommand CancelarBlancoCommand { get; set; }
        public ICommand CancelarNegroCommand { get; set; }
        public ICommand CrearCommand { get; set; }
        public ICommand Cerrar { get; set; }
        public ICommand PagoSinComprobante { get; set; }
        public ICommand PagoConComprobante { get; set; }
        public ICommand SacarPantalla { get; set; }
        public IRegionManager regionManager { get; set; }
        private decimal _dineroTotalGastos = 0;
        private decimal _dineroImprevistoPorcentual = 0;
        private decimal _sumaGastos;
        private decimal _dineroBeneficio;
        private decimal _dineroImpuesto;
        private decimal _totalObra;
        private decimal _subtotal;
        private decimal _total;
        private decimal _descuento;
        private decimal _IVA;
        private decimal _retencion;
        private decimal _percepcion;
        private decimal _recargos;
        private bool _mostrarBlanco;
        private bool _mostrarNegro;
        private bool _vender;
        private bool _blanco;
        private bool _negro;
        private long? _numeroComprobante;
        public long? NumeroComprobante
        {
            get { return _numeroComprobante; }
            set { SetProperty(ref _numeroComprobante, value); }
        }
        private long? _cae;
        public long? Cae
        {
            get { return _cae; }
            set { SetProperty(ref _cae, value); }
        }
        private DateTime _fecha = DateTime.Now;
        public DateTime Fecha
        {
            get { return _fecha; }
            set { SetProperty(ref _fecha, value); }
        }
      
     
        public decimal DineroImprevistoPorcentual
        {
            get { return _dineroImprevistoPorcentual; }
            set
            {
                SetProperty(ref _dineroImprevistoPorcentual, value);
            }
        }
        public decimal DineroTotalGastos
        {
            get { return _dineroTotalGastos; }
            set
            {
                SetProperty(ref _dineroTotalGastos, value);
            }
        }

        public decimal DineroImpuesto
        {
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
            get
            {
                return _sumaGastos;
            }
            set
            {
                SetProperty(ref _sumaGastos, value);
            }
        }
        public decimal Total
        {
            get { return _total; }
            set
            {
                SetProperty(ref _total, value);
            }
        }
        public decimal IVA
        {
            get { return _IVA; }
            set
            {
                SetProperty(ref _IVA, value);
                CalcularTotal();
            }
        }
        public decimal Retencion
        {
            get { return _retencion; }
            set
            {
                SetProperty(ref _retencion, value);
                CalcularTotal();

            }
        }
        public decimal Percepcion
        {
            get { return _percepcion; }
            set
            {
                SetProperty(ref _percepcion, value);
                CalcularTotal();
            }
        }
        public decimal Recargos
        {
            get { return _recargos; }
            set
            {
                SetProperty(ref _recargos, value);
                CalcularTotal();
            }
        }
        public decimal Descuento
        {
            get { return _descuento; }
            set
            {
                SetProperty(ref _descuento, value);
                CalcularTotal();
            }
        }

        private void CalcularTotal()
        {
            Total = ((Presupuesto.PrecioCliente - Descuento) + Recargos) + Retencion + IVA + Percepcion;
        }

        public decimal Subtotal
        {
            get { return _subtotal; }
            set
            {
                SetProperty(ref _subtotal, value);
                Presupuesto.SubTotal = Subtotal;
            }
        }

        public decimal DineroBeneficio
        {
            get { return _dineroBeneficio; }
            set { SetProperty(ref _dineroBeneficio, value); }
        }

        private async void DobleClick(PresupuestoDto obj)
        {
            if (obj != null)
            {
                if (obj.EstadoDeCobro == Constantes.EstadoDeCobro.SinCobrar)
                {
                    MostrarOpc = true;
                    Presupuesto = obj;
                    IVA = Presupuesto.Iva;
                    Cae = Presupuesto.Cae;
                    NumeroComprobante = Presupuesto.NumeroFacturacion;
                    Descuento = Presupuesto.Descuento;
                    Recargos = Presupuesto.Interes;
                    Percepcion = Presupuesto.Percepciones;
                    Retencion = Presupuesto.Retenciones;
                    var Gastos = await ApiProcessor.GetApi<GastoDto[]>($"Gasto/GetByPresupuesto/{obj.Id}");
                    DineroTotalGastos = Gastos.Sum(x => x.Monto);
                    DineroImprevistoPorcentual = DineroTotalGastos * (Presupuesto.ImprevistoPorcentual / 100m);
                    SumaGastos = DineroImprevistoPorcentual + DineroTotalGastos;
                    DineroBeneficio = SumaGastos * (Presupuesto.Beneficio / 100m);
                    Subtotal = DineroBeneficio + SumaGastos;
                    DineroImpuesto = Subtotal * (Presupuesto.Impuestos / 100m);
                    TotalObra = DineroImpuesto + Subtotal;
                    CalcularTotal();
                    if (Presupuesto.Cobrado == 0)
                    {
                        Blanco = true;
                        Negro = true;
                    }
                    else
                    {
                        if (Presupuesto.Cobrado > 0 && (Presupuesto.Iva <= 0 || Presupuesto.Retenciones <= 0 || Presupuesto.Percepciones <= 0))
                        {
                            Blanco = false;
                            Negro = true;
                        }
                        else
                        {
                            Blanco = true;
                            Negro = false;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Presupuesto ya cobrado");
                }
            }
        }

        public async Task Inicializar()
        {
            Presupuestos = new ObservableCollection<PresupuestoDto>(await ApiProcessor.GetApi<PresupuestoDto[]>("Presupuesto/GetAprobado"));
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


        public Visibility SinPresupuesto
        {
            get { return _sinPresupuesto; }
            set
            {
                SetProperty(ref _sinPresupuesto, value);
            }
        }
        
        public bool MostarNegro { get { return _mostrarNegro; } set { SetProperty(ref _mostrarNegro, value); } }

        public bool MostrarOpc { get { return _mostrarOpc; } set { SetProperty(ref _mostrarOpc, value); } }

        public bool MostrarBlanco { get { return _mostrarBlanco; } set { SetProperty(ref _mostrarBlanco, value); } }

        public bool Vender { get { return _vender; } set { SetProperty(ref _vender, value); } }

   

        public bool Blanco { get { return _blanco; } set { SetProperty(ref _blanco, value); } }
        public bool Negro { get { return _negro; } set { SetProperty(ref _negro, value); } }

        private void SinComprobanteAbrir()
        {
            MostrarOpc = false;
            MostarNegro = true;
        }

        private void ConComprobanteAbrir()
        {
            MostrarOpc = false;
            MostrarBlanco = true;

        }

        private void SacarPantallas()
        {
            MostrarOpc = false;
        }
    }
}
