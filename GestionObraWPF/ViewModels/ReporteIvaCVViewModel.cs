using GestionObraWPF.DTOs;
using GestionObraWPF.Servicios;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GestionObraWPF.ViewModels
{
    public class ReporteIvaCVViewModel : BindableBase
    {
        private ObservableCollection<IvaCompraVentaDto> _ivaCompraVenta;
        public ObservableCollection<IvaCompraVentaDto> IvaCompraVenta
        {
            get { return _ivaCompraVenta; }
            set { SetProperty(ref _ivaCompraVenta, value); }
        }
        private decimal _ivaCompra;
        public decimal IvaCompra
        {
            get { return _ivaCompra; }
            set { SetProperty(ref _ivaCompra, value); }
        }
        private decimal _ivaVenta;
        public decimal IvaVenta
        {
            get { return _ivaVenta; }
            set { SetProperty(ref _ivaVenta, value); }
        }
        private decimal _total;
        public decimal Total
        {
            get { return _total; }
            set { SetProperty(ref _total, value); }
        }
        private ObservableCollection<PresupuestoDto> _presupuesto;
        public ObservableCollection<PresupuestoDto> Presupuestos
        {
            get { return _presupuesto; }
            set { SetProperty(ref _presupuesto, value); }
        }

        private ObservableCollection<ComprobanteCompraDto> _comprobanteCompra;
        private DateTime _fechaDesde = DateTime.Now;
        private DateTime _fechaHasta = DateTime.Now;

        public ObservableCollection<ComprobanteCompraDto> ComprobanteCompra
        {
            get { return _comprobanteCompra; }
            set { SetProperty(ref _comprobanteCompra, value); }
        }

        public ReporteIvaCVViewModel()
        {
            IvaCompraVenta = new ObservableCollection<IvaCompraVentaDto>();
            FiltrarCommand = new DelegateCommand(Filtrar);
        }

        private async void Filtrar()
        {
            if (FechaDesde <= FechaHasta)
            {
                ComprobanteCompra = new ObservableCollection<ComprobanteCompraDto>(await ApiProcessor.GetApi<ComprobanteCompraDto[]>($"ComprobanteCompra/GetByFecha/{FechaDesde.ToString("MM-dd-yyyy")}/{FechaHasta.ToString("MM-dd-yyyy")}"));
                Presupuestos = new ObservableCollection<PresupuestoDto>(await ApiProcessor.GetApi<PresupuestoDto[]>($"Presupuesto/GetByFacturadoFecha/{FechaDesde.ToString("MM-dd-yyyy")}/{FechaHasta.ToString("MM-dd-yyyy")}"));
                Calcular();
            }
        }

        public ICommand FiltrarCommand { get; set; }
        public DateTime FechaDesde { get { return _fechaDesde; } set { SetProperty(ref _fechaDesde, value); } }
        public DateTime FechaHasta { get { return _fechaHasta; } set { SetProperty(ref _fechaHasta, value); } }


        public async Task Inicializar()
        {
            Presupuestos = new ObservableCollection<PresupuestoDto>(await ApiProcessor.GetApi<PresupuestoDto[]>("Presupuesto/GetFacturado"));
            ComprobanteCompra = new ObservableCollection<ComprobanteCompraDto>(await ApiProcessor.GetApi<ComprobanteCompraDto[]>("ComprobanteCompra/GetAll"));
            Calcular();
        }

        private void Calcular()
        {
            IvaCompraVenta.Clear();
            IvaCompra = 0;
            IvaVenta = 0;
            foreach (var item in Presupuestos.Where(x=>x.Iva!=0))
            {
                IvaCompraVenta.Add(new IvaCompraVentaDto { NumeroComprobante = item.NumeroFacturacion == null ? 0 : (long)item.NumeroFacturacion, Cae=item.Cae == null ? 0 : (long)item.Cae, CondicionIva = item.Empresa.CondicionIva.Descripcion,Fecha=item.FechaFacturacion,RazonSocial=item.Empresa.RazonSocial,Debe = item.Iva, CUIT = item.Empresa.Cuit, Monto = item.Total });
                IvaVenta += item.Iva;
            }
            foreach (var item in ComprobanteCompra.Where(x => x.Iva != 0))
            {
                IvaCompraVenta.Add(new IvaCompraVentaDto { Haber = item.Iva, CUIT = item.CUIT, Monto = item.Total, Fecha = item.Fecha, RazonSocial = item.Proveedor == null ? "" : item.Proveedor.RazonSocial, CondicionIva = item.Proveedor == null ? "" : item.Proveedor.CondicionIva.Descripcion, Cae = item.Cae == null ? 0 : (long)item.Cae, NumeroComprobante = item.NumeroCompronte == null ? 0 : (long)item.NumeroCompronte });
                IvaCompra += item.Iva;
            }
            Total = IvaVenta - IvaCompra;
        }
    }
}
