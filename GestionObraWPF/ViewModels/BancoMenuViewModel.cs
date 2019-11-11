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

namespace GestionObraWPF.ViewModels
{
    public class BancoMenuViewModel : BindableBase
    {
        private bool _chequeEntrada;
        private bool _transferenciaEntrada;
        private bool _transferenciaSalida;
        private bool _depositoEntrada;
        private bool _depositoSalida;
        private IEventAggregator eventAggregator;
        private OperacionDto _operacion;
        private BancoDto _banco;
        private ObservableCollection<BancoDto> _bancos;
        private decimal _extraccion;

        public bool ChequeEntrada { get { return _chequeEntrada; } set { SetProperty(ref _chequeEntrada, value); } }
        public bool DepositoSalida { get { return _depositoSalida; } set { SetProperty(ref _depositoSalida, value); } }
        public bool DepositoEntrada { get { return _depositoEntrada; } set { SetProperty(ref _depositoEntrada, value); } }
        public bool TransferenciaSalida { get { return _transferenciaSalida; } set { SetProperty(ref _transferenciaSalida, value); } }
        public bool TransferenciaEntrada { get { return _transferenciaEntrada; } set { SetProperty(ref _transferenciaEntrada, value); } }

        public ICommand AbrirCheque { get; set; }
        public ICommand AbrirTransfEntrada { get; set; }
        public ICommand CargarCommando { get; set; }
        public ICommand CancelarCommando { get; set; }
        public ICommand AbrirTransfSalida { get; set; }
        public ICommand AbrirDepositoEntrada { get; set; }
        public ICommand AbrirDepositoSalida { get; set; }
        public decimal Extraccion { get { return _extraccion; } set { SetProperty(ref _extraccion, value); } }
        public OperacionDto Operacion { get { return _operacion; } set { SetProperty(ref _operacion, value); } }
        public BancoDto Banco { get { return _banco; } set { SetProperty(ref _banco, value); } }
        public ObservableCollection<BancoDto> Bancos { get { return _bancos; } set { SetProperty(ref _bancos, value); } }

        public BancoMenuViewModel(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            Operacion = new OperacionDto();
            CancelarCommando = new DelegateCommand(CancelarExtraccion);
            CargarCommando = new DelegateCommand(ExtraerDinero);
            AbrirCheque = new DelegateCommand(AbrirChequeEntrada);
            AbrirTransfEntrada = new DelegateCommand(AbrirTransfE);
            AbrirTransfSalida = new DelegateCommand(AbrirTransfS);
            AbrirDepositoEntrada = new DelegateCommand(AbrirDepositoE);
            AbrirDepositoSalida = new DelegateCommand(AbrirDepositoS);
            eventAggregator.GetEvent<PubSubEvent<Dictionary<string, bool>>>().Subscribe(CerrarPops);
        }

        private void CancelarExtraccion()
        {
            DepositoSalida = false;
        }

        private async void ExtraerDinero()
        {
            if (Extraccion > 0)
            {
                if (await Servicios.ApiProcessor.GetApi<bool>("Caja/CajasEstado"))
                {
                    var cuentaCorriente = await ApiProcessor.GetApi<CuentaCorrienteDto>($"CuentaCorriente/Banco/{Banco.Id}");
                    Operacion.CuentaCorrienteId = cuentaCorriente.Id;
                    Operacion.FechaVencimiento = Operacion.FechaEmision;
                    Operacion.TipoOperacion = TipoOperacion.Deposito;
                    Operacion.DePara = "Gonelec";
                    Operacion.Concepto = "Extraccion";
                    Operacion.Debe = 0;
                    Operacion.Haber = Extraccion;
                    Operacion.Referencia = 0;
                    Operacion.ReferenciaPlus = "";
                    var deposito = new DepositoDto();
                    deposito.BancoId = Banco.Id;
                    deposito.Usado = UsadoEn.Banco;
                    deposito.Entrada = false;
                    deposito.DePara = Operacion.DePara;
                    deposito.Concepto = "Extraccion";
                    deposito.Fecha= (DateTime)Operacion.FechaEmision;
                    deposito.Numero = long.Parse(Operacion.CodigoCausal);
                    deposito.Monto = (decimal)Operacion.Haber;
                    await ApiProcessor.PostApi(deposito, "Deposito/Insert");
                    await ApiProcessor.PostApi(Operacion, "Operacion/Insert");
                    Operacion = new OperacionDto();
                    var Caja = await Servicios.ApiProcessor.GetApi<CajaDto>("Caja/CajaAbierta");
                    var detalleCaja = new DetalleCajaDto
                    {
                        CajaId = Caja.Id,
                        Monto = Extraccion,
                        TipoMovimiento = Constantes.TipoMovimiento.Ingreso,
                        TipoPago = Constantes.TipoPago.Efectivo
                    };
                    await Servicios.ApiProcessor.PostApi<DetalleCajaDto>(detalleCaja, "DetalleCaja/Insert");
                    MessageBox.Show("Se completo la extraccion!");
                    Extraccion = 0;
                    Banco = null;                                 
                }
                else
                {
                    MessageBox.Show("Por favor abra la caja");
                }
            }
        }

        public async Task Inicializar()
        {
            Bancos = new ObservableCollection<BancoDto>(await ApiProcessor.GetApi<BancoDto[]>("Banco/GetAll"));
        }

        private void AbrirDepositoS()
        {
          eventAggregator.GetEvent<PubSubEvent<string>>().Publish("Banco");
            DepositoSalida = true;
        }

        private void AbrirDepositoE()
        {
         eventAggregator.GetEvent<PubSubEvent<string>>().Publish("Banco");
            DepositoEntrada = true;
        }

        private void AbrirTransfS()
        {
         eventAggregator.GetEvent<PubSubEvent<string>>().Publish("Banco");
            TransferenciaSalida = true;
        }

        private void AbrirTransfE()
        {
         eventAggregator.GetEvent<PubSubEvent<string>>().Publish("Banco");
            TransferenciaEntrada = true;
        }

        private void AbrirChequeEntrada()
        {
         eventAggregator.GetEvent<PubSubEvent<string>>().Publish("Banco");
            ChequeEntrada = true;
        }

        private void CerrarPops(Dictionary<string, bool> obj)
        {
            foreach (var item in obj)
            {
                switch (item.Key)
                {
                    case "TransferenciaEntrada":
                        TransferenciaEntrada = item.Value;
                        break;
                    case "ChequeEntrada":
                        ChequeEntrada = item.Value; 
                        break;
                    case "TransferenciaSalida":
                        TransferenciaSalida = item.Value;
                        break;
                    case "DepositoEntrada":
                        DepositoEntrada = item.Value;
                        break;
                    case "ComprobanteSalida":
                        DepositoSalida = item.Value;
                        break;
                }
            }
        }

    }
}
