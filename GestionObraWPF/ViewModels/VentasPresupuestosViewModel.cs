using GestionObraWPF.Constantes;
using GestionObraWPF.DTOs;
using GestionObraWPF.Model;
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
    public class VentasPresupuestosViewModel : BindableBase
    {
        public VentasPresupuestosViewModel(IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            this.regionManager = regionManager;
            Command = new DelegateCommand<PresupuestoDto>(DobleClick);
            Presupuesto = null;
            Operacion = new OperacionDto();
            SacarPantalla = new DelegateCommand(SacarPantallas);
            CargarTransferencia = new DelegateCommand(PagoTransferencia);
            CargarCheque = new DelegateCommand(PagoCheque);
            CargarDeposito = new DelegateCommand(PagoDeposito);
            CrearCommand = new DelegateCommand(Facturar);
            CobrarEfectivo = new DelegateCommand(CobrarEfec);
            AbrirFormaPago = new DelegateCommand<string>(FormaPagoOpen);
            CancelarFormaPago = new DelegateCommand<string>(CerrarFormaPago);
            Cerrar = new DelegateCommand(CerrarOpc);
            CancelarBlancoCommand = new DelegateCommand(CancelarBlanco);
            CancelarNegroCommand = new DelegateCommand(CancelarNegro);
            PagoSinComprobante = new DelegateCommand(SinComprobanteAbrir);
            PagoConComprobante = new DelegateCommand(ConComprobanteAbrir);
        }

        private async void CobrarEfec()
        {
            if (await Servicios.ApiProcessor.GetApi<bool>("Caja/CajasEstado"))
            {
                if (ComprobanteEntrada.Monto > 0 && ComprobanteEntrada.TipoComprobanteEntrada > 0)
                {
                    ComprobanteEntrada.Fecha = Fecha;
                    ComprobanteEntrada.UsuarioId = UsuarioGral.UsuarioId;
                    ComprobanteEntrada.Usado = UsadoEn.Venta;
                    await ApiProcessor.PostApi<ComprobanteEntradaDto>(ComprobanteEntrada, "ComprobanteEntrada/Insert");
                    var Caja = await ApiProcessor.GetApi<CajaDto>("Caja/CajaAbierta");
                    var detalleCaja = new DetalleCajaDto
                    {
                        CajaId = Caja.Id,
                        Monto = ComprobanteEntrada.Monto,
                        TipoMovimiento = TipoMovimiento.Ingreso,
                        TipoPago = TipoPago.Efectivo
                    };
                    await Servicios.ApiProcessor.PostApi<DetalleCajaDto>(detalleCaja, "DetalleCaja/Insert");
                    ComprobanteEntrada = new ComprobanteEntradaDto();
                    Presupuesto.Cobrado += Cobrar;
                    if (Total == Presupuesto.Cobrado)
                    {
                        Presupuesto.EstadoDeCobro = Constantes.EstadoDeCobro.Cobrado;
                    }
                    await ApiProcessor.PutApi(Presupuesto, $"Presupuesto/{Presupuesto.Id}");
                    Cobrar = 0;
                    if (Presupuesto.EstadoDeCobro == EstadoDeCobro.Cobrado)
                    {
                        await Inicializar();
                    }
                    Presupuesto = new PresupuestoDto();
                    Efectivo = false;
                }
                else
                {
                    MessageBox.Show("Faltan datos que rellenar");
                }
            }
            else
            {
                MessageBox.Show("Por favor abra la caja");
            }
        }

        private async  void PagoDeposito()
        {
            if (Operacion.Debe > 0 && Banco != null)
            {
                var cuentaCorriente = await ApiProcessor.GetApi<CuentaCorrienteDto>($"CuentaCorriente/Banco/{Banco.Id}");
                Operacion.CuentaCorrienteId = cuentaCorriente.Id;
                Operacion.FechaEmision = DateTime.Now;
                Operacion.FechaVencimiento = DateTime.Now;
                Operacion.TipoOperacion = TipoOperacion.Deposito;
                Operacion.DePara = Cliente.RazonSocial;
                Operacion.Haber = 0;
                Operacion.Referencia = 0;
                Operacion.ReferenciaPlus = "";
                await ApiProcessor.PostApi(Operacion, "Operacion/Insert");
                var deposito = new DepositoDto();
                deposito.BancoId = Banco.Id;
                deposito.Usado = UsadoEn.Venta;
                deposito.Entrada = true;
                deposito.Fecha = (DateTime)Operacion.FechaEmision;
                deposito.Concepto = Operacion.Concepto;
                deposito.DePara = Operacion.DePara;
                deposito.Numero = long.Parse(Operacion.CodigoCausal);
                deposito.Monto = (decimal)Operacion.Debe;
                await ApiProcessor.PostApi(deposito, "Deposito/Insert");
                Operacion = new OperacionDto();
                Presupuesto.Cobrado += Cobrar;
                if (Total == Presupuesto.Cobrado)
                {
                    Presupuesto.EstadoDeCobro = Constantes.EstadoDeCobro.Cobrado;
                }
                await ApiProcessor.PutApi(Presupuesto, $"Presupuesto/{Presupuesto.Id}");
                Cobrar = 0;
                if (Presupuesto.EstadoDeCobro == EstadoDeCobro.Cobrado)
                {
                   await Inicializar();
                }
                Presupuesto = new PresupuestoDto();
                Deposito = false;
                MessageBox.Show("La operacion a sido registrado con exito!");
            }
            else
            {
                MessageBox.Show("Faltan datos para realizar la carga!");
            }
        }

        private DateTime _fecha = DateTime.Now;
        public DateTime Fecha
        {
            get { return _fecha; }
            set { SetProperty(ref _fecha, value); }
        }
        private async void PagoCheque()
        {
            if (ChequeEntrada.Banco != null && ChequeEntrada.Monto > 0 && !string.IsNullOrWhiteSpace(ChequeEntrada.Serie) && (Dia > 0 && Dia < 32) && (Mes > 0 && Mes < 13) && Ano > 1900 && !string.IsNullOrWhiteSpace(ChequeEntrada.EmitidoPor) && ChequeEntrada.Numero > 0)
            {
                ChequeEntrada.FechaHasta = new DateTime(Ano, Mes, Dia);
                ChequeEntrada.BancoId = ChequeEntrada.Banco.Id;
                ChequeEntrada.Usado = UsadoEn.Venta;
                var cuentaCorriente = await ApiProcessor.GetApi<CuentaCorrienteDto>($"CuentaCorriente/Banco/{ChequeEntrada.BancoId}");
                await ApiProcessor.PostApi<ChequeEntradaDto>(ChequeEntrada, "ChequeEntrada/Insert");
                var op = new OperacionDto
                {
                    CuentaCorrienteId = cuentaCorriente.Id,
                    Debe = ChequeEntrada.Monto,
                    EstaEnResumen = true,
                    FechaEmision = ChequeEntrada.FechaDesde,
                    TipoOperacion = Constantes.TipoOperacion.Cheque,
                    DePara = ChequeEntrada.EmitidoPor,
                    Descontado = ChequeEntrada.MontoContado,
                    Concepto = ChequeEntrada.Concepto,
                    FechaVencimiento = ChequeEntrada.FechaHasta,
                    CodigoCausal = ChequeEntrada.Serie,
                    Haber = 0,
                    Referencia = ChequeEntrada.Numero,
                    ReferenciaPlus = $"{ ChequeEntrada.Numero}"
                };
                await ApiProcessor.PostApi<OperacionDto>(op, "Operacion/Insert");
                ChequeEntrada = new ChequeEntradaDto();
                Dia = Mes = Ano = 0;
                Presupuesto.Cobrado += Cobrar;
                if (Total == Presupuesto.Cobrado)
                {
                    Presupuesto.EstadoDeCobro = Constantes.EstadoDeCobro.Cobrado;
                }
                await ApiProcessor.PutApi(Presupuesto, $"Presupuesto/{Presupuesto.Id}");
                Cobrar = 0;
                if (Presupuesto.EstadoDeCobro == EstadoDeCobro.Cobrado)
                {
                    await Inicializar();
                }
                Presupuesto = new PresupuestoDto();
                Cheque = false;
                MessageBox.Show("El cheque a sido registrado con exito!");
            }
            else
            {
                MessageBox.Show("Faltan datos para realizar la carga!");
            }
        }

        private async void PagoTransferencia()
        {
            if (Operacion.Debe > 0 && Banco != null)
            {
                var cuentaCorriente = await ApiProcessor.GetApi<CuentaCorrienteDto>($"CuentaCorriente/Banco/{Banco.Id}");
                Operacion.CuentaCorrienteId = cuentaCorriente.Id;
                Operacion.FechaEmision = DateTime.Now;
                Operacion.FechaVencimiento = DateTime.Now;
                Operacion.TipoOperacion = TipoOperacion.Tranferencia;
                Operacion.DePara = Cliente.RazonSocial;
                Operacion.Haber = 0;
                Operacion.Referencia = 0;
                Operacion.ReferenciaPlus = "";
                await ApiProcessor.PostApi(Operacion, "Operacion/Insert");
                Operacion = new OperacionDto();
                var transferencia = new TransferenciaDto();
                transferencia.BancoId = Banco.Id;
                transferencia.Usado = UsadoEn.Venta;
                transferencia.Entrada = false;
                transferencia.ImpuestoBancario = Impuesto;
                transferencia.Concepto = Operacion.Concepto;
                transferencia.Fecha = (DateTime)Operacion.FechaEmision;
                transferencia.PagueseA = Cliente.RazonSocial;
                transferencia.Numero = long.Parse(Operacion.CodigoCausal);
                transferencia.Monto = Total;
                await ApiProcessor.PostApi(transferencia, "Transferencia/Insert");
                Presupuesto.Cobrado += Cobrar;
                if (Total == Presupuesto.Cobrado)
                {
                    Presupuesto.EstadoDeCobro = Constantes.EstadoDeCobro.Cobrado;
                }

                await ApiProcessor.PutApi(Presupuesto, $"Presupuesto/{Presupuesto.Id}");
                Cobrar = 0;
                if (Presupuesto.EstadoDeCobro == EstadoDeCobro.Cobrado)
                {
                    await Inicializar();
                }
                Presupuesto = new PresupuestoDto();
                Transferencia = false;
                MessageBox.Show("La operacion a sido registrado con exito!");
            }
            else
            {
                MessageBox.Show("Faltan datos para realizar la carga!");
            }
        }

        private async void FormaPagoOpen(string nombreBoton)
        {
            Vender = false;
            Cliente = Presupuesto.Obra.Propietario;
            switch (nombreBoton)
            {
                case "Cheque":
                    Cheque = true;
                    ChequeEntrada = new ChequeEntradaDto();
                    ChequeEntrada.Monto = Cobrar;
                    ChequeEntrada.Concepto = $"Pago de la obra {Presupuesto.Obra.Codigo}-{Presupuesto.Obra.Descripcion}";
                    break;
                case "Efectivo":
                    Efectivo = true;
                    ComprobanteEntrada = new ComprobanteEntradaDto();
                    ComprobanteEntrada.Monto = Cobrar;
                    ComprobanteEntrada.Detalle = $"Pago de la obra {Presupuesto.Obra.Codigo}-{Presupuesto.Obra.Descripcion}";
                    break;
                case "Transferencia":
                    Transferencia = true;
                    Operacion = new OperacionDto();
                    Subtotal = Cobrar;
                    Operacion.Concepto = $"Pago de la obra {Presupuesto.Obra.Codigo}-{Presupuesto.Obra.Descripcion}";
                    break;
                case "Deposito":
                    Deposito= true;
                    Operacion = new OperacionDto();
                    Operacion.Debe = Cobrar;
                    Operacion.Concepto = $"Pago de la obra {Presupuesto.Obra.Codigo}-{Presupuesto.Obra.Descripcion}";
                    break;
            }
        }

        private async void VerificarVenta(bool obj)
        {
            if (obj)
            {
                Presupuesto.Cobrado += Cobrar;
                if (Total == Presupuesto.Cobrado)
                {
                    Presupuesto.EstadoDeCobro = Constantes.EstadoDeCobro.Cobrado;
                }

                await ApiProcessor.PutApi(Presupuesto, $"Presupuesto/{Presupuesto.Id}");
                Efectivo = false;
                Cobrar = 0;
                if (Presupuesto.EstadoDeCobro == EstadoDeCobro.Cobrado)
                {
                    await Inicializar();
                }
                Presupuesto = new PresupuestoDto();
                MessageBox.Show("Operacion realizada!");

            }
            else
            {
                Efectivo = false;
            }
             eventAggregator.GetEvent<PubSubEvent<bool>>().Unsubscribe(VerificarVenta);
        }

        private void CerrarFormaPago(string nombreBoton)
        {
            Vender = false;
            Cobrar = 0;
            switch (nombreBoton)
            {
                case "Cheque":
                    Cheque = false;
                    ChequeEntrada = new ChequeEntradaDto();
                    break;
                case "Efectivo":
                    Efectivo = false;
                    break;
                case "Transferencia":
                    Transferencia = false;
                    Operacion = new OperacionDto();
                    break;
                case "Deposito":
                    Deposito = false;
                    Operacion = new OperacionDto();
                    break;
            }
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
            Cobrar = 0;
            Descuento = 0; Recargos = 0; Retencion = 0; IVA = 0; Percepcion = 0; Cobrar = 0;
            Presupuesto = new PresupuestoDto();
        }


        private void Facturar()
        {
            if (Presupuesto.Cobrado + Cobrar <= Total && Cobrar>0)
            {
                MostarNegro = false;
                MostrarBlanco = false;
                Vender = true;
            }
            else
            {
                MessageBox.Show("El monto que desea cobrar es incorrecto");
            }
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
        public DelegateCommand<PresupuestoDto> CancelarPresupuesto { get; set; }
        public ICommand CancelarBlancoCommand { get; set; }
        public ICommand CancelarNegroCommand { get; set; }
        public DelegateCommand CargarCheque { get; }
        public DelegateCommand CargarDeposito { get; }
        public ICommand CrearCommand { get; set; }
        public DelegateCommand CobrarEfectivo { get; }
        public DelegateCommand<string> AbrirFormaPago { get; set;}
        public DelegateCommand<string> CancelarFormaPago { get; set;}
        public ICommand Cerrar { get; set; }
        private ComprobanteEntradaDto _comprobanteEntrada;
        public ComprobanteEntradaDto ComprobanteEntrada
        {
            get { return _comprobanteEntrada; }
            set { SetProperty(ref _comprobanteEntrada, value); }
        }
        public ICommand PagoSinComprobante { get; set; }
        public ICommand PagoConComprobante { get; set; }
        public ICommand SacarPantalla { get; set; }
        public DelegateCommand CargarTransferencia { get; }
        public ICommand AprobarPresupuesto { get; set; }
        public DelegateCommand<PresupuestoDto> PendientePrespuesto { get; set; }
        public IRegionManager regionManager { get; set; }
        private decimal _dineroTotalGastos = 0;
        private decimal _dineroImprevistoPorcentual = 0;
        private decimal _sumaGastos;
        private decimal _dineroBeneficio;
        private decimal _dineroImpuesto;
        private decimal _totalObra;
        private decimal _subtotal;
        private decimal _cobrar;
        private decimal _total;
        private decimal _descuento;
        private decimal _IVA;
        private decimal _retencion;
        private decimal _percepcion;
        private decimal _recargos;
        private bool _mostrarBlanco;
        private bool _mostrarNegro;
        private bool _vender;
        private bool _cheque;
        private bool _efectivo;
        private bool _transferencia;
        private bool _deposito;
        private ObservableCollection<BancoDto> _bancos;
        private BancoDto _banco;
        private OperacionDto _operacion;
        private ChequeEntradaDto _chequeEntrada;
        private int _dia;
        private int _mes;
        private int _ano;
        private EmpresaDto _cliente;
        private ObservableCollection<EmpresaDto> _clientes;
        private bool _blanco;
        private bool _negro;
        private decimal _subtotalTrans;
        public decimal SubtotalTrans
        {
            get { return _subtotalTrans; }
            set { SetProperty(ref _subtotalTrans, value); CalcularTransferencia(); }
        }

        private decimal _impuesto;
        public decimal Impuesto
        {
            
            get { return _impuesto; }
            set { SetProperty(ref _impuesto, value); CalcularTransferencia(); }
        }

        private void CalcularTransferencia()
        {
            Operacion.Debe = Impuesto + SubtotalTrans;
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
        public decimal Cobrar
        {
            get { return _cobrar; }
            set
            {
                SetProperty(ref _cobrar, value);
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
                    if(Presupuesto.Cobrado == 0)
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
            Clientes = new ObservableCollection<EmpresaDto>(await ApiProcessor.GetApi<EmpresaDto[]>("Empresa/GetAll"));
            Bancos = new ObservableCollection<BancoDto>(await ApiProcessor.GetApi<BancoDto[]>("Banco/GetAll"));
            Presupuestos = new ObservableCollection<PresupuestoDto>(await ApiProcessor.GetApi<PresupuestoDto[]>("Presupuesto/GetFacturado"));
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
        public ObservableCollection<BancoDto> Bancos { get { return _bancos; } set { SetProperty(ref _bancos, value); } }

        public ObservableCollection<EmpresaDto> Clientes { get { return _clientes; } set { SetProperty(ref _clientes, value); } }

        public ChequeEntradaDto ChequeEntrada { get { return _chequeEntrada; } set { SetProperty(ref _chequeEntrada, value); } }

        public EmpresaDto Cliente { get { return _cliente; } set { SetProperty(ref _cliente, value); } }

        public BancoDto Banco { get { return _banco; } set { SetProperty(ref _banco, value); } }

        public OperacionDto Operacion { get { return _operacion; } set { SetProperty(ref _operacion, value); } }

        public bool MostarNegro { get { return _mostrarNegro; } set { SetProperty(ref _mostrarNegro, value); } }

        public bool MostrarOpc { get { return _mostrarOpc; } set { SetProperty(ref _mostrarOpc, value); } }

        public bool MostrarBlanco { get { return _mostrarBlanco; } set { SetProperty(ref _mostrarBlanco, value); } }

        public bool Vender { get { return _vender; } set { SetProperty(ref _vender, value); } }

        public bool Cheque { get { return _cheque; } set { SetProperty(ref _cheque, value); } }

        public bool Efectivo { get { return _efectivo; } set { SetProperty(ref _efectivo, value); } }

        public bool Transferencia { get { return _transferencia; } set { SetProperty(ref _transferencia, value); } }

        public bool Deposito { get { return _deposito; } set { SetProperty(ref _deposito, value); } }

        public int Dia { get { return _dia; } set { SetProperty(ref _dia, value); }  }
        public int Mes { get { return _mes; } set { SetProperty(ref _mes, value); }  }
        public int Ano { get { return _ano; } set { SetProperty(ref _ano, value); } }

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
