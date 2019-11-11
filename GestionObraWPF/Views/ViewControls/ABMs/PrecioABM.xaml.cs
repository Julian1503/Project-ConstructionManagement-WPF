using GestionObraWPF.ViewModels;
using System;
using System.Windows.Controls;

namespace GestionObraWPF.Views.ViewControls.ABMs
{
    /// <summary>
    /// Interaction logic for PrecioABM
    /// </summary>
    public partial class PrecioABM : UserControl
    {
        public PrecioABM()
        {
            InitializeComponent();
        }
        protected async override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            await ((PrecioABMViewModel)this.DataContext).Inicializar();
        }
    }
}
