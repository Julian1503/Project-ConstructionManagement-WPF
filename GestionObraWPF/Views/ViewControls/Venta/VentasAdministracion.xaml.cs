using GestionObraWPF.ViewModels;
using System;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;

namespace GestionObraWPF.Views.ViewControls.Venta
{
    /// <summary>
    /// Interaction logic for VentasAdministracion
    /// </summary>
    public partial class VentasAdministracion : UserControl
    {
        public VentasAdministracion()
        {
            InitializeComponent();
        }
        protected async override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            await ((VentasAdministracionViewModel)this.DataContext).Inicializar();
        }
       
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
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
