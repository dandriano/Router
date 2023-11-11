using Prism.DryIoc;
using Prism.Ioc;
using Prism.Mvvm;
using Router.ViewModels;
using Router.Views;
using System.Windows;


namespace Router
{
    public partial class App : PrismApplication
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void ConfigureViewModelLocator()
        {
            base.ConfigureViewModelLocator();

            ViewModelLocationProvider.Register<GraphView, GraphViewModel>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry) { }
    }
}
