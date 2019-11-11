using GestionObraWPF.ViewModels;
using System;
using System.Windows.Controls;

namespace GestionObraWPF.Views.ViewControls.ABMs
{
    /// <summary>
    /// Interaction logic for StockABM
    /// </summary>
    public partial class StockABM : UserControl
    {
        public StockABM()
        {
            InitializeComponent();
        }
        protected async override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            await ((StockABMViewModel)this.DataContext).Inicializar();
        }
    }
}
