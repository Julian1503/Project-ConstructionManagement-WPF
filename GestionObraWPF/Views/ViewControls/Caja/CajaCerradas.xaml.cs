using GestionObraWPF.Helpers;
using System.Windows.Controls;

namespace GestionObraWPF.Views.ViewControls.Caja
{
    /// <summary>
    /// Interaction logic for CajaCerradas
    /// </summary>
    public partial class CajaCerradas : UserControl
    {
        public CajaCerradas()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Excel.ExportToExcelAndCsv(grilla, "Cajas");
        }
    }
}
