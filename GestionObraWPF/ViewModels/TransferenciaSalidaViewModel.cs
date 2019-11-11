using GestionObraWPF.Constantes;
using GestionObraWPF.DTOs;
using GestionObraWPF.Servicios;
using Prism.Commands;
using Prism.Events;
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
    public class TransferenciaSalidaViewModel : BindableBase
    {
        public TransferenciaSalidaViewModel(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            CerrarCommand = new DelegateCommand(Cancelar);
            CrearCommand = new DelegateCommand(Crear);
            Operacion = new OperacionDto();

            this.eventAggregator.GetEvent<PubSubEvent<ComprobanteCompraDto>>().Subscribe(CargarComprobante);
            this.eventAggregator.GetEvent<PubSubEvent<string>>().Subscribe(Uso);
            this.eventAggregator.GetEvent<PubSubEvent<decimal>>().Subscribe(PasandoPrecio);
        }

        private void PasandoPrecio(decimal obj)
        {
            Subtotal = obj;
            ActivarSubtotal = false;
        }

        private void CargarComprobante(ComprobanteCompraDto obj)
        {
            if (obj.Proveedor != null)
            {
                Operacion.DePara = $"{obj.CUIT} - { obj.Proveedor.RazonSocial}";
            }
            else
            {
                Operacion.DePara = obj.CUIT;
            }
            Subtotal = obj.Total;
        }

        public EmpresaDto Cliente { get { return _cliente; } set { SetProperty(ref _cliente, value); } }
        public OperacionDto Operacion { get { return _operacion; } set { SetProperty(ref _operacion, value); } }
        public BancoDto Banco { get { return _banco; } set { SetProperty(ref _banco, value); } }
        public decimal Impuesto { get { return _impuesto; } set { SetProperty(ref _impuesto, value); CalcularTotal(); } }
        private bool _activarSubtotal = true;
        public bool ActivarSubtotal
        {
            get { return _activarSubtotal; }
            set { SetProperty(ref _activarSubtotal, value); }
        }
        private void CalcularTotal()
        {
            Total = Impuesto + Subtotal;
        }

        public decimal Total { get { return _total; } set { SetProperty(ref _total, value); } }
        public ObservableCollection<BancoDto> Bancos { get { return _bancos; } set { SetProperty(ref _bancos, value); } }
        public ObservableCollection<EmpresaDto> Clientes { get { return _clientes; } set { SetProperty(ref _clientes, value); } }
        private IEventAggregator eventAggregator;
        private ObservableCollection<BancoDto> _bancos;
        private ObservableCollection<EmpresaDto> _clientes;
        private OperacionDto _operacion;
        private BancoDto _banco;
        private EmpresaDto _cliente;
        private decimal _impuesto;
        private decimal _total;
        private decimal _subTotal;

        private void Uso(string obj)
        {
            switch (obj)
            {
                case "Banco":
                    Usado = UsadoEn.Banco;
                    break;
                case "Caja":
                   Usado = UsadoEn.Caja;
                    break;
                case "Proveedor":
                   Usado = UsadoEn.Proveedor;
                    break;
                case "Venta":
                    Usado = UsadoEn.Venta;
                    break;
            }
        }

        private async void Crear()
        {
            Operacion.Haber = Total;
            if (Operacion.Haber > 0 && Banco != null)
            {
                var cuentaCorriente = await ApiProcessor.GetApi<CuentaCorrienteDto>($"CuentaCorriente/Banco/{Banco.Id}");
                Operacion.Haber = Total;
                Operacion.CuentaCorrienteId = cuentaCorriente.Id;
                Operacion.FechaVencimiento = Operacion.FechaEmision;
                Operacion.TipoOperacion = TipoOperacion.Tranferencia;
                Operacion.Debe = 0;
                Operacion.Referencia = 0;
                Operacion.ReferenciaPlus = "";
                var transferencia = new TransferenciaDto();
                transferencia.BancoId = Banco.Id;
                transferencia.Usado = Usado;
                transferencia.Entrada =false;
                transferencia.ImpuestoBancario = Impuesto;
                transferencia.Concepto = Operacion.Concepto;
                transferencia.Fecha = (DateTime)Operacion.FechaEmision;
                transferencia.PagueseA = Operacion.DePara;
                transferencia.Numero = long.Parse(Operacion.CodigoCausal);
                transferencia.Monto = Total;
                await ApiProcessor.PostApi(transferencia, "Transferencia/Insert");
                await ApiProcessor.PostApi(Operacion, "Operacion/Insert");
                var diccionario = new Dictionary<string, bool>();
                eventAggregator.GetEvent<PubSubEvent<bool>>().Publish(true);
                diccionario.Add("TransferenciaSalida", true);
                eventAggregator.GetEvent<PubSubEvent<Dictionary<string, bool>>>().Publish(diccionario);
                Operacion = new OperacionDto();
                ActivarSubtotal = true;
                MessageBox.Show("La operacion a sido registrado con exito!");
            }
            else
            {
                MessageBox.Show("Faltan ingresar datos");
            }
        }

        private void Cancelar()
        {
            Operacion = new OperacionDto();
            var diccionario = new Dictionary<string, bool>();
            diccionario.Add("TransferenciaSalida", false);
            eventAggregator.GetEvent<PubSubEvent<Dictionary<string, bool>>>().Publish(diccionario);
            ActivarSubtotal = true;
        }

        public ICommand CerrarCommand { get; set; }
        public ICommand CrearCommand { get; set; }
        public UsadoEn Usado { get; set; }
        public decimal Subtotal { get { return _subTotal; } set { SetProperty(ref _subTotal, value); CalcularTotal(); } }

        public async Task Inicializar()
        {
            Bancos = new ObservableCollection<BancoDto>(await Servicios.ApiProcessor.GetApi<BancoDto[]>("Banco/GetAll"));
            Clientes = new ObservableCollection<EmpresaDto>(await Servicios.ApiProcessor.GetApi<EmpresaDto[]>("Empresa/GetAll"));
        }
    }
}
