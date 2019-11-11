using GestionObraWPF.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionObraWPF.Model
{
    public class UsuarioDto : BasicModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
        public IdentificacionDto Identificacion { get; set; }
        public long IdentificacionId { get; set; }
        public long EmpleadoId { get; set; }
        public EmpleadoDto Empleado { get; set; }
        public bool EstaBloqueado { get; set; }
    }
}
