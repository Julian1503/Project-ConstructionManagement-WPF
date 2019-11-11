using GestionObraWPF.ViewModels;
using System;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;

namespace GestionObraWPF.Views.ViewControls.Compra
{
    /// <summary>
    /// Interaction logic for ListadoComprobantesCompra
    /// </summary>
    public partial class ListadoComprobantesCompra : UserControl
    {
        public ListadoComprobantesCompra()
        {
            InitializeComponent();
        }
        protected async override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            await ((ListadoComprobantesCompraViewModel)this.DataContext).Inicializar();
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
