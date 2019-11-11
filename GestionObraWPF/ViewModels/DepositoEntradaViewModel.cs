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
    public class DepositoEntradaViewModel : BindableBase
    {
        public DepositoEntradaViewModel(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            CerrarCommand = new DelegateCommand(Cancelar);
            CrearCommand = new DelegateCommand(Crear);
            Operacion = new OperacionDto();
            this.eventAggregator.GetEvent<PubSubEvent<string>>().Subscribe(Uso);
        }

        public EmpresaDto Cliente { get { return _cliente; } set { SetProperty(ref _cliente, value); } }
        public OperacionDto Operacion { get { return _operacion; } set { SetProperty(ref _operacion, value); } }
        public BancoDto Banco { get { return _banco; } set { SetProperty(ref _banco, value); } }
        public ObservableCollection<BancoDto> Bancos { get { return _bancos; } set { SetProperty(ref _bancos, value); } }
        public ObservableCollection<EmpresaDto> Clientes { get { return _clientes; } set { SetProperty(ref _clientes, value); } }
        private IEventAggregator eventAggregator;
        public UsadoEn Usado { get; set; }
        private ObservableCollection<BancoDto> _bancos;
        private ObservableCollection<EmpresaDto> _clientes;
        private OperacionDto _operacion;
        private BancoDto _banco;
        private EmpresaDto _cliente;
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
            if(Operacion.Debe>0 && Banco != null)
            {
                var cuentaCorriente = await ApiProcessor.GetApi<CuentaCorrienteDto>($"CuentaCorriente/Banco/{Banco.Id}");
                Operacion.CuentaCorrienteId =cuentaCorriente.Id;
                Operacion.FechaVencimiento = Operacion.FechaEmision;
                Operacion.TipoOperacion = TipoOperacion.Deposito;
                Operacion.Haber = 0;
                Operacion.Referencia = 0;
                Operacion.ReferenciaPlus = "";
                var deposito = new DepositoDto();
                deposito.BancoId = Banco.Id;
                deposito.Usado = Usado;
                deposito.Entrada = true;
                deposito.Fecha = (DateTime)Operacion.FechaEmision;
                deposito.Concepto = Operacion.Concepto;
                deposito.DePara = Operacion.DePara;
                deposito.Numero = long.Parse(Operacion.CodigoCausal);
                deposito.Monto = (decimal)Operacion.Debe;
                await ApiProcessor.PostApi(deposito, "Deposito/Insert");
                await ApiProcessor.PostApi(Operacion, "Operacion/Insert");
                var diccionario = new Dictionary<string, bool>();
                diccionario.Add("DepositoEntrada", false);
                Operacion = new OperacionDto();
                MessageBox.Show("La operacion a sido registrado con exito!");
            }
        }

        private void Cancelar()
        {
            Operacion = new OperacionDto();
            var diccionario = new Dictionary<string, bool>();
            diccionario.Add("DepositoEntrada", false);
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
