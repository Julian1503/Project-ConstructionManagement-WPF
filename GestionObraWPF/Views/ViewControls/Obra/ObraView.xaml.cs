using GestionObraWPF.ViewModels;
using System;
using System.Windows.Controls;

namespace GestionObraWPF.Views.ViewControls.Obra
{
    /// <summary>
    /// Interaction logic for ObraView
    /// </summary>
    public partial class ObraView : UserControl
    {
        public ObraView()
        {
            InitializeComponent();
        }
        protected async override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            await ((ObraViewViewModel)this.DataContext).Inicialize();
        }
    }
}
