using Photogrammetry.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using System;

namespace Photogrammetry.Infrastructure.Modules
{
    public class MainModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var region = containerProvider.Resolve<IRegionManager>();

            region.RegisterViewWithRegion("ContentRegion", typeof(Toolbar));
            
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<Toolbar>();
            containerRegistry.RegisterForNavigation<FirstTaskPage>();
            containerRegistry.RegisterForNavigation<SecondTaskPage>();
            containerRegistry.RegisterForNavigation<ThirdTaskPage>();
            containerRegistry.RegisterForNavigation<FourthTaskPage>();
        }
    }
}
