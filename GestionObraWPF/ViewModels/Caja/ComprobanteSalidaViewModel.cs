using GestionObraWPF.Constantes;
using GestionObraWPF.DTOs;
using GestionObraWPF.Model;
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
    public class ComprobanteSalidaViewModel : BindableBase
    {
        private ComprobanteSalidaDto _ComprobanteSalida;
        private ObservableCollection<SubRubroDto> _subRubrosEntrada;

        public ComprobanteSalidaDto ComprobanteSalida { get { return _ComprobanteSalida; } set { SetProperty(ref _ComprobanteSalida, value); } }
        public ObservableCollection<SubRubroDto> SubrubrosSalida { get { return _subRubrosEntrada; } set { SetProperty(ref _subRubrosEntrada, value); } }
        public ICommand CargarComando { get; set; }
        public ICommand CancelarCommando { get; set; }
        private IEventAggregator eventAggregator;
        private RubroDto _rubro;
        private decimal _descuento;
        private decimal _total;
        private decimal _subtotal;
        private decimal _IVA;
        private decimal _percepcion;

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
        private ObservableCollection<RubroDto> _rubrosEntrada;
        private decimal _recargos;
        private decimal _retencion;

        private bool _activarSubtotal = true;
        public bool ActivarSubtotal
        {
            get { return _activarSubtotal; }
            set { SetProperty(ref _activarSubtotal, value); }
        }

        public ObservableCollection<RubroDto> Rubros { get { return _rubrosEntrada; } set { SetProperty(ref _rubrosEntrada, value); } }
        public decimal Total
        {
            get { return _total; }
            set
            {
                SetProperty(ref _total, value);
            }
        }
        public RubroDto Rubro
        {
            get { return _rubro; }
            set
            {
                SetProperty(ref _rubro, value);
                Manejar();

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

        public decimal Subtotal
        {
            get { return _subtotal; }
            set
            {
                SetProperty(ref _subtotal, value);
                CalcularTotal();
            }
        }

        private void CalcularTotal()
        {
            Total = Subtotal - Descuento + Recargos + IVA+Retencion+Percepcion; 
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
        private async void Manejar()
        {
            if (Rubro != null)
                SubrubrosSalida = new ObservableCollection<SubRubroDto>(await Servicios.ApiProcessor.GetApi<SubRubroDto[]>($"Subrubro/GetByRubro/{Rubro.Id}"));
        }

        public ComprobanteSalidaViewModel(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            CargarComando = new DelegateCommand(CargaComprobante);
            CancelarCommando = new DelegateCommand(Cancelar);
            ComprobanteSalida = new ComprobanteSalidaDto();
            this.eventAggregator.GetEvent<PubSubEvent<string>>().Subscribe(Usado);
            this.eventAggregator.GetEvent<PubSubEvent<decimal>>().Subscribe(PasandoPrecio);
            this.eventAggregator.GetEvent<PubSubEvent<ComprobanteCompraDto>>().Subscribe(Compra);
        }

        private void PasandoPrecio(decimal obj)
        {
            Subtotal = obj;
            ActivarSubtotal = false;
        }

        private void Compra(ComprobanteCompraDto obj)
        {
            Subtotal = obj.Total;
        }

        private void Usado(string obj)
        {
            switch (obj)
            {
                case "Banco":
                    ComprobanteSalida.Usado = UsadoEn.Banco;
                    break;
                case "Caja":
                    ComprobanteSalida.Usado = UsadoEn.Caja;
                    break;
                case "Proveedor":
                    ComprobanteSalida.Usado = UsadoEn.Proveedor;
                    break;
                case "Venta":
                    ComprobanteSalida.Usado = UsadoEn.Venta;
                    break;
                default:
                    ComprobanteSalida.Detalle = obj;
                    break;
            }
        }

        public async Task Inicializar()
        {
            Rubros = new ObservableCollection<RubroDto>(await ApiProcessor.GetApi<RubroDto[]>("Rubro/GetBySalida"));
        }

        private void Cancelar()
        {
            var diccionario = new Dictionary<string, bool>();
            Subtotal = 0; IVA = 0; Recargos = 0; Descuento = 0; Percepcion = 0;Retencion = 0;
            diccionario.Add("ComprobanteSalida", false);
            ComprobanteSalida = new ComprobanteSalidaDto();
            eventAggregator.GetEvent<PubSubEvent<bool>>().Publish(false);
            eventAggregator.GetEvent<PubSubEvent<Dictionary<string, bool>>>().Publish(diccionario);
            ActivarSubtotal = true;
        }

        public async void CargaComprobante()
        {
            if (await Servicios.ApiProcessor.GetApi<bool>("Caja/CajasEstado"))
            {
                ComprobanteSalida.Monto = Subtotal;
                if (ComprobanteSalida.Monto > 0 && ComprobanteSalida.NumeroComprobante > 0 && ComprobanteSalida.SubRubro != null && ComprobanteSalida.TipoComprobanteSalida > 0)
                {
                    ComprobanteSalida.UsuarioId = UsuarioGral.UsuarioId;
                    ComprobanteSalida.Interes = Recargos;
                    ComprobanteSalida.Descuento = Descuento;
                    ComprobanteSalida.Retenciones = Retencion;
                    ComprobanteSalida.Percepciones = Percepcion;
                    ComprobanteSalida.Iva = IVA;
                    ComprobanteSalida.SubRubroId = ComprobanteSalida.SubRubro.Id;
                    await ApiProcessor.PostApi<ComprobanteSalidaDto>(ComprobanteSalida, "ComprobanteSalida/Insert");
                    var Caja = await ApiProcessor.GetApi<CajaDto>("Caja/CajaAbierta");
                    var detalleCaja = new DetalleCajaDto
                    {
                        CajaId = Caja.Id,
                        Monto = ComprobanteSalida.Monto,
                        TipoMovimiento = Constantes.TipoMovimiento.Egreso,
                        TipoPago = Constantes.TipoPago.Efectivo
                    };
                    await Servicios.ApiProcessor.PostApi<DetalleCajaDto>(detalleCaja, "DetalleCaja/Insert");
                    var diccionario = new Dictionary<string, bool>();
                    diccionario.Add("ComprobanteSalida", false);
                eventAggregator.GetEvent<PubSubEvent<bool>>().Publish(true);
                    eventAggregator.GetEvent<PubSubEvent<Dictionary<string, bool>>>().Publish(diccionario);
                    ComprobanteSalida = new ComprobanteSalidaDto();
                    Subtotal = 0; IVA = 0; Recargos = 0; Descuento = 0; Percepcion = 0;Retencion = 0;
                    ActivarSubtotal = true;
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
