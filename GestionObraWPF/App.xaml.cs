using GestionObraWPF.Modules;
using GestionObraWPF.ViewModels;
using GestionObraWPF.ViewModels.Caja;
using GestionObraWPF.Views;
using GestionObraWPF.Views.Reportes;
using GestionObraWPF.Views.ViewControls;
using GestionObraWPF.Views.ViewControls.ABMs;
using GestionObraWPF.Views.ViewControls.Banco;
using GestionObraWPF.Views.ViewControls.Caja;
using GestionObraWPF.Views.ViewControls.Compra;
using GestionObraWPF.Views.ViewControls.Contratista;
using GestionObraWPF.Views.ViewControls.Empleado;
using GestionObraWPF.Views.ViewControls.Obra;
using GestionObraWPF.Views.ViewControls.Obra.Empleado;
using GestionObraWPF.Views.ViewControls.Obra.Jornal;
using GestionObraWPF.Views.ViewControls.Obra.Material;
using GestionObraWPF.Views.ViewControls.Obra.Planificacion;
using GestionObraWPF.Views.ViewControls.Presupuesto;
using GestionObraWPF.Views.ViewControls.Proveedor;
using GestionObraWPF.Views.ViewControls.Venta;
using Prism;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Mvvm;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Navigation;

namespace GestionObraWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        MainWindow ventana;
        public override void Initialize()
        {
            CultureInfo.CurrentCulture = new CultureInfo("es-AR");
            ViewModelLocationProvider.Register<ReporteTesoreria>(()=>new ReporteTesoreriaViewModel());
            base.Initialize();
        }

        protected override Window CreateShell()
        {
            return ventana = Container.Resolve<MainWindow>();
        }

        protected override void InitializeShell(Window shell)
        {
            var login = new Login();
            login.ShowDialog();
            if (!((LoginViewModel)login.DataContext).Login)
            {
                Environment.Exit(0);
            }
            ((MainWindowViewModel)ventana.DataContext).InicializarBotones();
            
        }
 

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<Login, LoginViewModel>("Login");   
            containerRegistry.RegisterForNavigation<MainWindow, MainWindowViewModel>("MainWindow");
            containerRegistry.RegisterForNavigation<ObraABM, ObraABMViewModel>("Inicio");
            containerRegistry.RegisterForNavigation<PaginaInicio, PaginaInicioViewModel>("PantallaPrincipal");
            containerRegistry.RegisterForNavigation<MaterialABM, MaterialABMViewModel>("Material");
            containerRegistry.RegisterForNavigation<ZonaABM, ZonaABMViewModel>("Zona");
            containerRegistry.RegisterForNavigation<CondicionIvaABM, CondicionIvaABMViewModel>("CondicionIva");
            containerRegistry.RegisterForNavigation<EmpresaABM, EmpresaABMViewModel>("Empresa");
            containerRegistry.RegisterForNavigation<BancoABM, BancoABMViewModel>("Banco");
            containerRegistry.RegisterForNavigation<CategoriaABM, CategoriaABMViewModel>("Categoria");
            containerRegistry.RegisterForNavigation<EmpleadoABM, EmpleadoABMViewModel>("Empleado");
            containerRegistry.RegisterForNavigation<PantallaContratista, PantallaContratistaViewModel>("PantallaContratista");
            containerRegistry.RegisterForNavigation<PantallaEmpleado, PantallaEmpleadoViewModel>("PantallaEmpleado");
            containerRegistry.RegisterForNavigation<ListaAsistenciaEmpleado, ListaAsistenciaEmpleadoViewModel>("ListaEmpleado");
            containerRegistry.RegisterForNavigation<ListadoAsistenciaContratista, ListadoAsistenciaContratistaViewModel>("ContratistasAsistencia");
            containerRegistry.RegisterForNavigation<ContratistaABM, ContratistaABMViewModel>("Contratistas");
            containerRegistry.RegisterForNavigation<ProveedorABM, ProveedorABMViewModel>("Proveedor");
            containerRegistry.RegisterForNavigation<Caja, CajaViewModel>("Caja");
            containerRegistry.RegisterForNavigation<RubroABM, RubroABMViewModel>("Rubro");
            containerRegistry.RegisterForNavigation<SubRubroABM, SubRubroABMViewModel>("SubRubro");
            containerRegistry.RegisterForNavigation<TipoGastoABM, TipoGastoABMViewModel>("TipoGasto");
            containerRegistry.RegisterForNavigation<PersonaABM, PersonaABMViewModel>("Persona");
            containerRegistry.RegisterForNavigation<PrecioABM, PrecioABMViewModel>("Precio");
            containerRegistry.RegisterForNavigation<CompraInsumos, CompraInsumosViewModel>("Compra");
            containerRegistry.RegisterForNavigation<DescripcionTareaABM, DescripcionTareaDtoABMViewModel>("DescripcionTarea");
            containerRegistry.RegisterForNavigation<ObraView, ObraViewViewModel>("ObraInicio");
            containerRegistry.RegisterForNavigation<Tareas, TareasViewModel>("Tarea");
            containerRegistry.RegisterForNavigation<ObraDetalle, ObraDetalleViewModel>("ObraDetalle");
            containerRegistry.RegisterForNavigation<IngresoMaterial, IngresoMaterialViewModel>("IngresoMaterial");
            containerRegistry.RegisterForNavigation<SalidaMaterial, SalidaMaterialViewModel>("SalidaMaterial");
            containerRegistry.RegisterForNavigation<PresupuestoInicio, PresupuestoInicioViewModel>("Presupuesto");
            containerRegistry.RegisterForNavigation<PresupuestoDetalle, PresupuestoDetalleViewModel>("PresupuestoDetalle");
            containerRegistry.RegisterForNavigation<VentasPresupuestos, VentasPresupuestosViewModel>("Venta");
            containerRegistry.RegisterForNavigation<Presupuestos, PresupuestosViewModel>("ListaPresupuesto");
            containerRegistry.RegisterForNavigation<Usuario, UsuarioViewModel>("Usuario");
            containerRegistry.RegisterForNavigation<Gantt, GanttViewModel>("Gantt");
            containerRegistry.RegisterForNavigation<GanttReal, GanttRealViewModel>("GanttReal");
            containerRegistry.RegisterForNavigation<VentasAdministracion, VentasAdministracionViewModel>("VentasAdm");
            containerRegistry.RegisterForNavigation<ReporteMaterialesUsados, ReporteMaterialesUsadosViewModel>("MaterialesUsados");
            containerRegistry.RegisterForNavigation<ReporteAdministracion, ReporteAdministracionViewModel>("ReporteAdm");
            containerRegistry.RegisterForNavigation<ReporteRetencion,ReporteRetencionViewModel>("ReporteRetencion");
            containerRegistry.RegisterForNavigation<ReportePercepcion, ReportePercepcionViewModel>("ReportePercepcion");
            containerRegistry.RegisterForNavigation<ComprobanteCompra, ComprobanteCompraViewModel>("ReporteComprobanteCompra");
            containerRegistry.RegisterForNavigation<ReporteComprobanteSalida, ReporteComprobanteSalidaViewModel>("ReporteComprobanteSalida");
            containerRegistry.RegisterForNavigation<ReporteComprobanteEntrada, ReporteComprobanteEntradaViewModel>("ReporteComprobanteEntrada");
            containerRegistry.RegisterForNavigation<ReporteChequeEntrada, ReporteChequeEntradaViewModel>("ReporteChequeEntrada");
            containerRegistry.RegisterForNavigation<ReporteChequeSalida, ReporteChequeSalidaViewModel>("ReporteChequeSalida");
            containerRegistry.RegisterForNavigation<ReporteDeposito, ReporteDepositoViewModel>("ReporteDeposito");
            containerRegistry.RegisterForNavigation<ReporteTransferencia, ReporteTransferenciaViewModel>("ReporteTransferencia");
            containerRegistry.RegisterForNavigation<CajaCerradas, CajaCerradasViewModel>("ReporteCaja");
            containerRegistry.RegisterForNavigation<CajaMenu, CajaMenuViewModel>("CajaMenu");
            containerRegistry.RegisterForNavigation<ReporteObra, ReporteObraViewModel>("ReporteObra");
            containerRegistry.RegisterForNavigation<ReporteObra, ReporteObraViewModel>("ReporteObra");
            containerRegistry.RegisterForNavigation<Proveedor, ProveedorViewModel>("ProveedorMenu");
            containerRegistry.RegisterForNavigation<ReporteCuenta, ReporteCuentaViewModel>("ReporteCuenta");
            containerRegistry.RegisterForNavigation<StockABM, StockABMViewModel>("Stock");
            containerRegistry.RegisterForNavigation<CuentaCorriente, CuentaCorrienteViewModel>("CuentaCorriente");
            containerRegistry.RegisterForNavigation<TransferenciaEntrada, TransferenciaEntradaViewModel>("TransaccionEntrada");
            containerRegistry.RegisterForNavigation<TransferenciaSalida, TransferenciaSalidaViewModel>("TransaccionSalida");
            containerRegistry.RegisterForNavigation<ReporteIvaCV, ReporteIvaCVViewModel>("Iva");
            containerRegistry.RegisterForNavigation<DepositoEntrada, DepositoEntradaViewModel>("DepositoEntrada");
            containerRegistry.RegisterForNavigation<BancoMenu, BancoMenuViewModel>("BancoMenu");
            containerRegistry.RegisterForNavigation<ListadoComprobantesCompra, ListadoComprobantesCompraViewModel>("ListadoCompra");
            containerRegistry.RegisterForNavigation<Jornal, JornalViewModel>("Jornal");
            containerRegistry.RegisterForNavigation<JornalDetalle, JornalDetalleViewModel>("JornalDetalle");
            containerRegistry.RegisterForNavigation<ObraPlanificacion, ObraPlanificacionViewModel>("Planificacion");
            containerRegistry.RegisterForNavigation<ObraPlanificacionDetalle, ObraPlanificacionDetalleViewModel>("ObraPlanificacionDetalle");
            containerRegistry.RegisterForNavigation<EmpleadoObra, EmpleadoObraViewModel>("EmpleadoObra");
            containerRegistry.RegisterForNavigation<ReporteTesoreria, ReporteTesoreriaViewModel>("Reporte");
            containerRegistry.RegisterForNavigation<Banco, BancoViewModel>("BancoGeneral");
            containerRegistry.RegisterForNavigation<CobrarCheque, CobrarChequeViewModel>("CobrarCheque");
            containerRegistry.RegisterForNavigation<Cheque, ChequeViewModel>("ChequeSalida");
            containerRegistry.RegisterForNavigation<ChequeEntrada, ChequeEntradaViewModel>("ChequeEntrada");
            containerRegistry.RegisterForNavigation<ComprobanteEntrada, ComprobanteEntradaViewModel>("ComprobanteEntrada");
            containerRegistry.RegisterForNavigation<ComprobanteSalida, ComprobanteSalidaViewModel>("ComprobanteSalida");
            containerRegistry.RegisterForNavigation<CuentaCorrienteEntrada, CuentaCorrienteEntradaViewModel>("CuentaCorrienteEntrada");

        }

    }
}
