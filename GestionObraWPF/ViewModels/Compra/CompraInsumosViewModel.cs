using GestionObraWPF.DTOs;
using GestionObraWPF.Helpers;
using GestionObraWPF.Model;
using GestionObraWPF.Servicios;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace GestionObraWPF.ViewModels
{
    public class CompraInsumosViewModel : BindableBase
    {
        private ObservableCollection<MaterialDto> _materiales;
        private string _busqueda;
        private MaterialDto _material;
        private DetalleComprobanteDto _detalle;
        private decimal _precio;
        public ICommand QuitarProducto { get; set; }
        public ICommand Agregar { get; set; }
        public ICommand TextChange { get; set; }
        public ICommand Comprar { get; set; }

        private IRegionManager regionManager;

        public ICommand Disminuir { get; set; }
        public ICommand Buscar { get; set; }
        public ICommand Salir { get; }
        public ICommand CrearCommand { get; }

        private int _cantidad;
        private ObservableCollection<DetalleComprobanteDto> _detallesComprobante;
        private ComprobanteCompraDto _comprobante;
        private bool _mostrarCrearObra;
        private bool _compraOpen;
        private decimal _total;
        private decimal _descuento;
        private decimal _subtotal;
        private decimal _IVA;
        private decimal _recargos;
        private decimal _retencion;
        private decimal _percepcion;
        private decimal _subtotalDesc;
        private ObservableCollection<ObraDto> _obras;
        private ObservableCollection<ProveedorDto> _proveedores;
        private string _cuit;
        private bool _tieneProv = true;

        public int Cantidad
        {
            get { return _cantidad; }
            set
            {
                SetProperty(ref _cantidad, value);
            }
        }
        public decimal Precio
        {
            get { return _precio; }
            set
            {
                SetProperty(ref _precio, value);
            }
        }
        public CompraInsumosViewModel(IRegionManager regionManager)
        {
            this.regionManager = regionManager;
            Disminuir = new DelegateCommand(DisminuirMaterial);
            Buscar = new DelegateCommand(Buscando);
            Salir = new DelegateCommand(SalirCompra);
            CrearCommand = new DelegateCommand(GenerarComprobante);
            Agregar = new DelegateCommand(AgregarProducto, () => ObjetoNull.IsNull(Material)).ObservesProperty(() => Material);
            Comprar = new DelegateCommand(ComprarProducto);
            DetallesComprobante = new ObservableCollection<DetalleComprobanteDto>();
            Material = null;
            Comprobante = new ComprobanteCompraDto();
            QuitarProducto = new DelegateCommand(Quitar);
        }

        private async void GenerarComprobante()
        {
            if(Retencion>=0 && IVA>=0 && Descuento>=0 && Percepcion>=0 && Subtotal>0 && Retencion>=0 && (!TieneProv || Comprobante.Proveedor!=null))
            {

                var ultimoComprobante = await ApiProcessor.GetApi<int>("ComprobanteCompra/GetUltimo");

                Comprobante.Recargos = Recargos;
                Comprobante.Descuento = Descuento;
                Comprobante.Iva = IVA;
                Comprobante.Monto = Subtotal;
                if (Comprobante.Obra != null)
                {
                    Comprobante.ObraId = Comprobante.Obra.Id;
                }
                Comprobante.Percepciones = Percepcion;
                Comprobante.Retenciones = Retencion;
                if (TieneProv)
                {
                    Comprobante.ProveedorId = Comprobante.Proveedor.Id;
                    Comprobante.cuit = Comprobante.Proveedor.Cuit;
                }
                else
                {
                    Comprobante.ProveedorId = null;
                    Comprobante.cuit = Cuit;
                }
                Comprobante.Pagado = false;
                Comprobante.NumeroCompra = ultimoComprobante;
                Comprobante.EstaEliminado = false;

                regionManager.RequestNavigate("Contenido","Compra");
                await ApiProcessor.PostApi(Comprobante, "ComprobanteCompra/Insert");
                foreach (var i in DetallesComprobante)
                {
                    StockHelper.AgregarStock(i.MaterialId,Cantidad);
                    await ApiProcessor.PostApi<DetalleComprobanteDto>(i, "DetalleComprobante/Insert");
                }
                MessageBox.Show("Carga de materiales exitosa");

            }
            else
            {
                MessageBox.Show("Faltan ingresar datos");
            }
        }

        

        private void SalirCompra()
        {
            MostrarCrearObra = false;
            Comprobante.Obra = null;
        }

        private async void Buscando()
        {
            if (string.IsNullOrWhiteSpace(Busqueda))
            {
                Materiales = new ObservableCollection<MaterialDto>(await Servicios.ApiProcessor.GetApi<MaterialDto[]>("Material/GetAll"));

            }
            else
            {
                Materiales = new ObservableCollection<MaterialDto>(await Servicios.ApiProcessor.GetApi<MaterialDto[]>($"Material/GetByFilter/{Busqueda}"));
            }
        }

        private  void ComprarProducto()
        {
            if (DetallesComprobante.Count() > 0)
            {
            MostrarCrearObra = true;
            }
            else
            {
                MessageBox.Show("Ingrese productos a su carrito antes de realizar la compra!");
            }
        }

        private void DisminuirMaterial()
        {
            if (Detalle != null)
            {
                if (Detalle != null && Detalle.Cantidad - 1 > 0)
                {
                    Detalle.Cantidad -= 1;

                }
                else if (Detalle.Cantidad - 1 <= 0)
                {
                    DetallesComprobante.Remove(Detalle);
                }
                Subtotal = DetallesComprobante.Sum(x => x.SubTotal);
            }
        }

        private void Quitar()
        {
            if (Detalle != null)
            {
                DetallesComprobante.Remove(Detalle);
                Subtotal = DetallesComprobante.Sum(x => x.SubTotal);
            }
        }

        private void AgregarProducto()
        {
            if (!DetallesComprobante.Any(x => x.MaterialId == Material.Id))
            {
                if (Cantidad > 0 && Precio > 0 && Material != null)
                {
                    DetallesComprobante.Add(new DetalleComprobanteDto { MaterialId = Material.Id, ComprobanteId = ComprobanteId, PrecioUnitario = Precio, Cantidad = Cantidad, Codigo = Material.Codigo, Descripcion = Material.Descripcion, EstaEliminado = false });
                }
            }
            else
            {
                DetallesComprobante.FirstOrDefault(x => x.MaterialId == Material.Id).Cantidad += Cantidad;
            }
            Subtotal = DetallesComprobante.Sum(x => x.SubTotal);
        }

        private void CalcularTotal()
        {
            Total = ((Subtotal - Descuento) + Recargos) + Retencion + IVA + Percepcion;
        }

        public string Busqueda
        {
            get { return _busqueda; }
            set
            {
                SetProperty(ref _busqueda, value);
            }
        }

        public decimal Total
        {
            get { return _total; }
            set
            {
                SetProperty(ref _total, value);
            }
        }
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
        public bool MostrarCrearObra
        {
            get { return _mostrarCrearObra; }
            set
            {
                SetProperty(ref _mostrarCrearObra, value);
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
        public decimal SubtotalDesc
        {
            get { return _subtotalDesc; }
            set
            {
                SetProperty(ref _subtotalDesc, value);
                CalcularTotal();
            }
        }

        public ObservableCollection<ObraDto> Obras
        {
            get { return _obras; }
            set { SetProperty(ref _obras, value); }
        }
        public ObservableCollection<ProveedorDto> Proveedores
        {
            get { return _proveedores; }
            set { SetProperty(ref _proveedores, value); }
        }

        public ObservableCollection<MaterialDto> Materiales
        {
            get { return _materiales; }
            set { SetProperty(ref _materiales, value); }
        }

        public ComprobanteCompraDto Comprobante
        {
            get { return _comprobante; }
            set { SetProperty(ref _comprobante, value); }
        }

        private long ComprobanteId { get; set; }

        public MaterialDto Material
        {
            get { return _material; }
            set { SetProperty(ref _material, value); }
        }
        public DetalleComprobanteDto Detalle
        {
            get { return _detalle; }
            set { SetProperty(ref _detalle, value); }
        }

        public ObservableCollection<DetalleComprobanteDto> DetallesComprobante
        {
            get { return _detallesComprobante; }
            set
            {
                SetProperty(ref _detallesComprobante, value);
            }
        }

        public bool CompraOpen
        {
            get { return _compraOpen; }
            set
            {
                SetProperty(ref _compraOpen, value);
            }
        }

        public string Cuit
        {
            get { return _cuit; }
            set
            {
                SetProperty(ref _cuit, value);
            }
        }

        public bool TieneProv {
            get { return _tieneProv; }
            set
            {
                SetProperty(ref _tieneProv, value);
                if (_tieneProv)
                {
                    ProveedorAct();
                }
                else
                {
                    Proveedores = null;
                }
            }
        }

        public async void Inicialize()
        {
            Obras = new ObservableCollection<ObraDto>(await Servicios.ApiProcessor.GetApi<ObraDto[]>("Obra/GetAll"));
            ProveedorAct();
            Materiales = new ObservableCollection<MaterialDto>(await Servicios.ApiProcessor.GetApi<MaterialDto[]>("Material/GetAll"));
            ComprobanteId = await ApiProcessor.GetApi<long>("ComprobanteCompra/GetLast");
            Obras.Add(null);
        }
        private async void ProveedorAct()
        {
            Proveedores = new ObservableCollection<ProveedorDto>(await Servicios.ApiProcessor.GetApi<ProveedorDto[]>("Proveedor/GetAll"));
        }
    }
}
