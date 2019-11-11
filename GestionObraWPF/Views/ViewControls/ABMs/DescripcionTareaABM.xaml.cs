using GestionObraWPF.ViewModels;
using System;
using System.Windows.Controls;

namespace GestionObraWPF.Views.ViewControls.ABMs
{
    /// <summary>
    /// Interaction logic for DescripcionTareaABM
    /// </summary>
    public partial class DescripcionTareaABM : UserControl
    {
        public DescripcionTareaABM()
        {
            InitializeComponent();
        }
        protected async override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            await ((DescripcionTareaDtoABMViewModel)this.DataContext).Inicializar();
        }
    }
}
