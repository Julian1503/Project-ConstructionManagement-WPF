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
    public class TransferenciaEntradaViewModel : BindableBase
    {
        public TransferenciaEntradaViewModel(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            CerrarCommand = new DelegateCommand(Cancelar);
            CrearCommand = new DelegateCommand(Crear);
            Operacion = new OperacionDto();
            this.eventAggregator.GetEvent<PubSubEvent<string>>().Subscribe(Uso);
        }

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

        public EmpresaDto Cliente { get { return _cliente; } set { SetProperty(ref _cliente, value); } }
        public UsadoEn Usado { get; set; }
        public OperacionDto Operacion { get { return _operacion; } set { SetProperty(ref _operacion, value); } }
        public BancoDto Banco { get { return _banco; } set { SetProperty(ref _banco, value); } }
        public ObservableCollection<BancoDto> Bancos { get { return _bancos; } set { SetProperty(ref _bancos, value); } }
        public ObservableCollection<EmpresaDto> Clientes { get { return _clientes; } set { SetProperty(ref _clientes, value); } }
        private IEventAggregator eventAggregator;
        private ObservableCollection<BancoDto> _bancos;
        private ObservableCollection<EmpresaDto> _clientes;
        private OperacionDto _operacion;
        private BancoDto _banco;
        private EmpresaDto _cliente;
        private decimal _subtotal;
        private void Calcular()
        {
            Total = Subtotal + Impuesto;
        }

        public decimal Subtotal
        {
            get { return _subtotal; }
            set { SetProperty(ref _subtotal, value); Calcular(); }
        }
        private decimal _total;
        public decimal Total
        {
            get { return _total; }
            set { SetProperty(ref _total, value); }
        }
        private decimal _impuesto;
        public decimal Impuesto
        {
            get { return _impuesto; }
            set { SetProperty(ref _impuesto, value); Calcular(); }
        }
        private async void Crear()
        {
            Operacion.Debe = Total;
            if (Operacion.Debe > 0 && Banco != null)
            {
                var cuentaCorriente = await ApiProcessor.GetApi<CuentaCorrienteDto>($"CuentaCorriente/Banco/{Banco.Id}");
                Operacion.CuentaCorrienteId = cuentaCorriente.Id;
                Operacion.FechaVencimiento = Operacion.FechaEmision;
                Operacion.TipoOperacion = TipoOperacion.Tranferencia;
                Operacion.Haber = 0;
                Operacion.Referencia = 0;
                Operacion.ReferenciaPlus = "";
                var transferencia = new TransferenciaDto();
                transferencia.BancoId = Banco.Id;
                transferencia.Usado = Usado;
                transferencia.ImpuestoBancario = 0;
                transferencia.Concepto = Operacion.Concepto;
                transferencia.Entrada= true;
                transferencia.Fecha = (DateTime) Operacion.FechaEmision;
                transferencia.PagueseA = Operacion.DePara;
                transferencia.Numero = long.Parse(Operacion.CodigoCausal);
                transferencia.Monto = (decimal)Operacion.Debe;
                await ApiProcessor.PostApi(transferencia, "Transferencia/Insert");
                await ApiProcessor.PostApi(Operacion, "Operacion/Insert");
                var diccionario = new Dictionary<string, bool>();
                diccionario.Add("TransferenciaEntrada", false);
                Operacion = new OperacionDto();
                MessageBox.Show("La operacion a sido registrado con exito!");
            }
        }

        private void Cancelar()
        {
            Operacion = new OperacionDto();
            var diccionario = new Dictionary<string, bool>();
            diccionario.Add("TransferenciaEntrada", false);
            eventAggregator.GetEvent<PubSubEvent<Dictionary<string, bool>>>().Publish(diccionario);
        }

        public ICommand CerrarCommand { get; set; }
        public ICommand CrearCommand { get; set; }

        public async Task Inicializar()
        {
            Bancos = new ObservableCollection<BancoDto>(await Servicios.ApiProcessor.GetApi<BancoDto[]>("Banco/GetAll"));
            Clientes = new ObservableCollection<EmpresaDto>(await Servicios.ApiProcessor.GetApi<EmpresaDto[]>("Empresa/GetAll"));
        }
    }
}
