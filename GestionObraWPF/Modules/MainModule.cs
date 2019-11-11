using GestionObraWPF.Views.ViewControls;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionObraWPF.Modules
{
    class MainModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var region = containerProvider.Resolve<IRegionManager>();
            region.RegisterViewWithRegion("Inicio", typeof(ObraABM));

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {

        }
    }
}
