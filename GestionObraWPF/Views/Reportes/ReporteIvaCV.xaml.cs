using GestionObraWPF.Helpers;
using GestionObraWPF.ViewModels;
using System;
using System.Windows.Controls;

namespace GestionObraWPF.Views.Reportes
{
    /// <summary>
    /// Interaction logic for ReporteIvaCV
    /// </summary>
    public partial class ReporteIvaCV : UserControl
    {
        public ReporteIvaCV()
        {
            InitializeComponent();
        }
        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Excel.ExportToExcelAndCsv(grilla, "PlanCuentas");
        }
        protected async override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            await ((ReporteIvaCVViewModel)this.DataContext).Inicializar();
        }
    }
}
