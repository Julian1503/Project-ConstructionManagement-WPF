using GestionObraWPF.Constantes;
using GestionObraWPF.DTOs;
using GestionObraWPF.Servicios;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace GestionObraWPF.ViewModels
{
    public class ListadoComprobantesCompraViewModel : BindableBase
    {
        private ObservableCollection<ComprobanteCompraDto> _comprobantes;
        private Visibility _sinComprobantes = Visibility.Collapsed;
        private ComprobanteCompraDto _comprobanteCompra;
        private bool _comprar;

        public ListadoComprobantesCompraViewModel(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            PagoCommand = new DelegateCommand<ComprobanteCompraDto>(Pago);
            Cerrar = new DelegateCommand(CerrarPago);
            CrearCommand = new DelegateCommand(AbrirFP);
            Efectivo = new DelegateCommand(EfectivoP);
            Cheque = new DelegateCommand(ChequeS);
            Cancelar = new DelegateCommand(CancelarResumen);
            Trans = new DelegateCommand(Transs);
            eventAggregator.GetEvent<PubSubEvent<Dictionary<string, bool>>>().Subscribe(CerrarPops);
        }

        private void Operando(bool obj)
        {
            Opero = obj;
        }

        private void CancelarResumen()
        {
            Resumen = false;
        }

        private void AbrirFP()
        {
            if (ComprobanteCompraDto.Pagando + Pagar <= ComprobanteCompraDto.Total)
            {
                Resumen = false;
                Comprar = true;
            }
            else
            {
                MessageBox.Show("Monto ingresado incorrecto");
            }
        }
        private bool _opero;
        public bool Opero
        {
            get { return _opero; }
            set { SetProperty(ref _opero, value); }
        }
        private decimal _pagar;
        public decimal Pagar
        {
            get { return _pagar; }
            set { SetProperty(ref _pagar, value); }
        }
        private async void CerrarPops(Dictionary<string, bool> obj)
        {
            foreach (var item in obj)
            {
                switch (item.Key)
                {
                    case "ChequeSalida":
                        if (item.Value)
                        {
                            if (ComprobanteCompraDto.Pagando + Pagar <= ComprobanteCompraDto.Total && Opero)
                            {
                                ComprobanteCompraDto.FormaPago = TipoPago.Cheque;
                                if (ComprobanteCompraDto.Pagando == ComprobanteCompraDto.Total)
                                {
                                    ComprobanteCompraDto.Pagado = true;
                                }
                                ComprobanteCompraDto.Pagando += Pagar;
                                await ApiProcessor.PutApi(ComprobanteCompraDto, $"ComprobanteCompra/{ComprobanteCompraDto.Id}");
                                await Inicializar();
                                eventAggregator.GetEvent<PubSubEvent<bool>>().Unsubscribe(Operando);
                                Opero = false;
                            }
                        }
                            ChequeSalida = false;
                        break;
                    case "TransferenciaSalida":
                        if (item.Value)
                        {
                            if (ComprobanteCompraDto.Pagando + Pagar <= ComprobanteCompraDto.Total && Opero)
                            {
                                ComprobanteCompraDto.FormaPago = TipoPago.Transferencia;
                                ComprobanteCompraDto.Pagando += Pagar;
                                if (ComprobanteCompraDto.Pagando == ComprobanteCompraDto.Total)
                                {
                                    ComprobanteCompraDto.Pagado = true;
                                }
                                await ApiProcessor.PutApi(ComprobanteCompraDto, $"ComprobanteCompra/{ComprobanteCompraDto.Id}");
                                await Inicializar();
                                eventAggregator.GetEvent<PubSubEvent<bool>>().Unsubscribe(Operando);
                                Opero = false;
                            }
                            TransSal = false;
                        }
                        break;
                    case "ComprobanteSalida":
                        if (!item.Value)
                        {
                            if (ComprobanteCompraDto.Pagando + Pagar <= ComprobanteCompraDto.Total && Opero)
                            {
                                ComprobanteCompraDto.FormaPago = TipoPago.Cheque;
                                ComprobanteCompraDto.Pagando += Pagar;
                                if (ComprobanteCompraDto.Pagando == ComprobanteCompraDto.Total)
                                {
                                    ComprobanteCompraDto.Pagado = true;
                                }
                                await ApiProcessor.PutApi(ComprobanteCompraDto, $"ComprobanteCompra/{ComprobanteCompraDto.Id}");
                                await Inicializar();
                                eventAggregator.GetEvent<PubSubEvent<bool>>().Unsubscribe(Operando);
                                Opero = false;
                            }
                            EfectivoSalida = false;
                        }
                        break;
                }
            }
        }
        private void ChequeS()
        {
            Comprar = false;
            ChequeSalida = true;
            eventAggregator.GetEvent<PubSubEvent<bool>>().Subscribe(Operando);
            eventAggregator.GetEvent<PubSubEvent<ComprobanteCompraDto>>().Publish(ComprobanteCompraDto);
            eventAggregator.GetEvent<PubSubEvent<decimal>>().Publish(Pagar);
            eventAggregator.GetEvent<PubSubEvent<string>>().Publish("Proveedor");
        }

        private void Transs()
        {

            Comprar = false;
            TransSal = true;
            eventAggregator.GetEvent<PubSubEvent<bool>>().Subscribe(Operando);
            eventAggregator.GetEvent<PubSubEvent<ComprobanteCompraDto>>().Publish(ComprobanteCompraDto);
            eventAggregator.GetEvent<PubSubEvent<decimal>>().Publish(Pagar);
            eventAggregator.GetEvent<PubSubEvent<string>>().Publish("Proveedor");
        }

        private async void EfectivoP()
        {
            Comprar = false;
            EfectivoSalida = true;
            eventAggregator.GetEvent<PubSubEvent<bool>>().Subscribe(Operando);
            eventAggregator.GetEvent<PubSubEvent<ComprobanteCompraDto>>().Publish(ComprobanteCompraDto);
            eventAggregator.GetEvent<PubSubEvent<decimal>>().Publish(Pagar);
            eventAggregator.GetEvent<PubSubEvent<string>>().Publish("Proveedor");
            if (ComprobanteCompraDto.Proveedor != null)
            {
                eventAggregator.GetEvent<PubSubEvent<string>>().Publish($"{ComprobanteCompraDto.Proveedor.RazonSocial}");
            }
            else
            {
                eventAggregator.GetEvent<PubSubEvent<string>>().Publish($"{ComprobanteCompraDto.CUIT}");
            }
        }

        private void CerrarPago()
        {
            Comprar = false;
        }

        private void Pago(ComprobanteCompraDto obj)
        {
            if (obj != null)
            {
                if (!obj.Pagado)
                {
                    ComprobanteCompraDto = obj;
                    Resumen = true;
                }
                else
                {
                    MessageBox.Show("Comprobante ya pagado");
                }
            }
        }

        public ObservableCollection<ComprobanteCompraDto> Comprobantes { get { return _comprobantes; } set { SetProperty(ref _comprobantes, value); } }

        public Visibility SinComprobantes { get { return _sinComprobantes; } set { SetProperty(ref _sinComprobantes, value); } }

        private IEventAggregator eventAggregator;
        private bool _efectivoSalida;
        private bool _chequeSalida;
        private bool _transSal;

        public DelegateCommand<ComprobanteCompraDto> PagoCommand { get; set; }
        public ICommand Cerrar { get; set; }
        public DelegateCommand CrearCommand { get; }
        public ICommand Efectivo { get; set; }
        public ICommand Cheque { get; set; }
        public DelegateCommand Cancelar { get; }

        public ComprobanteCompraDto ComprobanteCompraDto { get { return _comprobanteCompra; } set { SetProperty(ref _comprobanteCompra, value); } }

        public bool Comprar { get { return _comprar; } set { SetProperty(ref _comprar, value); } }

        public bool EfectivoSalida { get { return _efectivoSalida; } set { SetProperty(ref _efectivoSalida, value); } }
        private bool _resumen;
        public bool Resumen
        {
            get { return _resumen; }
            set { SetProperty(ref _resumen, value); }
        }
        public ICommand Trans { get; set; }
        public bool ChequeSalida { get { return _chequeSalida; } set { SetProperty(ref _chequeSalida, value); } }
        public bool TransSal { get { return _transSal; } set { SetProperty(ref _transSal, value); } }

        public async Task Inicializar()
        {
            Comprobantes = new ObservableCollection<ComprobanteCompraDto>(await ApiProcessor.GetApi<ComprobanteCompraDto[]>("ComprobanteCompra/GetAll"));
            if (Comprobantes.Count() == 0)
            {
                SinComprobantes = Visibility.Visible;
            }
        }
    }
}
