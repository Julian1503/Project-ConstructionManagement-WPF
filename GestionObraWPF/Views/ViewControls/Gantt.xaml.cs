using GestionObraWPF.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace GestionObraWPF.Views.ViewControls
{
    /// <summary>
    /// Interaction logic for Gantt
    /// </summary>
    public partial class Gantt : UserControl
    {
        public Gantt()
        {
            InitializeComponent();
            Axis.MaxRange = 100;
        }

        private async  void UserControl_IsVisibleChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            if (this.Visibility == System.Windows.Visibility.Visible)
            {
                await((GanttViewModel)this.DataContext).Inicializar();
            }
        }
    }
}
