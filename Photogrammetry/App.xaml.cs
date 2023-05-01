using Photogrammetry.Infrastructure.Modules;
using Photogrammetry.Views;
using Prism.Ioc;
using Prism.Modularity;
using System.Windows;

namespace Photogrammetry
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {

        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            base.ConfigureModuleCatalog(moduleCatalog);

            moduleCatalog.AddModule<MainModule>();
            moduleCatalog.AddModule<MenuModule>();
        }
    }
}
