using GestionObraWPF.Helpers;
using GestionObraWPF.ViewModels;
using System;
using System.Windows.Controls;

namespace GestionObraWPF.Views.ViewControls.Contratista
{
    /// <summary>
    /// Interaction logic for ListadoAsistenciaContratista
    /// </summary>
    public partial class ListadoAsistenciaContratista : UserControl
    {
        public ListadoAsistenciaContratista()
        {
            InitializeComponent();
        }
        protected async override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            await ((ListadoAsistenciaContratistaViewModel)this.DataContext).Inicializar();
        }
        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Excel.ExportToExcelAndCsv(grid, "ComprobantesSalida");
        }
    }
}
