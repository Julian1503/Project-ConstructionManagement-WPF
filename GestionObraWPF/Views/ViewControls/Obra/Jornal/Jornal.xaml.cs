using GestionObraWPF.Helpers;
using System.Windows.Controls;

namespace GestionObraWPF.Views.ViewControls.Obra.Jornal
{
    /// <summary>
    /// Interaction logic for Jornal
    /// </summary>
    public partial class Jornal : UserControl
    {
        public Jornal()
        {
            InitializeComponent();
        }
        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Excel.ExportToExcelAndCsv(grilla,"Jornal");
        }
    }
}
