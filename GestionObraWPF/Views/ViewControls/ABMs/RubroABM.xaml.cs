using GestionObraWPF.ViewModels;
using System;
using System.Windows.Controls;

namespace GestionObraWPF.Views.ViewControls.ABMs
{
    /// <summary>
    /// Interaction logic for RubroABM
    /// </summary>
    public partial class RubroABM : UserControl
    {
        public RubroABM()
        {
            InitializeComponent();
        }
        protected async override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            await ((RubroABMViewModel)this.DataContext).Inicializar();
        }
    }
}
