using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionObraWPF.ViewModels.EventAgreggator
{
     public class PopUp
    {
        public string ButtonTitle { get; set; }
        public bool ControlersEnable { get; set; }
        public bool ShowPopUp { get; set; }
        public PopUp(string buttonTitle, bool showPopUp, bool controlersEnable)
        {
            ButtonTitle = buttonTitle;
            ControlersEnable = controlersEnable;
            ShowPopUp = showPopUp;
        }
    }
}
