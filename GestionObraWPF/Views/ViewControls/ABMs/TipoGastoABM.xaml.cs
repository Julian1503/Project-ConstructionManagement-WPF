using GestionObraWPF.ViewModels;
using System;
using System.Windows.Controls;

namespace GestionObraWPF.Views.ViewControls.ABMs
{
    /// <summary>
    /// Interaction logic for TipoGastoABM
    /// </summary>
    public partial class TipoGastoABM : UserControl
    {
        public TipoGastoABM()
        {
            InitializeComponent();
        }
        protected async override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            await ((TipoGastoABMViewModel)this.DataContext).Inicializar();
        }
    }
}
