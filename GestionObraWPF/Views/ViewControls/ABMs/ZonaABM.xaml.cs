using GestionObraWPF.ViewModels;
using System;
using System.Windows.Controls;

namespace GestionObraWPF.Views.ViewControls.ABMs
{
    /// <summary>
    /// Interaction logic for ZonaABM
    /// </summary>
    public partial class ZonaABM : UserControl
    {
        public ZonaABM()
        {
            InitializeComponent();
        }
        protected async override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            await ((ZonaABMViewModel)this.DataContext).Inicializar();
        }
    }
}
