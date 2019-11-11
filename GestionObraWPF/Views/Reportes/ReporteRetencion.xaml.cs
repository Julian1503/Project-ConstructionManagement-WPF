using GestionObraWPF.Helpers;
using GestionObraWPF.ViewModels;
using System;
using System.Windows.Controls;

namespace GestionObraWPF.Views.Reportes
{
    /// <summary>
    /// Interaction logic for ReporteRetencion
    /// </summary>
    public partial class ReporteRetencion : UserControl
    {
        public ReporteRetencion()
        {
            InitializeComponent();
        }
        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Excel.ExportToExcelAndCsv(grilla);
        }
        protected async override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            await ((ReporteRetencionViewModel)this.DataContext).Inicializar();
        }
    }
}
