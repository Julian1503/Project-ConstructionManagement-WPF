using GestionObraWPF.ViewModels;
using System;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;

namespace GestionObraWPF.Views.ViewControls.Caja
{
    /// <summary>
    /// Interaction logic for CobrarCheque
    /// </summary>
    public partial class CobrarCheque : UserControl
    {
        public CobrarCheque()
        {
            InitializeComponent();
        }
        protected async override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
           await ((CobrarChequeViewModel)this.DataContext).Initialize();
        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
