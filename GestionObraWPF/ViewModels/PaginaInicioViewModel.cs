using GestionObraWPF.Constantes;
using GestionObraWPF.DTOs;
using GestionObraWPF.Servicios;
using GestionObraWPF.Views.ViewControls.Model;
using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Wpf;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace GestionObraWPF.ViewModels
{
    public class PaginaInicioViewModel : BindableBase
    {


        public class DateModel
        {
            public System.DateTime DateTime { get; set; }
            public double Value { get; set; }
        }
        public class ClaseGrafico
        {
            public DateTime Fecha { get; set; }
            public decimal Monto { get; set; }
            public TipoMovimiento TipoMovimiento{ get; set; }
        }

        private ObservableCollection<CuadroInicio> _cuadros;
        private ObservableCollection<CuentaCorrienteDto> _cuentaCorrientes;
        private SeriesCollection _seriesCollection;
        private string[] _labels;
        private Func<double, string> _yFormatter;
        private ZoomingOptions _zoomingMode;
        private Func<double, string> _xFormatter;
        private ObservableCollection<DetalleCajaDto> _detalleCaja;
        private Func<double, string> _xEgresos;
        public Func<double, string> XCashFlow
        {
            get { return _xEgresos; }
            set { SetProperty(ref _xEgresos, value); }
        }
        private Func<double, string> _yEgresos;
        public Func<double, string> YCashFlow
        {
            get { return _yEgresos; }
            set { SetProperty(ref _yEgresos, value); }
        }
        private ObservableCollection<JornalDto> _jornales;
        public ObservableCollection<JornalDto> Jornales
        {
            get { return _jornales; }
            set { SetProperty(ref _jornales, value); }
        }
        private SeriesCollection _seriesEgresos;
        public SeriesCollection SeriesCashFlow
        {
            get { return _seriesEgresos; }
            set { SetProperty(ref _seriesEgresos, value); }
        }
        public ObservableCollection<CuadroInicio> Cuadros { get { return _cuadros; } set { SetProperty(ref _cuadros, value); } }
        public ObservableCollection<CuentaCorrienteDto> CuentaCorrientes { get { return _cuentaCorrientes; } set { SetProperty(ref _cuentaCorrientes, value); } }
        public ObservableCollection<DetalleCajaDto> DetalleCaja { get { return _detalleCaja; } set { SetProperty(ref _detalleCaja, value); } }
        private decimal _oficina;
        public decimal Oficina
        {
            get { return _oficina; }
            set { SetProperty(ref _oficina, value); }
        }
        private decimal _total;
        public decimal Total
        {
            get { return _total; }
            set { SetProperty(ref _total, value); }
        }
        private ObservableCollection<ComprobanteCompraDto> _comprobantesCompra;
        public ObservableCollection<ComprobanteCompraDto> ComprobanteCompras
        {
            get { return _comprobantesCompra; }
            set { SetProperty(ref _comprobantesCompra, value); }
        }
        public List<OperacionDto> Operaciones { get; private set; }
        public decimal Descontado { get; private set; }
        public decimal Debe { get; private set; }
        public decimal Haber { get; private set; }
        public decimal Diferencia { get; private set; }
        public ZoomingOptions ZoomingMode
        {
            get { return _zoomingMode; }
            set
            {
                SetProperty(ref _zoomingMode, value);
            }
        }
        private ObservableCollection<ClaseGrafico> graficas;
        public ObservableCollection<ClaseGrafico> Graficas
        {
            get { return graficas; }
            set { SetProperty(ref graficas, value); }
        }
        private int[] _obras;
        public int[] Obras
        {
            get { return _obras; }
            set { SetProperty(ref _obras, value); }
        }
        private ObservableCollection<PresupuestoDto> _presupuesos;
        private SeriesCollection _series;
        private SeriesCollection _seriesPresupuesto;

        public ObservableCollection<PresupuestoDto> Presupuestos
        {
            get { return _presupuesos; }
            set { SetProperty(ref _presupuesos, value); }
        }

        private SeriesCollection _seriesSalida;
        public SeriesCollection SeriesSalida
        {
            get { return _seriesSalida; }
            set { SetProperty(ref _seriesSalida, value); }
        }

        private SeriesCollection _seriesEntrada;
        public SeriesCollection SeriesEntrada
        {
            get { return _seriesEntrada; }
            set { SetProperty(ref _seriesEntrada, value); }
        }
        private decimal _totalPresupuesto;
        public decimal TotalPresupuesto
        {
            get { return _totalPresupuesto; }
            set { SetProperty(ref _totalPresupuesto, value); }
        }
        public SeriesCollection SeriesCollection { get { return _seriesCollection; } set { SetProperty(ref _seriesCollection, value); } }
        public Func<ChartPoint, string> PointLabel { get; set; }
        public Func<ChartPoint, string> PointLabelPresupuesto { get; set; }
        public SeriesCollection Series { get { return _series; } set { SetProperty(ref _series, value); } }
        public SeriesCollection SeriesPresupuesto { get { return _seriesPresupuesto; } set { SetProperty(ref _seriesPresupuesto, value); } }
        public string[] Labels { get { return _labels; } set { SetProperty(ref _labels, value); } }
        public Func<double, string> YFormatter { get { return _yFormatter; } set { SetProperty(ref _yFormatter, value); } }
        public Func<double, string> XFormatter { get { return _xFormatter; } set { SetProperty(ref _xFormatter, value); } }
        public PaginaInicioViewModel()
        {
            Cuadros = new ObservableCollection<CuadroInicio>();
            Labels = new string[] { "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre" };
            var dayConfig = Mappers.Xy<DateModel>()
  .X(dateModel => dateModel.DateTime.Ticks / (TimeSpan.FromDays(1).Ticks * 30.44))
  .Y(dateModel => dateModel.Value);
            SeriesCollection = new SeriesCollection(dayConfig);
            PointLabel = chartPoint =>
               string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation);
            PointLabelPresupuesto = chartPointer =>
              string.Format("{0} ({1:P})", chartPointer.Y, chartPointer.Participation);
            Series = new SeriesCollection();
            Graficas = new ObservableCollection<ClaseGrafico>();
            SeriesPresupuesto = new SeriesCollection();
            SeriesEntrada = new SeriesCollection();
            SeriesSalida = new SeriesCollection();
            SeriesCashFlow = new SeriesCollection(dayConfig);
            ZoomingMode = ZoomingOptions.X;
        }

        //{
        //    new CuadroInicio{ Titulo="Obras en proceso",Cantidad="10", Icono=MaterialDesignThemes.Wpf.PackIconKind.AccountBalance},
        //    new CuadroInicio{ Titulo="Dinero Caja",Cantidad="1000", Icono=MaterialDesignThemes.Wpf.PackIconKind.AccountBalance},
        //    new CuadroInicio{ Titulo="Obras en proceso",Cantidad="10", Icono=MaterialDesignThemes.Wpf.PackIconKind.AccountBalance}
        //};]


        public async Task Inicializar()
        {
            try
            {
                var totalIngreso = 0d;
                var totalEgreso = 0d;
                var cashflow = new ChartValues<DateModel>();
                var egresos = new ChartValues<DateModel>();
                CuentaCorrientes = new ObservableCollection<CuentaCorrienteDto>(await Servicios.ApiProcessor.GetApi<CuentaCorrienteDto[]>("CuentaCorriente/GetAll"));
                ComprobanteCompras = new ObservableCollection<ComprobanteCompraDto>(await ApiProcessor.GetApi<ComprobanteCompraDto[]>("ComprobanteCompra/GetAll"));
                var comprobanteSalida = await ApiProcessor.GetApi<decimal[]>("ComprobanteSalida/GetPorcentaje");
                var comprobanteEntrada = await ApiProcessor.GetApi<decimal[]>("ComprobanteEntrada/GetPorcentaje");

                DetalleCaja = new ObservableCollection<DetalleCajaDto>(await Servicios.ApiProcessor.GetApi<DetalleCajaDto[]>("DetalleCaja/GetAll"));
                Oficina = await ApiProcessor.GetApi<decimal>("ComprobanteCompra/GetOficina");
                Jornales = new ObservableCollection<JornalDto>(await ApiProcessor.GetApi<JornalDto[]>("Jornal/GetAll"));
                Jornales.Sum(x=>x.Repuestos);
                Jornales.Sum(x=>x.Gasolina);
                Obras = await ApiProcessor.GetApi<int[]>("Obra/NumeroPendientes");
                Presupuestos = new ObservableCollection<PresupuestoDto>(await ApiProcessor.GetApi<PresupuestoDto[]>("Presupuesto/GetAprobado"));
                if (CuentaCorrientes != null)
                {
                    foreach (var i in CuentaCorrientes)
                    {
                        Operaciones = new List<OperacionDto>(await ApiProcessor.GetApi<OperacionDto[]>($"Operacion/GetByBanco/{i.BancoId}"));
                        var values = new ChartValues<DateModel>();
                      
                        var total = 0d;
                        foreach (var item in Operaciones.Where(x => x.FechaEmision.Value.Year == DateTime.Now.Year).OrderBy(x => x.FechaEmision))
                        {
                            if (item.Haber != 0)
                            {
                                total -= (double)item.Haber;
                                totalEgreso += (double)item.Haber;
                                Graficas.Add(new ClaseGrafico { Fecha = item.FechaEmision.Value,TipoMovimiento=TipoMovimiento.Egreso,Monto=(decimal)item.Haber});
                            }
                            else
                            {
                                total += (double)item.Debe;
                                totalIngreso += (double)item.Debe;
                                Graficas.Add(new ClaseGrafico { Fecha = item.FechaEmision.Value,TipoMovimiento=TipoMovimiento.Ingreso,Monto= (decimal)item.Debe});
                            }
                            values.Add(new DateModel { DateTime = item.FechaEmision.Value, Value = total });
                        }
                        SeriesCollection.Add(new LineSeries { Values = values, Title = $"{i.Banco.RazonSocial}" });
                        Descontado = (decimal)Operaciones.Where(x => x.Descontado != null).Sum(x => x.Descontado);
                        Debe = (decimal)Operaciones.Where(x => x.Debe != null).Sum(x => x.Debe);
                        Haber = (decimal)Operaciones.Where(x => x.Haber != null).Sum(x => x.Haber);
                        Diferencia += Debe - Haber - Descontado;
                    }
                        Cuadros.Add(new CuadroInicio { Titulo = $"Saldo de bancos", Cantidad = $"{Diferencia.ToString("C")}", Icono = MaterialDesignThemes.Wpf.PackIconKind.AccountBalance });
                }
                if (DetalleCaja != null)
                {
                    var values = new ChartValues<DateModel>();
                    foreach (var i in DetalleCaja.GroupBy(x=>x.CajaId))
                    {
                        var total = 0d;
                        foreach (var item in i)
                        {
                            if (item.TipoMovimiento == Constantes.TipoMovimiento.Egreso)
                            {
                                total -= (double)item.Monto;
                                Graficas.Add(new ClaseGrafico { Fecha = item.Caja.FechaApertura, TipoMovimiento=TipoMovimiento.Egreso,Monto= (decimal)item.Monto});
                            }
                            else
                            {
                                total += (double)item.Monto;
                                Graficas.Add(new ClaseGrafico { Fecha = item.Caja.FechaApertura,TipoMovimiento=TipoMovimiento.Ingreso,Monto= (decimal)item.Monto });
                            }

                            values.Add(new DateModel { DateTime = item.Caja.FechaApertura, Value = total });
                        }
                    }
                    SeriesCollection.Add(new LineSeries { Values = values, Title = $"Caja" });
                }
                var suma = 0d;
                var egreso = 0d;
                foreach (var item in Graficas.OrderBy(x => x.Fecha))
                {
                    if (item.TipoMovimiento == Constantes.TipoMovimiento.Egreso)
                    {
                        egreso += (double)item.Monto;
                        egresos.Add(new DateModel { DateTime = item.Fecha.Date, Value = egreso });
                    }
                    else
                    {
                        suma += (double)item.Monto;
                        cashflow.Add(new DateModel { DateTime = item.Fecha.Date, Value = suma });
                    }
                }

                SeriesCashFlow.Add(new LineSeries { Values = cashflow, Title = $"Ingresos" });
                SeriesCashFlow.Add(new LineSeries { Values = egresos, Title = $"Egresos" });
                YCashFlow = value => value.ToString("C");
                XCashFlow = value =>
                {
                    try
                    {
                        return new DateTime((long)(value * TimeSpan.FromDays(1).Ticks * 30.44)).ToString("M");
                    }
                    catch (Exception)
                    {

                        return DateTime.Now.ToString("M");
                    }
                };
                YFormatter = value => value.ToString("C");

                XFormatter = value =>
                {
                    try
                    {
                        return new DateTime((long)(value * TimeSpan.FromDays(1).Ticks * 30.44)).ToString("M");
                    }
                    catch (Exception)
                    {

                        return DateTime.Now.ToString("M");
                    }
                };

                if (await ApiProcessor.GetApi<bool>("Caja/CajasEstado"))
                {
                    var caja = await Servicios.ApiProcessor.GetApi<CajaDto>("Caja/CajaAbierta");
                    var detallesCaja = new ObservableCollection<DetalleCajaDto>(await Servicios.ApiProcessor.GetApi<DetalleCajaDto[]>($"DetalleCaja/GetByCaja/{caja.Id}"));
                    decimal MontoSistema = 0;
                    if (detallesCaja.Count > 0)
                    {
                        MontoSistema = detallesCaja.Where(x => x.TipoMovimiento == Constantes.TipoMovimiento.Ingreso).Sum(x => x.Monto) - detallesCaja.Where(x => x.TipoMovimiento == Constantes.TipoMovimiento.Egreso).Sum(x => x.Monto);
                    }
                    Cuadros.Add(new CuadroInicio { Titulo = $"Saldo de la caja", Cantidad = $"{MontoSistema.ToString("C")}", Icono = MaterialDesignThemes.Wpf.PackIconKind.CashRegister, Color = "#74D774" });
                }
                else
                {
                    Cuadros.Add(new CuadroInicio { Titulo = $"La caja esta cerrada", Cantidad = "", Icono = MaterialDesignThemes.Wpf.PackIconKind.CashRegister, Color = "#74D774 " });
                }
                Cuadros.Add(new CuadroInicio { Titulo = $"Gastos de oficina", Cantidad = $"{Oficina.ToString("C")}", Icono = MaterialDesignThemes.Wpf.PackIconKind.Shredder,Color = "#F3E5A1" });
                Cuadros.Add(new CuadroInicio { Titulo = $"Cantidad de obras finalizadas", Cantidad = $"{Obras[0]} de {Obras[1]}", Icono = MaterialDesignThemes.Wpf.PackIconKind.TransmissionTower, Color = "#D3B8DD " });
                var Gasolina = Jornales.Sum(x => x.Gasolina);
                var Repuesto = Jornales.Sum(x => x.Repuestos);
                var Compras = ComprobanteCompras.Where(x => NoObras.Obras.Contains(x.Id)).Sum(x=>x.Pagando);
                Total = Gasolina + Repuesto + Compras;
                Series.Add(new PieSeries { Title = "Gasolina", Values = new ChartValues<decimal>(new decimal[] { Gasolina }), DataLabels = true, LabelPoint = PointLabel });
                Series.Add(new PieSeries { Title = "Repuesto", Values = new ChartValues<decimal>(new decimal[] { Repuesto }), DataLabels = true, LabelPoint = PointLabel });
                Series.Add(new PieSeries { Title = "Compras", Values = new ChartValues<decimal>(new decimal[] { Compras }), DataLabels = true, LabelPoint = PointLabel });
                SeriesSalida.Add(new PieSeries { Title = "Blanco", Values = new ChartValues<decimal>(new decimal[] { comprobanteSalida[0] }), DataLabels = true, LabelPoint = PointLabel });
                SeriesSalida.Add(new PieSeries { Title = "Negro", Values = new ChartValues<decimal>(new decimal[] { comprobanteSalida[1] }), DataLabels = true, LabelPoint = PointLabel });
                SeriesEntrada.Add(new PieSeries { Title = "Blanco", Values = new ChartValues<decimal>(new decimal[] { comprobanteEntrada[0] }), DataLabels = true, LabelPoint = PointLabel });
                SeriesEntrada.Add(new PieSeries { Title = "Negro", Values = new ChartValues<decimal>(new decimal[] { comprobanteEntrada[1] }), DataLabels = true, LabelPoint = PointLabel });
                var Cobrado = Presupuestos.Sum(x => x.CobradoSinImpuestos);
                TotalPresupuesto = Presupuestos.Sum(x => x.TotalSinImpuestos);
                var DiferenciaPresupuesto = TotalPresupuesto - Cobrado;
                SeriesPresupuesto.Add(new PieSeries { Title = "Cobrado", Values = new ChartValues<decimal>(new decimal[] { Cobrado }), DataLabels = true, LabelPoint = PointLabelPresupuesto });
                SeriesPresupuesto.Add(new PieSeries { Title = "Faltante", Values = new ChartValues<decimal>(new decimal[] { DiferenciaPresupuesto }), DataLabels = true, LabelPoint = PointLabelPresupuesto });

            }
            catch (Exception)
            {
                MessageBox.Show("No se pudo cargar por problemas de conexion.");
            }
        }
        

    }

}
