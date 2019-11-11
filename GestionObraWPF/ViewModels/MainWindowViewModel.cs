using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using GestionObraWPF.DTOs;
using GestionObraWPF.Model;
using GestionObraWPF.Servicios;
using GestionObraWPF.Views.ViewControls;
using GestionObraWPF.Views.ViewControls.Model;
using MaterialDesignThemes.Wpf;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace GestionObraWPF.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "Prism Application";

        List<SubItem> AdministracionYFinanzas ;
        List<SubItem> Configuracion ;
        List<SubItem> Tesoreria ;
        List<SubItem> GestionOperativa ;
        private ObservableCollection<ItemMenu> _itemMenu;

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
        private List<SubItem> _usuarios;
        public List<SubItem> Usuarios
        {
            get { return _usuarios; }
            set { SetProperty(ref _usuarios, value); }
        }
        public IRegionManager regionManager { get; set; }
        public ICommand Navigation { get; set; }
        public ICommand InicioCommand { get; set; }
        public ObservableCollection<ItemMenu> ItemMenu { get { return _itemMenu; } set { SetProperty(ref _itemMenu, value); } }
        public IdentificacionDto Identificacion { get; private set; }

        public MainWindowViewModel(IRegionManager regionManager)
        {
            AdministracionYFinanzas = new List<SubItem>();
            Configuracion = new List<SubItem>();
            Tesoreria = new List<SubItem>();
            GestionOperativa = new List<SubItem>();
            Usuarios = new List<SubItem>();
            InicioCommand = new DelegateCommand(Inicio);
            this.regionManager = regionManager;
            Tesoreria.Add(new SubItem("Banco", regionManager, "BancoGeneral"));
            Tesoreria.Add(new SubItem("Caja", regionManager, "CajaMenu")); 
            Tesoreria.Add(new SubItem("Listado compras", regionManager, "ListadoCompra"));
            Tesoreria.Add(new SubItem("Listado facturacion", regionManager, "Venta"));
            Tesoreria.Add(new SubItem("Reportes", regionManager, "Reporte"));
            GestionOperativa.Add(new SubItem("Creacion de obra", regionManager, "Inicio"));
            GestionOperativa.Add(new SubItem("Presupuesto", regionManager, "Presupuesto"));
            GestionOperativa.Add(new SubItem("Planificacion", regionManager, "Planificacion"));
            GestionOperativa.Add(new SubItem("Avances", regionManager, "ObraInicio"));
            GestionOperativa.Add(new SubItem("Materiales usados", regionManager, "MaterialesUsados"));
            GestionOperativa.Add(new SubItem("Reporte", regionManager, "ReporteObra"));
            AdministracionYFinanzas.Add(new SubItem("Clientes", regionManager, "Empresa"));
            AdministracionYFinanzas.Add(new SubItem("Contratistas", regionManager, "PantallaContratista"));
            AdministracionYFinanzas.Add(new SubItem("Personal", regionManager, "PantallaEmpleado"));
            AdministracionYFinanzas.Add(new SubItem("Proveedores", regionManager, "ProveedorMenu"));
            AdministracionYFinanzas.Add(new SubItem("Presupuesto", regionManager, "Presupuesto"));
            AdministracionYFinanzas.Add(new SubItem("Ventas", regionManager, "VentasAdm"));
            AdministracionYFinanzas.Add(new SubItem("Reportes", regionManager, "ReporteAdm"));
            Configuracion.Add(new SubItem("Categorias", regionManager, "Categoria"));
            Configuracion.Add(new SubItem("Condicion Iva", regionManager, "CondicionIva"));
            Configuracion.Add(new SubItem("Material", regionManager, "Material"));
            Configuracion.Add(new SubItem("Nombre Tarea", regionManager, "DescripcionTarea"));
            Configuracion.Add(new SubItem("Rubro", regionManager, "Rubro"));
            Configuracion.Add(new SubItem("SubRubro", regionManager, "SubRubro"));
            Configuracion.Add(new SubItem("Tipo de Gasto", regionManager, "TipoGasto"));
            Configuracion.Add(new SubItem("Stock", regionManager, "Stock"));
            Configuracion.Add(new SubItem("Zona", regionManager, "Zona"));
            Usuarios.Add(new SubItem("Usuario", regionManager, "Usuario"));
        }

        private void Inicio()
        {
            regionManager.RequestNavigate("Contenido", "PantallaPrincipal");
        }


        public void InicializarBotones()
        {
            ItemMenu = new ObservableCollection<ItemMenu>()
            {
                new ItemMenu("Tesorería",Tesoreria,PackIconKind.ClipboardPulseOutline,UsuarioGral.Identificacion.Tesoreria),
                new ItemMenu("Gestión Operativa",GestionOperativa,PackIconKind.Highway,UsuarioGral.Identificacion.Obra),
                new ItemMenu("Administracion y Finanzas", AdministracionYFinanzas, PackIconKind.FileDocumentBoxMultipleOutline,UsuarioGral.Identificacion.Administracion),
                new ItemMenu("Configuracion", Configuracion, PackIconKind.Nut,UsuarioGral.Identificacion.Configuracion),
                new ItemMenu("Adm. de Usuarios", Usuarios, PackIconKind.Person,UsuarioGral.Identificacion.Usuarios),
            };
        }

    }
}
