using GestionObraWPF.ViewModels;
using System;
using System.Windows.Controls;

namespace GestionObraWPF.Views.ViewControls.ABMs
{
    /// <summary>
    /// Interaction logic for PersonaABM
    /// </summary>
    public partial class PersonaABM : UserControl
    {
        public PersonaABM()
        {
            InitializeComponent();
        }
        protected async override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            await ((PersonaABMViewModel)this.DataContext).Inicializar();
        }
    }
}
