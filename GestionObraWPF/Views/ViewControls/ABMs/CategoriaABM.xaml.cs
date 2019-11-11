using GestionObraWPF.ViewModels;
using System;
using System.Windows.Controls;

namespace GestionObraWPF.Views.ViewControls.ABMs
{
    /// <summary>
    /// Interaction logic for Categoria
    /// </summary>
    public partial class CategoriaABM : UserControl
    {
        public CategoriaABM()
        {
            InitializeComponent();
        }
        protected async override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            await((CategoriaABMViewModel)this.DataContext).Inicializar();
        }
    }
}
