using GestionObraWPF.Helpers;
using GestionObraWPF.ViewModels;
using System;
using System.Windows.Controls;

namespace GestionObraWPF.Views.Reportes
{
    /// <summary>
    /// Interaction logic for ReporteMaterialesUsados
    /// </summary>
    public partial class ReporteMaterialesUsados : UserControl
    {
        public ReporteMaterialesUsados()
        {
            InitializeComponent();
        }
        protected async override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
           await ((ReporteMaterialesUsadosViewModel)this.DataContext).Inicializar();
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Excel.ExportToExcelAndCsv(grilla);
        }
    }
}
