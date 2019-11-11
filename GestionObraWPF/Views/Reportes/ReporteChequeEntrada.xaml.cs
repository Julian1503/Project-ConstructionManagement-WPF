using GestionObraWPF.Helpers;
using GestionObraWPF.ViewModels;
using System;
using System.Windows.Controls;

namespace GestionObraWPF.Views.Reportes
{
    /// <summary>
    /// Interaction logic for ChequeEntrada
    /// </summary>
    public partial class ReporteChequeEntrada : UserControl
    {
        public ReporteChequeEntrada()
        {
            InitializeComponent();
        }
        protected async override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            await ((ReporteChequeEntradaViewModel)this.DataContext).Inicializar();
        }
        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Excel.ExportToExcelAndCsv(grilla, "ChequesEntrada");
        }
    }
}
