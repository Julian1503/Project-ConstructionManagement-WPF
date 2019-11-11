using GestionObraWPF.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;

namespace GestionObraWPF.Views.ViewControls.Presupuesto
{
    /// <summary>
    /// Interaction logic for PresupuestoInicio
    /// </summary>
    public partial class PresupuestoInicio : UserControl
    {
        public PresupuestoInicio()
        {
            InitializeComponent();
        }
        protected async override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            await((PresupuestoInicioViewModel)this.DataContext).Inicializar();
        }

        private void DockPanel_MouseRightButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
                ContextMenu cm = this.FindResource("cmButton") as ContextMenu;
                cm.PlacementTarget = sender as Button;
                cm.IsOpen = true;
        }

        private void MenuItem_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            MessageBox.Show("");
        }
    }
}
