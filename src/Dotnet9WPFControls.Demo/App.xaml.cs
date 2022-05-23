using Dotnet9WPFControls.Demo.Views;
using Prism.DryIoc;
using Prism.Ioc;
using System.Windows;

namespace Dotnet9WPFControls.Demo
{
    public partial class App : PrismApplication
    {
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
        }

        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindowView>();
        }
    }
}