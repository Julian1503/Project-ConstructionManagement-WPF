using GestionObraWPF.ViewModels;
using System;
using System.Windows.Controls;

namespace GestionObraWPF.Views.ViewControls.ABMs
{
    /// <summary>
    /// Interaction logic for ProveedorABM
    /// </summary>
    public partial class ProveedorABM : UserControl
    {
        public ProveedorABM()
        {
            InitializeComponent();
        }
        protected async override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            await ((ProveedorABMViewModel)this.DataContext).Inicializar();
        }
    }
}
