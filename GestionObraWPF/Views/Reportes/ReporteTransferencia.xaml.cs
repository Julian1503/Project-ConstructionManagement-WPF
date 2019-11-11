using GestionObraWPF.Helpers;
using GestionObraWPF.ViewModels;
using System;
using System.Windows.Controls;

namespace GestionObraWPF.Views.Reportes
{
    /// <summary>
    /// Interaction logic for ReporteTransferencia
    /// </summary>
    public partial class ReporteTransferencia : UserControl
    {
        public ReporteTransferencia()
        {
            InitializeComponent();
        }
        protected async override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            await ((ReporteTransferenciaViewModel)this.DataContext).Inicializar();
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Excel.ExportToExcelAndCsv(grilla, "Transferencias");
        }
    }
}
