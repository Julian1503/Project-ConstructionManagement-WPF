using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionObraWPF.Views.ViewControls.Model
{
    public class CuadroInicio
    {
        public string Titulo { get; set; }
        public string Cantidad { get; set; }
        public string Unidad { get; set; }
        public string Color { get; set; } = "LightBlue";
        public string MensajeAdvertencia { get; set; }
        public PackIconKind Icono { get; set; }   
        public PackIconKind IconoAdvertencia { get; set; }
    }
}
