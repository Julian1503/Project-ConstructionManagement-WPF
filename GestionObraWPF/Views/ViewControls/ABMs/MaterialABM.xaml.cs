using GestionObraWPF.ViewModels;
using System;
using System.Windows.Controls;

namespace GestionObraWPF.Views.ViewControls.ABMs
{
    /// <summary>
    /// Interaction logic for MaterialABM
    /// </summary>
    public partial class MaterialABM : UserControl
    {
        public MaterialABM()
        {
            InitializeComponent();
        }
        protected async override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            await ((MaterialABMViewModel)this.DataContext).Inicializar();
        }
    }
}
