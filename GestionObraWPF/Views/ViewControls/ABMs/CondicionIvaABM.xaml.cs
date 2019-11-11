using GestionObraWPF.ViewModels;
using System;
using System.Windows.Controls;

namespace GestionObraWPF.Views.ViewControls.ABMs
{
    /// <summary>
    /// Interaction logic for CondicionIvaABM
    /// </summary>
    public partial class CondicionIvaABM : UserControl
    {
        public CondicionIvaABM()
        {
            InitializeComponent();
        }
        protected async override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            await ((CondicionIvaABMViewModel)this.DataContext).Inicializar();
        }
    }
}
