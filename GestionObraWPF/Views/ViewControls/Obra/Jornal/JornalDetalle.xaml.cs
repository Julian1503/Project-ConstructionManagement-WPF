using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;

namespace GestionObraWPF.Views.ViewControls.Obra.Jornal
{
    /// <summary>
    /// Interaction logic for JornalDetalle
    /// </summary>
    public partial class JornalDetalle : UserControl
    {
        public JornalDetalle()
        {
            InitializeComponent();
        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            NumberValidationTextBox(sender, e);
            if (((TextBox)sender).Text.Length == 2)
            {
                ((TextBox)sender).Text += ":";
                ((TextBox)sender).Select(((TextBox)sender).Text.Length, 0);
            }
        }
    }
}
