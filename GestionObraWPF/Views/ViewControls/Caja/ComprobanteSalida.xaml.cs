using GestionObraWPF.ViewModels;
using System;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;

namespace GestionObraWPF.Views.ViewControls.Caja
{
    /// <summary>
    /// Interaction logic for ComprobanteSalida
    /// </summary>
    public partial class ComprobanteSalida : UserControl
    {
        public ComprobanteSalida()
        {
            InitializeComponent();
        }
        protected async override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            await((ComprobanteSalidaViewModel)this.DataContext).Inicializar();

        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
