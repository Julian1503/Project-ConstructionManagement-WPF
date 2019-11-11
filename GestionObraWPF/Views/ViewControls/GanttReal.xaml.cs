using GestionObraWPF.ViewModels;
using System.Windows.Controls;

namespace GestionObraWPF.Views.ViewControls
{
    /// <summary>
    /// Interaction logic for GanttReal
    /// </summary>
    public partial class GanttReal : UserControl
    {
        public GanttReal()
        {
            InitializeComponent();
            Axis.MaxRange = 100;
        }

        private async void UserControl_IsVisibleChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            if (this.Visibility == System.Windows.Visibility.Visible)
            {
                await ((GanttRealViewModel)this.DataContext).Inicializar();
            }
        }
    }
}
