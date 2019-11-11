using GestionObraWPF.Helpers;
using GestionObraWPF.ViewModels;
using System;
using System.Windows.Controls;

namespace GestionObraWPF.Views.ViewControls.Banco
{
    /// <summary>
    /// Interaction logic for CuentaCorriente
    /// </summary>
    public partial class CuentaCorriente : UserControl
    {
        public CuentaCorriente()
        {
            InitializeComponent();
        }
        protected async override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            await ((CuentaCorrienteViewModel)this.DataContext).Inicializar();
        }
        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Excel.ExportToExcelAndCsv(datagrid,"CuentaCorriente");
        }
    }
}
