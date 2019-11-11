using GestionObraWPF.Constantes;
using GestionObraWPF.DTOs;
using GestionObraWPF.Model;
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
    public class ComprobanteEntradaViewModel : BindableBase
    {
        private ComprobanteEntradaDto _comprobanteEntrada;
        private ObservableCollection<SubRubroDto> _subRubrosEntrada;

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
            Total = ((Subtotal - Descuento) + Recargos) + IVA ;
        }

        public decimal Subtotal
        {
            get { return _subtotal; }
            set
            {
                SetProperty(ref _subtotal, value);
                CalcularTotal();
            }
        }
        public decimal SubtotalDesc
        {
            get { return _subtotalDesc; }
            set
            {
                SetProperty(ref _subtotalDesc, value);
                CalcularTotal();
            }
        }
        public ComprobanteEntradaDto ComprobanteEntrada { get { return _comprobanteEntrada; } set { SetProperty(ref _comprobanteEntrada, value); } }
        public ObservableCollection<RubroDto> Rubros { get { return _rubrosEntrada; } set { SetProperty(ref _rubrosEntrada, value); } }
        public RubroDto Rubro { get { return _rubro; } set { SetProperty(ref _rubro, value);
                Manejar();

            } }

        private async void Manejar()
        {
            if(Rubro!=null)
                SubrubrosEntrada = new ObservableCollection<SubRubroDto>(await Servicios.ApiProcessor.GetApi<SubRubroDto[]>($"Subrubro/GetByRubro/{Rubro.Id}"));

        }

        public ObservableCollection<SubRubroDto> SubrubrosEntrada { get { return _subRubrosEntrada; } set { SetProperty(ref _subRubrosEntrada, value); } }
        public ICommand CargarComando { get; set; }
        public ICommand CancelarCommando { get; set; }
        private IEventAggregator eventAggregator;
        private ObservableCollection<RubroDto> _rubrosEntrada;
        private RubroDto _rubro;
        private decimal _total;
        private decimal _descuento;
        private decimal _subtotal;
        private decimal _IVA;
        private decimal _recargos;
        private decimal _subtotalDesc;
        private decimal _retencion;
        private bool _activarSubtotal =true;
        public bool ActivarSubtotal
        {
            get { return _activarSubtotal; }
            set { SetProperty(ref _activarSubtotal, value); }
        }

        public ComprobanteEntradaViewModel(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            CargarComando = new DelegateCommand(CargaComprobante);
            CancelarCommando = new DelegateCommand(Cancelar);
            ComprobanteEntrada = new ComprobanteEntradaDto();
            this.eventAggregator.GetEvent<PubSubEvent<string>>().Subscribe(Usado);
            this.eventAggregator.GetEvent<PubSubEvent<decimal>>().Subscribe(Venta);
        }

        private void Venta(decimal obj)
        {
            Subtotal = obj;
            ActivarSubtotal = false;
        }

        private void Usado(string obj)
        {
            switch (obj)
            {
                case "Banco":
                    ComprobanteEntrada.Usado = UsadoEn.Banco;
                    break;
                case "Caja":
                    ComprobanteEntrada.Usado = UsadoEn.Caja;
                    break;
                case "Proveedor":
                    ComprobanteEntrada.Usado = UsadoEn.Proveedor;
                    break;
                case "Venta":
                    ComprobanteEntrada.Usado = UsadoEn.Venta;
                    break;
                default:
                    ComprobanteEntrada.Detalle = obj;
                    break;
            }
        }

        public async Task Inicializar()
        {
            Rubros = new ObservableCollection<RubroDto>(await Servicios.ApiProcessor.GetApi<RubroDto[]>("Rubro/GetByEntrada"));
        }
        private void Cancelar()
        {
            var diccionario = new Dictionary<string, bool>();
            Subtotal = 0; IVA = 0;Recargos = 0; Descuento = 0;
            diccionario.Add("ComprobanteEntrada", false);
            eventAggregator.GetEvent<PubSubEvent<bool>>().Publish(false);
            ComprobanteEntrada = new ComprobanteEntradaDto();
            eventAggregator.GetEvent<PubSubEvent<Dictionary<string, bool>>>().Publish(diccionario);
        }

        public async void CargaComprobante()
        {
            if (await Servicios.ApiProcessor.GetApi<bool>("Caja/CajasEstado"))
            {
                ComprobanteEntrada.Monto = Subtotal;
                if (ComprobanteEntrada.Monto > 0 && ComprobanteEntrada.NumeroComprobante > 0 && ComprobanteEntrada.SubRubro != null && ComprobanteEntrada.TipoComprobanteEntrada > 0)
                {
                    ComprobanteEntrada.UsuarioId= UsuarioGral.UsuarioId;
                    ComprobanteEntrada.Descuento = Descuento;
                    ComprobanteEntrada.Iva = IVA;
                    ComprobanteEntrada.SubRubroId = ComprobanteEntrada.SubRubro.Id;
                     await Servicios.ApiProcessor.PostApi<ComprobanteEntradaDto>(ComprobanteEntrada, "ComprobanteEntrada/Insert");
                    var Caja = await Servicios.ApiProcessor.GetApi<CajaDto>("Caja/CajaAbierta");
                    var detalleCaja = new DetalleCajaDto
                    {
                        CajaId = Caja.Id,
                        Monto = ComprobanteEntrada.Monto,
                        TipoMovimiento = Constantes.TipoMovimiento.Ingreso,
                        TipoPago = Constantes.TipoPago.Efectivo
                    };
                      await Servicios.ApiProcessor.PostApi<DetalleCajaDto>(detalleCaja, "DetalleCaja/Insert");
                    var diccionario = new Dictionary<string, bool>();
                    diccionario.Add("ComprobanteEntrada", false);
                    eventAggregator.GetEvent<PubSubEvent<bool>>().Publish(true);
                    ComprobanteEntrada = new ComprobanteEntradaDto();
                    Subtotal = 0; IVA = 0;Recargos = 0; Descuento = 0;
                this.eventAggregator.GetEvent<PubSubEvent<decimal>>().Unsubscribe(Venta);
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
    }
}
