using System.Windows.Controls;

namespace GestionObraWPF.Views.ViewControls.Banco
{
    /// <summary>
    /// Interaction logic for CuentaCorrienteEntrada
    /// </summary>
    public partial class CuentaCorrienteEntrada : UserControl
    {
        public CuentaCorrienteEntrada()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Transitioner.SelectedIndex = 1;
        }

        private void Button_Click_1(object sender, System.Windows.RoutedEventArgs e)
        {
            Transitioner.SelectedIndex = 2;
        }

        private void Button_Click_2(object sender, System.Windows.RoutedEventArgs e)
        {
            Transitioner.SelectedIndex = 0;
        }
    }
}
