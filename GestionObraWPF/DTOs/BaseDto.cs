using Exportable.Attribute;
using Prism.Mvvm;
using System.Collections.Generic;
using System.ComponentModel;

namespace GestionObraWPF.DTOs
{
    public class BaseDto : BindableBase
    {
        [Exportable(IsIgnored =true)]
        public long Id { get; set; }

        [Exportable(IsIgnored = true)]
        public bool EstaEliminado { get; set; }
    }
}