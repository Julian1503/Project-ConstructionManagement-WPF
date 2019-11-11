using GestionObraWPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GestionObraWPF.Views.ViewControls
{
    /// <summary>
    /// Lógica de interacción para InicioView.xaml
    /// </summary>
    public partial class ObraABM : UserControl
    {
        public ObraABM()
        {
            InitializeComponent();
        }
        protected async override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            await ((ObraABMViewModel)this.DataContext).Inicializar();
        }
    }
}
