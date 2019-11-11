using GestionObraWPF.DTOs;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionObraWPF.Model
{
    public static class UsuarioGral 
    {
        public static IdentificacionDto Identificacion { get; set; } = new IdentificacionDto();
        public static long UsuarioId { get; set; }
    }
}
