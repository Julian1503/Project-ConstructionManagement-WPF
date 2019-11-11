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
    public class ReporteCuentaViewModel : BindableBase
    {
        private ObservableCollection<CuentaDto> _cuentas;
        public ObservableCollection<CuentaDto> Cuentas
        {
            get { return _cuentas; }
            set { SetProperty(ref _cuentas, value); }
        }
        private ObservableCollection<ComprobanteEntradaDto> _comprobanteEntrada;
        public ObservableCollection<ComprobanteEntradaDto> ComprobanteEntrada
        {
            get { return _comprobanteEntrada; }
            set { SetProperty(ref _comprobanteEntrada, value); }
        }
        private ObservableCollection<ComprobanteSalidaDto> _comprobanteSalidas;
        public ObservableCollection<ComprobanteSalidaDto> ComprobantesSalidas
        {
            get { return _comprobanteSalidas; }
            set { SetProperty(ref _comprobanteSalidas, value); }
        }
        private ObservableCollection<CuentaCorrienteDto> _cuentaCorrientes;
        public ObservableCollection<CuentaCorrienteDto> CuentaCorrientes
        {
            get { return _cuentaCorrientes; }
            set { SetProperty(ref _cuentaCorrientes, value); }
        }

        private ObservableCollection<DetalleCajaDto> _detalleCaja;
        public ObservableCollection<DetalleCajaDto> DetalleCaja
        {
            get { return _detalleCaja; }
            set { SetProperty(ref _detalleCaja, value); }
        }
        private ObservableCollection<OperacionDto> _operaciones;
        public ObservableCollection<OperacionDto> Operaciones
        {
            get { return _operaciones; }
            set { SetProperty(ref _operaciones, value); }
        }

        public async Task Inicializar()
        {
            Operaciones = new ObservableCollection<OperacionDto>(await ApiProcessor.GetApi<OperacionDto[]>("Operacion/GetAll"));
            ComprobantesSalidas = new ObservableCollection<ComprobanteSalidaDto>(await ApiProcessor.GetApi<ComprobanteSalidaDto[]>("ComprobanteSalida/GetAll"));
            ComprobanteEntrada = new ObservableCollection<ComprobanteEntradaDto>(await ApiProcessor.GetApi<ComprobanteEntradaDto[]>("ComprobanteEntrada/GetAll"));
            CuentaCorrientes = new ObservableCollection<CuentaCorrienteDto>(await Servicios.ApiProcessor.GetApi<CuentaCorrienteDto[]>("CuentaCorriente/GetAll"));
            DetalleCaja = new ObservableCollection<DetalleCajaDto>(await Servicios.ApiProcessor.GetApi<DetalleCajaDto[]>("DetalleCaja/GetAll"));
            await Calcular(false);
        }

        private async Task Calcular(bool valor)
        {
            Cuentas.Clear();
            if (CuentaCorrientes != null)
            {
                if (Operaciones != null)
                {
                    var bancos = Operaciones.Where(x => x.FechaVencimiento <= DateTime.Now && x.CuentaCorrienteId != 3).Sum(x => x.Debe) - Operaciones.Where(x => x.FechaVencimiento <= DateTime.Now && x.CuentaCorrienteId != 3).Sum(x => x.Haber);
                    Cuentas.Add(new CuentaDto { Id = 1, DescripcionCuenta = $"Bancos", SaldoCuenta = bancos });
                }
                foreach (var i in CuentaCorrientes)
                {
                    if (valor)
                    {
                        Operaciones = new ObservableCollection<OperacionDto>(await ApiProcessor.GetApi<OperacionDto[]>($"Operacion/GetByFecha/{FechaDesde.ToString("MM-dd-yyyy")}/{FechaHasta.ToString("MM-dd-yyyy")}/{i.Id}"));
                    }
                    else
                    {
                        Operaciones = new ObservableCollection<OperacionDto>(await ApiProcessor.GetApi<OperacionDto[]>($"Operacion/GetByBanco/{i.BancoId}"));
                    }
                    var total = 0m;
                    foreach (var item in Operaciones.Where(x => x.FechaEmision.Value.Year == DateTime.Now.Year && x.CuentaCorrienteId != 3).OrderBy(x => x.FechaEmision))
                    {
                        if (item.Haber != 0)
                        {
                            total -= (decimal)item.Haber;
                        }
                        else
                        {
                            total += (decimal)item.Debe;
                        }
                    }
                    if (i.BancoId != 3)
                    {
                        Cuentas.Add(new CuentaDto { Id = 1, DescripcionSubRubro = $"Banco {i.Banco.RazonSocial}", SaldoSubRubro = total });
                    }
                   
                }
                foreach (var i in CuentaCorrientes.Where(x=>x.BancoId==3))
                {
                    var valoresDep = 0m;
                    foreach (var item in Operaciones.Where(x => x.FechaEmision.Value.Year == DateTime.Now.Year).OrderBy(x => x.FechaEmision))
                    {
                        if (item.Haber != 0)
                        {
                            valoresDep -= (decimal)item.Haber;
                        }
                        else
                        {
                            valoresDep += (decimal)item.Debe;
                        }
                    }
                        Cuentas.Add(new CuentaDto { Id = 1, DescripcionCuenta = $"{i.Banco.RazonSocial}", SaldoCuenta = valoresDep });
                }
            }

            if (DetalleCaja != null)
            {
                var caja = DetalleCaja.Where(x => x.Caja.FechaApertura == DetalleCaja.Max(y => y.Caja.FechaApertura));
                var total = caja.Where(x => x.TipoMovimiento == Constantes.TipoMovimiento.Ingreso).Sum(x => x.Monto) - caja.Where(x => x.TipoMovimiento == Constantes.TipoMovimiento.Egreso).Sum(x => x.Monto);
                Cuentas.Add(new CuentaDto { Id = 1, DescripcionCuenta = $"Caja", SaldoCuenta = total });
            }
            if (ComprobantesSalidas != null)
            {
                var lista = ComprobantesSalidas.Where(x => x.SubRubroId != null).GroupBy(x => x.SubRubro.Rubro.Descripcion).ToList();
                foreach (var item in lista)
                {
                    Cuentas.Add(new CuentaDto { DescripcionCuenta = item.Key, SaldoCuenta = item.Sum(x => x.Total) });
                    var sub = item.GroupBy(x => x.SubRubro.Descripcion).ToList();
                    foreach (var i in sub)
                    {
                        Cuentas.Add(new CuentaDto { DescripcionSubRubro = i.Key, SaldoSubRubro = i.Sum(x => x.Total) });
                    }
                }
            }
            if (ComprobanteEntrada != null)
            {
                var lista = ComprobanteEntrada.Where(x=>x.SubRubroId!=null).GroupBy(x => x.SubRubro.Rubro.Descripcion).ToList();
                foreach (var item in lista)
                {
                    Cuentas.Add(new CuentaDto { DescripcionCuenta = item.Key, SaldoCuenta = item.Sum(x => x.Total) });
                    var sub = item.GroupBy(x => x.SubRubro.Descripcion).ToList();
                    foreach (var i in sub)
                    {
                        Cuentas.Add(new CuentaDto { DescripcionSubRubro = i.Key, SaldoSubRubro = i.Sum(x => x.Total) });
                    }
                }
            }
        }

        public async void Filtrar()
        {
            if (FechaDesde <= FechaHasta)
            {
                ComprobanteEntrada = new ObservableCollection<ComprobanteEntradaDto>(await ApiProcessor.GetApi<ComprobanteEntradaDto[]>($"ComprobanteEntrada/GetByFecha/{FechaDesde.ToString("MM-dd-yyyy")}/{FechaHasta.ToString("MM-dd-yyyy")}"));
                ComprobantesSalidas = new ObservableCollection<ComprobanteSalidaDto>(await ApiProcessor.GetApi<ComprobanteSalidaDto[]>($"ComprobanteSalida/GetByFecha/{FechaDesde.ToString("MM-dd-yyyy")}/{FechaHasta.ToString("MM-dd-yyyy")}"));
                DetalleCaja = new ObservableCollection<DetalleCajaDto>(await Servicios.ApiProcessor.GetApi<DetalleCajaDto[]>($"DetalleCaja/GetByFecha/{FechaDesde.ToString("MM-dd-yyyy")}/{FechaHasta.ToString("MM-dd-yyyy")}"));
                await Calcular(true);
            }
        }

        public ReporteCuentaViewModel()
        {
            Cuentas = new ObservableCollection<CuentaDto>();
            FiltrarCommand = new DelegateCommand(Filtrar);
        }
        public ICommand FiltrarCommand { get; set; }
        private DateTime _fechaDesde = DateTime.Now;
        private DateTime _fechaHasta = DateTime.Now;
        public DateTime FechaDesde { get { return _fechaDesde; } set { SetProperty(ref _fechaDesde, value); } }
        public DateTime FechaHasta { get { return _fechaHasta; } set { SetProperty(ref _fechaHasta, value); } }
    }
}
