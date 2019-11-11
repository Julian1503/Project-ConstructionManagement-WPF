using Prism.Services.Dialogs;
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
    /// Lógica de interacción para Mensaje.xaml
    /// </summary>
    public partial class Mensaje : Window
    {
        public Mensaje()
        {
            InitializeComponent();
        }
        public bool Acepto = false;
        public string Result = "";
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(texto.Text))
            {
                Result = texto.Text;
                Acepto = true;
                this.Close();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();

        }
    }
}
