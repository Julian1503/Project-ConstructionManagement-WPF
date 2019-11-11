using GestionObraWPF.ViewModels;
using System;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;

namespace GestionObraWPF.Views.ViewControls.Caja
{
    /// <summary>
    /// Interaction logic for Caja
    /// </summary>
    public partial class Caja : UserControl
    {
        public Caja()
        {
            InitializeComponent();
        }

        protected async override void OnInitialized(EventArgs e)
        {
             await ((CajaViewModel)this.DataContext).Inicializar();
            base.OnInitialized(e);
        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
