using System.Windows.Controls;

namespace GestionObraWPF.Views.ViewControls.Banco
{
    /// <summary>
    /// Interaction logic for Banco
    /// </summary>
    public partial class Banco : UserControl
    {
        public Banco()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Transitioner.SelectedIndex = 1;
        }
    }
}
