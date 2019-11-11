using GestionObraWPF.DTOs;
using GestionObraWPF.ViewModels;
using Prism.Events;
using System;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;

namespace GestionObraWPF.Views.ViewControls.Obra.Planificacion
{
    /// <summary>
    /// Interaction logic for ObraPlanificacionDetalle
    /// </summary>
    public partial class ObraPlanificacionDetalle : UserControl
    {
        public ObraPlanificacionDetalle()
        {
            InitializeComponent();
        }

       
        protected async override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            await ((ObraPlanificacionDetalleViewModel)this.DataContext).Inicializar();
        }


        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            NumberValidationTextBox(sender, e);
            if (((TextBox)sender).Text.Length == 2)
            {
                ((TextBox)sender).Text += ":";
                ((TextBox)sender).Select(((TextBox)sender).Text.Length, 0);
            }
        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (((TextBox)sender).Text.Length < 8)
            {
                ((TextBox)sender).Text += e.Key;
            }
        }

    }
}
