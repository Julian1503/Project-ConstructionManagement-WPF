using GestionObraWPF.ViewModels;
using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace GestionObraWPF.Views.ViewControls.ABMs
{
    /// <summary>
    /// Interaction logic for BancoABM
    /// </summary>
    public partial class BancoABM : UserControl
    {
        public BancoABM()
        {
            InitializeComponent();
        }
        protected async override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            await ((BancoABMViewModel)this.DataContext).Inicializar();
        }
    }
}
