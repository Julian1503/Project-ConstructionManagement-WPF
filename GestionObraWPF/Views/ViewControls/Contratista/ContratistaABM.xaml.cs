using GestionObraWPF.ViewModels;
using System;
using System.Windows.Controls;

namespace GestionObraWPF.Views.ViewControls.ABMs
{
    /// <summary>
    /// Interaction logic for ContratistaABM
    /// </summary>
    public partial class ContratistaABM : UserControl
    {
        public ContratistaABM()
        {
            InitializeComponent();
        }
        protected async override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            await ((ContratistaABMViewModel)this.DataContext).Inicializar();
        }
    }
}
