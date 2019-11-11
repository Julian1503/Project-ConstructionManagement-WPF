using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionObraWPF.DTOs
{
   public  class IdentificacionDto : BindableBase
    {
        private bool _tesoreria;
        private bool _obra;
        private bool _configuracion;
        private bool _administracion;

        public bool Tesoreria
        {
            get { return _tesoreria; }
            set { SetProperty(ref _tesoreria, value); }
        }
      
        public bool Obra
        {
            get { return _obra; }
            set { SetProperty(ref _obra, value); }
        }

        public bool Configuracion {
            get { return _configuracion; }
            set { SetProperty(ref _configuracion, value); }
        }

        public bool Administracion {
            get { return _administracion; }
            set { SetProperty(ref _administracion, value); }
        }

        private bool _usuarios;
        public bool Usuarios
        {
            get { return _usuarios; }
            set { SetProperty(ref _usuarios, value); }
        }
    }
}
