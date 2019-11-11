using GestionObraWPF.ViewModels;
using System;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace GestionObraWPF.Views.ViewControls.Compra
{
    /// <summary>
    /// Interaction logic for CompraInsumos
    /// </summary>
    public partial class CompraInsumos : UserControl
    {
        public CompraInsumos()
        {
            InitializeComponent();
            cuit.Visibility = System.Windows.Visibility.Collapsed;
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            ((CompraInsumosViewModel)this.DataContext).Inicialize();
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        private void TextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if(e.Key == System.Windows.Input.Key.Enter)
            {
                btnAgregar.Focus();
            }
        }

        private void ToggleButton_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            if (cuit != null)
            {
                if (((ToggleButton)sender).IsChecked is bool)
                {
                    cuit.Visibility = System.Windows.Visibility.Collapsed;
                    cuit.Text = "";
                }
            }
        }

        private void ToggleButton_Unchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            if (cuit != null)
            {
                if (((ToggleButton)sender).IsChecked is bool)
                {
                    cuit.Visibility = System.Windows.Visibility.Visible;
                }
            }
           
        }

        private void BtnAgregar_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }
    }
}
