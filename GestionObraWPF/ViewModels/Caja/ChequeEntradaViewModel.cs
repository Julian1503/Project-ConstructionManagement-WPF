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
    public class ChequeEntradaViewModel : BindableBase
    {
        public ChequeEntradaViewModel(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            CargarChequeCommand = new DelegateCommand(CargarChequeSalida);
            CancelarCommand = new DelegateCommand(Cancelar);
            BorrarCommand = new DelegateCommand(Borrar);
            ChequeEntrada = new ChequeEntradaDto();
            this.eventAggregator.GetEvent<PubSubEvent<string>>().Subscribe(Usado);
        }

        private void Usado(string obj)
        {
            switch (obj)
            {
                case "Banco":
                    ChequeEntrada.Usado = UsadoEn.Banco;
                    break;
                case "Caja":
                    ChequeEntrada.Usado = UsadoEn.Caja;
                    break;
                case "Proveedor":
                    ChequeEntrada.Usado = UsadoEn.Proveedor;
                    break;
                case "Venta":
                    ChequeEntrada.Usado = UsadoEn.Venta;
                    break;
            }
        }
        private ChequeEntradaDto _cheque;
        private ObservableCollection<BancoDto> _banco;

        public ICommand CargarChequeCommand { get; set; }
        public ICommand CancelarCommand { get; set; }
        public ICommand BorrarCommand { get; set; }

        private IEventAggregator eventAggregator;
        private int _dia = DateTime.Now.Day;
        private int _mes = DateTime.Now.Month;
        private int _ano = DateTime.Now.Year;

        public ChequeEntradaDto ChequeEntrada { get { return _cheque; } set { SetProperty(ref _cheque, value); } }

        public ObservableCollection<BancoDto> Bancos { get { return _banco; } set { SetProperty(ref _banco, value); } }

        public int Dia { get { return _dia; } set { SetProperty(ref _dia, value); } }
        public int Mes { get { return _mes; } set { SetProperty(ref _mes, value); } }
        public int Ano { get { return _ano; } set { SetProperty(ref _ano, value); } }


        private void Borrar()
        {
            ChequeEntrada = new ChequeEntradaDto();
            Dia = Mes = Ano = 0;
        }

        private void Cancelar()
        {
            var diccionario = new Dictionary<string, bool>();
            diccionario.Add("ChequeEntrada", false);
            ChequeEntrada = new ChequeEntradaDto();
            eventAggregator.GetEvent<PubSubEvent<Dictionary<string, bool>>>().Publish(diccionario);
        }

        public async Task Inicializar()
        {
            Bancos = new ObservableCollection<BancoDto>(await ApiProcessor.GetApi<BancoDto[]>("Banco/GetAll"));
        }

        public async void CargarChequeSalida()
        {
            if (ChequeEntrada.Banco != null && ChequeEntrada.Monto > 0 && !string.IsNullOrWhiteSpace(ChequeEntrada.Serie) && (Dia > 0 && Dia < 32) && (Mes > 0 && Mes < 13) && Ano > 1900 && !string.IsNullOrWhiteSpace(ChequeEntrada.EmitidoPor) && ChequeEntrada.Numero > 0)
            {
                ChequeEntrada.FechaHasta = new DateTime(Ano, Mes, Dia);
                ChequeEntrada.BancoId = ChequeEntrada.Banco.Id;
                await Servicios.ApiProcessor.PostApi(ChequeEntrada, "ChequeEntrada/Insert");
                if (ChequeEntrada.Usado != UsadoEn.Caja && ChequeEntrada.Usado != UsadoEn.Proveedor)
                {
                    var cuentaCorriente = await ApiProcessor.GetApi<CuentaCorrienteDto>($"CuentaCorriente/Banco/{ChequeEntrada.BancoId}");
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
                        ReferenciaPlus = $"{ChequeEntrada.Numero}"
                    };
                    await ApiProcessor.PostApi(op, "Operacion/Insert");
                }
                else
                {
                    if (await Servicios.ApiProcessor.GetApi<bool>("Caja/CajasEstado"))
                    {
                        var Caja = await Servicios.ApiProcessor.GetApi<CajaDto>("Caja/CajaAbierta");
                        var detalleCaja = new DetalleCajaDto
                        {
                            CajaId = Caja.Id,
                            Monto = ChequeEntrada.Monto,
                            TipoMovimiento = Constantes.TipoMovimiento.Ingreso,
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
                eventAggregator.GetEvent<PubSubEvent<Dictionary<string, bool>>>().Publish(diccionario);
                ChequeEntrada = new ChequeEntradaDto();
                MessageBox.Show("El cheque a sido registrado con exito!");
            }
            else
            {
                MessageBox.Show("Faltan datos para realizar la carga!");
            }
        }
    }
}
