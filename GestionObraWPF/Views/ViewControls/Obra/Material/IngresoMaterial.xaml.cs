using GestionObraWPF.Helpers;
using System.Windows.Controls;

namespace GestionObraWPF.Views.ViewControls.Obra.Material
{
    /// <summary>
    /// Interaction logic for IngresoMaterial
    /// </summary>
    public partial class IngresoMaterial : UserControl
    {
        public IngresoMaterial()
        {
            InitializeComponent();
        }
        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Excel.ExportToExcelAndCsv(datagrid, "InsumosUsados");
        }
    }
}
