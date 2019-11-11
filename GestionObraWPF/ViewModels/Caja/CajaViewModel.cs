using GestionObraWPF.DTOs;
using GestionObraWPF.Model;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace GestionObraWPF.ViewModels
{
    public class CajaViewModel : BindableBase
    {
        private ComprobanteEntradaDto _comprobanteEntrada;
        private ObservableCollection<SubRubroDto> _subRubroEntrada;
        private Visibility _visibilidad = Visibility.Collapsed;

        public bool CerrarCajaPopUp { get { return _cerrarCajaPopUp; } set { SetProperty(ref _cerrarCajaPopUp, value); } }
        public decimal MontoSistema { get { return _montoSistema; } set { SetProperty(ref _montoSistema, value); } }
        public decimal Diferencia { get { return _diferencia; } set { SetProperty(ref _diferencia, value); } }
        public decimal Extraccion { get { return _extraccion; } set { SetProperty(ref _extraccion, value); } }
        public decimal Deposito { get { return _deposito; } set { SetProperty(ref _deposito, value); } }
        public decimal MontoCierre
        {
            get { return _montoCierre; }
            set
            {
                SetProperty(ref _montoCierre, value);
                Diferencia = MontoCierre - MontoSistema;
            }
        }

        public CajaDto Caja { get { return _caja; } set { SetProperty(ref _caja, value); } }
        public bool AbrirCajaPopUp { get { return _abrirCajaPopUp; } set { SetProperty(ref _abrirCajaPopUp, value); } }
        public ObservableCollection<DetalleCajaDto> DetallesCaja { get { return _detallesCaja; } set { SetProperty(ref _detallesCaja, value); } }
        public CajaViewModel(IEventAggregator eventAggregator, IRegionManager regionManager)
        {
            this.regionManager = regionManager;
            this.eventAggregator = eventAggregator;
            CrearCommand = new DelegateCommand(CrearCaja);
            CrearCerrarCaja = new DelegateCommand(CerrandoCaja);
            //AbrirExtraccion = new DelegateCommand(Extraer);
            CancelarPop = new DelegateCommand<string>(CancelarPopUp);
            ExtraerDinero = new DelegateCommand(ExtraerDineros);
            Depositar = new DelegateCommand(DepositarDinero);
            CancelarCommand = new DelegateCommand(Cancelar);
            ComprobanteEntrada = new ComprobanteEntradaDto();
            Caja = new CajaDto();
            CerrarCerrarCaja = new DelegateCommand(CerrarCajaPop);
            AbrirCerrarCaja = new DelegateCommand(CerrarCerrarCajaPop);
            AbrirEfectivoEntrada = new DelegateCommand(AbrirEfecEntrada);
            AbrirCajaCommand = new DelegateCommand(AbrirCajaPop);
            AbrirEfectivoSalida = new DelegateCommand(AbrirEfecSalida);
            AbrirChequeSalida = new DelegateCommand(AbrirCheSalida);
            AbrirChequeEntrada = new DelegateCommand(AbrirCheEntrada);
            eventAggregator.GetEvent<PubSubEvent<Dictionary<string, bool>>>().Subscribe(CerrarPops);
            Diferencia = MontoCierre - MontoSistema;
        }

        private async void DepositarDinero()
        {
            if (Deposito > 0)
            {
                var caja = await Servicios.ApiProcessor.GetApi<CajaDto>("Caja/CajaAbierta");
                var detalle = new DetalleCajaDto()
                {
                    TipoMovimiento = Constantes.TipoMovimiento.Ingreso,
                    Monto = Deposito,
                    CajaId = caja.Id,
                    TipoPago = Constantes.TipoPago.Efectivo
                };
                await Servicios.ApiProcessor.PostApi(detalle, $"DetalleCaja/Insert");
                DepositarPopUp = false;
            }
        }

        private async void ExtraerDineros()
        {
            if (Extraccion > 0)
            {
                var caja = await Servicios.ApiProcessor.GetApi<CajaDto>("Caja/CajaAbierta");
                var detalle = new DetalleCajaDto()
                {
                    TipoMovimiento = Constantes.TipoMovimiento.Egreso,
                    Monto = Extraccion,
                    CajaId = caja.Id,
                    TipoPago = Constantes.TipoPago.Efectivo
                };
                await Servicios.ApiProcessor.PostApi(detalle, $"DetalleCaja/Insert");
                ExtraccionPopUp = false;
            }
        }

        private void CancelarPopUp(string nombre)
        {
            switch (nombre)
            {
                case "Extraccion":
                    ExtraccionPopUp = !ExtraccionPopUp;
                    Extraccion = 0;
                    break;
                case "Deposito":
                    DepositarPopUp = !DepositarPopUp;
                    Deposito = 0;
                    break;
            }
        }

        //private void Deposiar()
        //{
        //    DepositarPopUp = true;
        //    Extraccion = 0;
        //}

        //private void Extraer()
        //{
        //    ExtraccionPopUp = true;
        // Deposito = 0;
        //}

        private async void CerrandoCaja()
        {
            Caja.MontoSistema = MontoSistema;
            Caja.Diferencia = Diferencia;
            Caja.MontoCierre = MontoCierre;
            Caja.UsuarioCierreId = UsuarioGral.UsuarioId;
            await Servicios.ApiProcessor.PostApi(Caja, "Caja/CerrarCaja");
            CerrarCajaVisibilidad = Visibility.Collapsed;
            AbrirCajaVisibilidad = Visibility.Visible;
            CerrarCajaPopUp = false;
            Caja = new CajaDto();
        }

        private async void CerrarCerrarCajaPop()
        {
            CerrarCajaPopUp = true;
            Caja = await Servicios.ApiProcessor.GetApi<CajaDto>("Caja/CajaAbierta");
            DetallesCaja = new ObservableCollection<DetalleCajaDto>(await Servicios.ApiProcessor.GetApi<DetalleCajaDto[]>($"DetalleCaja/GetByCaja/{Caja.Id}"));
            if (DetallesCaja.Count > 0)
            {
                Egreso = DetallesCaja.Where(x => x.TipoMovimiento == Constantes.TipoMovimiento.Egreso).Sum(x => x.Monto);
                Ingreso = DetallesCaja.Where(x => x.TipoMovimiento == Constantes.TipoMovimiento.Ingreso).Sum(x => x.Monto);
                MontoSistema = Ingreso-Egreso;
            }
            Caja = new CajaDto();
        }

        private void CerrarCajaPop()
        {
            CerrarCajaPopUp = false;
            Caja = new CajaDto();
        }

        private async void CrearCaja()
        {
            DetalleCajaDto detalle;
            Caja.UsuarioAperturaId = UsuarioGral.UsuarioId;
            await Servicios.ApiProcessor.PostApi(Caja, "Caja/AbrirCaja");
            var caja = await Servicios.ApiProcessor.GetApi<CajaDto>("Caja/CajaAbierta");
            detalle = new DetalleCajaDto()
            {
                TipoMovimiento = Constantes.TipoMovimiento.Ingreso,
                Monto = Caja.MontoApertura,
                CajaId = caja.Id,
                TipoPago = Constantes.TipoPago.Efectivo
            };
            AbrirCajaPopUp = false;
            Caja = new CajaDto();
            CerrarCajaVisibilidad = Visibility.Visible;
            AbrirCajaVisibilidad = Visibility.Collapsed;
            await Servicios.ApiProcessor.PostApi<DetalleCajaDto>(detalle, $"DetalleCaja/Insert");
        }

        private void Cancelar()
        {
            AbrirCajaPopUp = false;
            Caja = new CajaDto();
        }

        private void AbrirCajaPop()
        {
            AbrirCajaPopUp = true;
            Caja = new CajaDto();
        }

        private void AbrirEfecSalida()
        {
            EfectivoSalida = true;
            eventAggregator.GetEvent<PubSubEvent<string>>().Publish("Caja");
        }

        private void AbrirEfecEntrada()
        {
            EfectivoEntrada = true;
            eventAggregator.GetEvent<PubSubEvent<string>>().Publish("Caja");
        }

        private void AbrirCheSalida()
        {
            ChequeSalida = true;
            eventAggregator.GetEvent<PubSubEvent<string>>().Publish("Caja");
        }

        private void AbrirCheEntrada()
        {
            ChequeEntrada = true;
            eventAggregator.GetEvent<PubSubEvent<string>>().Publish("Caja");
        }

        private void CerrarPops(Dictionary<string, bool> obj)
        {
            foreach (var item in obj)
            {
                switch (item.Key)
                {
                    case "ChequeSalida":
                        ChequeSalida = false;
                        break;
                    case "ChequeEntrada":
                        ChequeEntrada = false;
                        break;
                    case "ComprobanteEntrada":
                        EfectivoEntrada = item.Value;
                        break;
                    case "ComprobanteSalida":
                        EfectivoSalida = item.Value;
                        break;
                }
            }
        }

        public async Task Inicializar()
        {
            SubrubrosEntrada = new ObservableCollection<SubRubroDto>(await Servicios.ApiProcessor.GetApi<SubRubroDto[]>("SubRubro/ObtenerEntradas"));
            if (await Servicios.ApiProcessor.GetApi<bool>("Caja/CajasEstado"))
            {

                CerrarCajaVisibilidad = Visibility.Visible;
                AbrirCajaVisibilidad = Visibility.Collapsed;
            }
            else
            {
                CerrarCajaVisibilidad = Visibility.Collapsed;
                AbrirCajaVisibilidad = Visibility.Visible;
            }
        }
        public bool ChequeSalida { get { return _chequeSalida; } set { SetProperty(ref _chequeSalida, value); } }

        private IRegionManager regionManager;
        private IEventAggregator eventAggregator;

        public ICommand CrearCommand { get; set; }
        public ICommand CrearCerrarCaja { get; set; }
        public ICommand AbrirExtraccion { get; set; }
        public DelegateCommand<string> CancelarPop { get; }
        public ICommand AbrirDeposito { get; set; }
        public DelegateCommand ExtraerDinero { get; }
        public ICommand AbrirCerrarCaja { get; set; }
        public DelegateCommand Depositar { get; }
        public ICommand CancelarCommand { get; set; }
        public ICommand CerrarCerrarCaja { get; set; }

        private bool _chequeSalida;
        private bool _chequeEntrada;
        private bool _efectivoEntrada;
        private bool _efectivoSalida;
        private bool _ctaCteEntrada;
        private bool _ctaCteSalida;
        private Visibility _visibility;
        private CajaDto _caja;
        private bool _abrirCajaPopUp;
        private ObservableCollection<DetalleCajaDto> _detallesCaja;
        private bool _cerrarCajaPopUp;
        private decimal _montoSistema;
        private decimal _diferencia;
        private decimal _montoCierre;
        private bool _depositarPopUp;
        private bool _extraccionPopUp;
        private decimal _extraccion;
        private decimal _deposito;
        private decimal _egreso;
        private decimal _ingreso;

        public ComprobanteEntradaDto ComprobanteEntrada { get { return _comprobanteEntrada; } set { SetProperty(ref _comprobanteEntrada, value); } }

        public ICommand AbrirEfectivoEntrada { get; set; }
        public ICommand AbrirCajaCommand { get; set; }
        public ICommand AbrirEfectivoSalida { get; set; }
        public ICommand AbrirChequeSalida { get; set; }
        public ICommand AbrirChequeEntrada { get; set; }

        public ObservableCollection<SubRubroDto> SubrubrosEntrada { get { return _subRubroEntrada; } set { SetProperty(ref _subRubroEntrada, value); } }
        public Visibility CerrarCajaVisibilidad { get { return _visibilidad; } set { SetProperty(ref _visibilidad, value); } }

        public Visibility AbrirCajaVisibilidad { get { return _visibility; } set { SetProperty(ref _visibility, value); } }

        public bool ChequeEntrada { get { return _chequeEntrada; } set { SetProperty(ref _chequeEntrada, value); } }
        public bool EfectivoEntrada { get { return _efectivoEntrada; } set { SetProperty(ref _efectivoEntrada, value); } }
        public bool EfectivoSalida { get { return _efectivoSalida; } set { SetProperty(ref _efectivoSalida, value); } }

        public bool DepositarPopUp { get { return _depositarPopUp; } set { SetProperty(ref _depositarPopUp, value); } }
        public bool ExtraccionPopUp { get { return _extraccionPopUp; } set { SetProperty(ref _extraccionPopUp, value); } }

        public decimal Egreso {  get { return _egreso; } set { SetProperty(ref _egreso, value); } }
        public decimal Ingreso { get { return _ingreso; } set { SetProperty(ref _ingreso, value); } }
    }
}
