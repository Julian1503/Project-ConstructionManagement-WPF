using GestionObraWPF.ViewModels;
using System;
using System.Windows.Controls;

namespace GestionObraWPF.Views.ViewControls.Obra.Planificacion
{
    /// <summary>
    /// Interaction logic for PrismUserControl1
    /// </summary>
    public partial class ObraPlanificacion : UserControl
    {
        public ObraPlanificacion()
        {
            InitializeComponent();
        }
        protected async override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
           await ((ObraPlanificacionViewModel)this.DataContext).Inicialize();
        }
    }
}
