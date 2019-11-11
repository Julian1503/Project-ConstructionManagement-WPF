using Prism.Commands;
using Prism.Regions;
using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace GestionObraWPF.Views.ViewControls.Model
{
    public class SubItem
    {
        public SubItem(string name, IRegionManager regionManager, string pantalla)
        {
            Name = name;
            Command = new DelegateCommand(navigate);
            this.regionManager = regionManager;
            Pantalla = pantalla;
        }
        public SubItem(string name, IRegionManager regionManager, string pantalla,ICommand command)
        {
            Name = name;
            Command = command;
            this.regionManager = regionManager;
            Pantalla = pantalla;
        }
        private void navigate()
        {
            regionManager.RequestNavigate("Contenido", Pantalla);
        }

        public string Name { get; private set; }
        public ICommand Command { get; private set; }
        public string Pantalla { get; private set; }

        private IRegionManager regionManager;
    }
}