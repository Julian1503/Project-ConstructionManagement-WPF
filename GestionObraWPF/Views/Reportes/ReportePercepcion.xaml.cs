using GestionObraWPF.Helpers;
using GestionObraWPF.ViewModels;
using System;
using System.Windows.Controls;

namespace GestionObraWPF.Views.Reportes
{
    /// <summary>
    /// Interaction logic for ReportePercepcion
    /// </summary>
    public partial class ReportePercepcion : UserControl
    {
        public ReportePercepcion()
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
            await ((ReportePercepcionViewModel)this.DataContext).Inicializar();
        }
    }
}
