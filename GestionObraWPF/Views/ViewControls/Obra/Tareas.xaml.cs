using GestionObraWPF.Helpers;
using System.Windows.Controls;

namespace GestionObraWPF.Views.ViewControls.Obra
{
    /// <summary>
    /// Interaction logic for Tareas
    /// </summary>
    public partial class Tareas : UserControl
    {
        public Tareas()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Excel.ExportToExcelAndCsv(datagrid,"Tareas");
        }
    }
}
