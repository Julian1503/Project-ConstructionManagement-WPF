using GestionObraWPF.Constantes;
using GestionObraWPF.DTOs;
using GestionObraWPF.Servicios;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace GestionObraWPF.ViewModels.Caja
{
    public class ChequeViewModel : BindableBase
    {
        private ChequeSalidaDto _cheque;
        private ObservableCollection<BancoDto> _banco;

        public ICommand CargarChequeCommand { get; set; }
        public ICommand CancelarCommand { get; set; }
        public ICommand BorrarCommand { get; set; }

        private IEventAggregator eventAggregator;
        private int _dia = DateTime.Now.Day;
        private int _mes = DateTime.Now.Month;
        private int _ano = DateTime.Now.Year;

        public ChequeSalidaDto ChequeSalida { get { return _cheque; } set { SetProperty(ref _cheque, value); } }
        private bool _activarMonto = true;
        public bool ActivarMonto
        {
            get { return _activarMonto; }
            set { SetProperty(ref _activarMonto, value); }
        }

        public ObservableCollection<BancoDto> Bancos { get { return _banco; } set { SetProperty(ref _banco, value); } }

        public int Dia { get { return _dia; } set { SetProperty(ref _dia, value); } }
        public int Mes { get { return _mes; } set { SetProperty(ref _mes, value); } }
        public int Ano { get { return _ano; } set { SetProperty(ref _ano, value); } }

        public ChequeViewModel(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            CargarChequeCommand = new DelegateCommand(CargarChequeSalida);
            BorrarCommand = new DelegateCommand(Borrar);
            CancelarCommand = new DelegateCommand(Cancelar);
            ChequeSalida = new ChequeSalidaDto();
            this.eventAggregator.GetEvent<PubSubEvent<decimal>>().Subscribe(PasandoPrecio);
            this.eventAggregator.GetEvent<PubSubEvent<string>>().Subscribe(Usado);
            this.eventAggregator.GetEvent<PubSubEvent<ComprobanteCompraDto>>().Subscribe(Compra);
        }

        private void PasandoPrecio(decimal obj)
        {
        ActivarMonto = false;
            ChequeSalida.Monto = obj;
        }

        private void Compra(ComprobanteCompraDto obj)
        {
            if (obj.Proveedor != null)
            {
                ChequeSalida.PagueseA = obj.Proveedor.RazonSocial;
            }
            else
            {
                ChequeSalida.PagueseA = obj.cuit;
            }
            ChequeSalida.Monto = obj.Total;
        }

        private void Usado(string obj)
        {
            switch (obj)
            {
                case "Banco":
                    ChequeSalida.Usado = UsadoEn.Banco;
                    break;
                case "Caja":
                    ChequeSalida.Usado = UsadoEn.Caja;
                    break;
                case "Proveedor":
                    ChequeSalida.Usado = UsadoEn.Proveedor;
                    break;
                case "Venta":
                    ChequeSalida.Usado = UsadoEn.Venta;
                    break;
            }
        }
        private void Borrar()
        {
            ChequeSalida = new ChequeSalidaDto();
        }

        private void Cancelar()
        {
            var diccionario = new Dictionary<string, bool>();
            diccionario.Add("ChequeSalida", false);
            ChequeSalida = new ChequeSalidaDto();
            Dia = Mes = Ano = 0;
            eventAggregator.GetEvent<PubSubEvent<bool>>().Publish(false);
            eventAggregator.GetEvent<PubSubEvent<Dictionary<string, bool>>>().Publish(diccionario);
        ActivarMonto = true;
        }

        public async Task Inicializar()
        {
            Bancos = new ObservableCollection<BancoDto>(await Servicios.ApiProcessor.GetApi<BancoDto[]>("Banco/GetAll"));
        }

        public async void CargarChequeSalida()
        {
            if (ChequeSalida.Banco != null && ChequeSalida.Monto > 0 && !string.IsNullOrWhiteSpace(ChequeSalida.Serie) && (Dia > 0 && Dia < 32) && (Mes > 0 && Mes < 13) && Ano > 1900 && !string.IsNullOrWhiteSpace(ChequeSalida.PagueseA) && ChequeSalida.Numero > 0)
            {
                ChequeSalida.FechaHasta = new DateTime(Ano, Mes, Dia);
                ChequeSalida.BancoId = ChequeSalida.Banco.Id;
                await Servicios.ApiProcessor.PostApi<ChequeSalidaDto>(ChequeSalida, "ChequeSalida/Insert");
                if (ChequeSalida.Usado != UsadoEn.Caja && ChequeSalida.Usado != UsadoEn.Proveedor)
                {
                    var cuentaCorriente = await ApiProcessor.GetApi<CuentaCorrienteDto>($"CuentaCorriente/Banco/{ChequeSalida.BancoId}");
                    var op = new OperacionDto
                    {
                        CuentaCorrienteId = cuentaCorriente.Id,
                        Debe = 0,
                        EstaEnResumen = true,
                        TipoOperacion = Constantes.TipoOperacion.Cheque,
                        FechaEmision = ChequeSalida.FechaDesde,
                        Concepto = ChequeSalida.Concepto,
                        DePara = ChequeSalida.PagueseA,
                        Descontado = 0,
                        FechaVencimiento = ChequeSalida.FechaHasta,
                        Haber = ChequeSalida.Monto,
                        Referencia = ChequeSalida.Numero,
                        CodigoCausal = ChequeSalida.Serie,
                        ReferenciaPlus = $"{ ChequeSalida.Numero}"
                    };
                    await ApiProcessor.PostApi<OperacionDto>(op, "Operacion/Insert");
                }
                else
                {
                    if (await Servicios.ApiProcessor.GetApi<bool>("Caja/CajasEstado"))
                    {
                        var Caja = await Servicios.ApiProcessor.GetApi<CajaDto>("Caja/CajaAbierta");
                        var detalleCaja = new DetalleCajaDto
                        {
                            CajaId = Caja.Id,
                            Monto = ChequeSalida.Monto,
                            TipoMovimiento = Constantes.TipoMovimiento.Egreso,
                            TipoPago = Constantes.TipoPago.Cheque
                        };
                        await Servicios.ApiProcessor.PostApi<DetalleCajaDto>(detalleCaja, "DetalleCaja/Insert");
                    }

                    else
                    {
                        MessageBox.Show("Por favor abra la caja");
                    }
                }
                var diccionario = new Dictionary<string, bool>();
                diccionario.Add("ChequeSalida", true);
                    eventAggregator.GetEvent<PubSubEvent<bool>>().Publish(true);
                eventAggregator.GetEvent<PubSubEvent<Dictionary<string, bool>>>().Publish(diccionario);
                ChequeSalida = new ChequeSalidaDto();
                ActivarMonto = true;
                MessageBox.Show("El cheque a sido registrado con exito!");
            }
            else
            {
                MessageBox.Show("Faltan datos para realizar la carga!");
            }
        }
    }
}
