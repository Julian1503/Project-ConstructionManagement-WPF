using GestionObraWPF.DTOs;
using GestionObraWPF.Servicios;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GestionObraWPF.ViewModels
{
    public class ComprobanteCompraViewModel : BindableBase
    {
        private DateTime _fechaDesde = DateTime.Now;
        private DateTime _fechaHasta = DateTime.Now;
        private ObservableCollection<ComprobanteCompraDto> _comprobanteCompra;
        private decimal _iva;
        private decimal _intereses;
        private decimal _descuentos;
        private decimal _percepciones;
        private decimal _retenciones;
        private decimal _total;
        private int _blanco;
        private int _negro;
        public DelegateCommand<ComprobanteCompraDto> AbrirMateriales { get; set; }
        private string _cuit;
        public string Cuit
        {
            get { return _cuit; }
            set { SetProperty(ref _cuit, value); }
        }

        private bool _activarCuit;
        public bool ActivarCuit
        {
            get { return _activarCuit; }
            set { SetProperty(ref _activarCuit, value); if (ActivarCuit) { ActivarProveedores = false; ActivarObra = false; } }
        }

        private ObservableCollection<ProveedorDto> _proveedores;
        public ObservableCollection<ProveedorDto> Proveedores
        {
            get { return _proveedores; }
            set { SetProperty(ref _proveedores, value); }
        }
        private ObservableCollection<ObraDto> _obras;
        public ObservableCollection<ObraDto> Obras
        {
            get { return _obras; }
            set { SetProperty(ref _obras, value); }
        }

        private ObraDto _obra;
        public ObraDto Obra
        {
            get { return _obra; }
            set { SetProperty(ref _obra, value); }
        }

        private bool _activarProveedores;
        public bool ActivarProveedores
        {
            get { return _activarProveedores; }
            set { SetProperty(ref _activarProveedores, value); if (ActivarProveedores) { ActivarObra = false; ActivarCuit = false; } }
        }

        private bool _activarObra;
        public bool ActivarObra
        {
            get { return _activarObra; }
            set { SetProperty(ref _activarObra, value); if (ActivarObra) { ActivarProveedores = false; ActivarCuit = false; } }
        }

        private ProveedorDto _proveedor;
        public ProveedorDto Proveedor
        {
            get { return _proveedor; }
            set { SetProperty(ref _proveedor, value); }
        }
        private decimal _pagando;
        public decimal Pagando
        {
            get { return _pagando; }
            set { SetProperty(ref _pagando, value); }
        }
        private ObservableCollection<DetalleComprobanteDto> _detalle;
        public ObservableCollection<DetalleComprobanteDto> Detalle
        {
            get { return _detalle; }
            set { SetProperty(ref _detalle, value); }
        }
        private bool _abrirDetalle;
        public bool AbrirDetalle
        {
            get { return _abrirDetalle; }
            set { SetProperty(ref _abrirDetalle, value); }
        }
        private decimal _subtotal;
        public decimal Subtotal
        {
            get { return _subtotal; }
            set { SetProperty(ref _subtotal, value); }
        }
        public decimal Iva { get { return _iva; } set { SetProperty(ref _iva, value); } }
        public decimal Intereses { get { return _intereses; } set { SetProperty(ref _intereses, value); } }
        public decimal Descuentos { get { return _descuentos; } set { SetProperty(ref _descuentos, value); } }
        public decimal Percepciones { get { return _percepciones; } set { SetProperty(ref _percepciones, value); } }
        public decimal Retenciones { get { return _retenciones; } set { SetProperty(ref _retenciones, value); } }
        public decimal Total { get { return _total; } set { SetProperty(ref _total, value); } }
        public int Blanco { get { return _blanco; } set { SetProperty(ref _blanco, value); } }
        public int Negro { get { return _negro; } set { SetProperty(ref _negro, value); } }

        public ComprobanteCompraViewModel()
        {
            FiltrarCommand = new DelegateCommand(Filtrar);
            AbrirMateriales = new DelegateCommand<ComprobanteCompraDto>(Detalles);
        }

        private async void Detalles(ComprobanteCompraDto obj)
        {
            if (obj != null)
            {
                AbrirDetalle = true;
                Detalle = new ObservableCollection<DetalleComprobanteDto>(await ApiProcessor.GetApi<DetalleComprobanteDto[]>($"DetalleComprobante/GetByComprobante/{obj.Id}"));
                Subtotal = Detalle.Sum(x => x.SubTotal);
            }

        }

        public ICommand FiltrarCommand { get; set; }
        public DateTime FechaDesde { get { return _fechaDesde; } set { SetProperty(ref _fechaDesde, value); } }
        public DateTime FechaHasta { get { return _fechaHasta; } set { SetProperty(ref _fechaHasta, value); } }

        public ObservableCollection<ComprobanteCompraDto> ComprobantesCompra { get { return _comprobanteCompra; } set { SetProperty(ref _comprobanteCompra, value); } }

        public async Task Inicializar()
        {
            var a = await ApiProcessor.GetApi<object>("ComprobanteCompra/GetAll");
            ComprobantesCompra = new ObservableCollection<ComprobanteCompraDto>(await ApiProcessor.GetApi<ComprobanteCompraDto[]>("ComprobanteCompra/GetAll"));
            Proveedores = new ObservableCollection<ProveedorDto>(await ApiProcessor.GetApi<ProveedorDto[]>("Proveedor/GetAll"));
            Obras = new ObservableCollection<ObraDto>(await ApiProcessor.GetApi<ObraDto[]>("Obra/GetAll"));
                CalcularComprobantes();
        }

        private async void Filtrar()
        {
            if (FechaDesde <= FechaHasta)
            {
                if (ActivarCuit && !string.IsNullOrWhiteSpace(Cuit))
                {
                    ComprobantesCompra = new ObservableCollection<ComprobanteCompraDto>(await ApiProcessor.GetApi<ComprobanteCompraDto[]>($"ComprobanteCompra/GetByCuit/{FechaDesde.ToString("MM-dd-yyyy")}/{FechaHasta.ToString("MM-dd-yyyy")}/{Cuit}"));
                }
                else if (ActivarProveedores)
                {
                    ComprobantesCompra = new ObservableCollection<ComprobanteCompraDto>(await ApiProcessor.GetApi<ComprobanteCompraDto[]>($"ComprobanteCompra/GetByProveedor/{FechaDesde.ToString("MM-dd-yyyy")}/{FechaHasta.ToString("MM-dd-yyyy")}/{Proveedor.Id}"));
                }
                else if (ActivarObra)
                {
                    ComprobantesCompra = new ObservableCollection<ComprobanteCompraDto>(await ApiProcessor.GetApi<ComprobanteCompraDto[]>($"ComprobanteCompra/GetByFechaObra/{FechaDesde.ToString("MM-dd-yyyy")}/{FechaHasta.ToString("MM-dd-yyyy")}/{Obra.Id}"));
                }
                else
                {
                    ComprobantesCompra = new ObservableCollection<ComprobanteCompraDto>(await ApiProcessor.GetApi<ComprobanteCompraDto[]>($"ComprobanteCompra/GetByFecha/{FechaDesde.ToString("MM-dd-yyyy")}/{FechaHasta.ToString("MM-dd-yyyy")}"));
                }
                CalcularComprobantes();
            }
        }

        private void CalcularComprobantes()
        {
            if (ComprobantesCompra != null && ComprobantesCompra.Count>0)
            {
                Iva = ComprobantesCompra.Sum(x => x.Iva);
                Retenciones = ComprobantesCompra.Sum(x => x.Retenciones);
                Intereses = ComprobantesCompra.Sum(x => x.Recargos);
                Descuentos = ComprobantesCompra.Sum(x => x.Descuento);
                Percepciones = ComprobantesCompra.Sum(x => x.Percepciones);
                Retenciones = ComprobantesCompra.Sum(x => x.Retenciones);
                Total = ComprobantesCompra.Sum(x => x.Monto) + Iva + Retenciones + Intereses - Descuentos + Percepciones;
                Pagando = ComprobantesCompra.Sum(x => x.Pagando);
                Blanco = ComprobantesCompra.Where(x => x.Iva > 0 || x.Percepciones > 0 || x.Retenciones > 0).Count();
                Negro = ComprobantesCompra.Count() - Blanco;
            }
        }
    }
}
