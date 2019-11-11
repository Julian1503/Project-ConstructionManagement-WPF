using GestionObraWPF.ViewModels;
using System;
using System.Windows.Controls;

namespace GestionObraWPF.Views
{
    /// <summary>
    /// Interaction logic for EmpresaABM
    /// </summary>
    public partial class EmpresaABM : UserControl
    {
        public EmpresaABM()
        {
            InitializeComponent();
        }
        protected async override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            await ((EmpresaABMViewModel)this.DataContext).Inicializar();
        }
    }
}
