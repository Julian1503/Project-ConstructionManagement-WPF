using GestionObraWPF.ViewModels;
using System;
using System.Windows.Controls;

namespace GestionObraWPF.Views.ViewControls.ABMs
{
    /// <summary>
    /// Interaction logic for Usuario
    /// </summary>
    public partial class Usuario : UserControl
    {
        public Usuario()
        {
            InitializeComponent();
        }
        protected async override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            await ((UsuarioViewModel)this.DataContext).Inicializar();
        }
    }
}
