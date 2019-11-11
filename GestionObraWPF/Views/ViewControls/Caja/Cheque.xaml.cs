using GestionObraWPF.ViewModels.Caja;
using System;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;

namespace GestionObraWPF.Views.ViewControls.Caja
{
    /// <summary>
    /// Interaction logic for Cheque
    /// </summary>
    public partial class Cheque : UserControl
    {
        public Cheque()
        {
            InitializeComponent();
        }

        protected  async override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
           await ((ChequeViewModel)this.DataContext).Inicializar();
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void TextBox_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            if (((TextBox)sender).Text == "0")
            {
                ((TextBox)sender).Text = "";
            }
        }

        private void TextBox_LostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            if (((TextBox)sender).Text == "")
            {
                ((TextBox)sender).Text = "0";
            }
        }
    }
}
