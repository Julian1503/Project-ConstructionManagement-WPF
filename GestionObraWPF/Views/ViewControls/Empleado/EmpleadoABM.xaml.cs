using GestionObraWPF.ViewModels;
using System;
using System.Windows.Controls;

namespace GestionObraWPF.Views.ViewControls.ABMs
{
    /// <summary>
    /// Interaction logic for EmpleadoABM
    /// </summary>
    public partial class EmpleadoABM : UserControl
    {
        public EmpleadoABM()
        {
            InitializeComponent();
        }
        protected async override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            await ((EmpleadoABMViewModel)this.DataContext).Inicializar();
        }
    }
}
