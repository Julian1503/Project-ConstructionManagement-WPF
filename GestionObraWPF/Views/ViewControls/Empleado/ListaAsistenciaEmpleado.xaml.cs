using GestionObraWPF.Helpers;
using GestionObraWPF.ViewModels;
using System;
using System.Windows.Controls;

namespace GestionObraWPF.Views.ViewControls.Empleado
{
    /// <summary>
    /// Interaction logic for ListaAsistenciaEmpleado
    /// </summary>
    public partial class ListaAsistenciaEmpleado : UserControl
    {
        public ListaAsistenciaEmpleado()
        {
            InitializeComponent();
        }

        protected async override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            await ((ListaAsistenciaEmpleadoViewModel)this.DataContext).Inicializar();
        }
        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Excel.ExportToExcelAndCsv(grid, "ComprobantesSalida");
        }
    }
}
