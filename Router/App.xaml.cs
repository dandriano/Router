using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Router.ViewModels;
using System.Windows;

namespace Router
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            RegisterServices();
            var mainWindow = new MainWindow();

            mainWindow.Show();
        }

        private void RegisterServices()
        {
            Ioc.Default.ConfigureServices(
                new ServiceCollection()
                    .AddTransient<GraphViewModel>()
                    .BuildServiceProvider()
            );
        }
    }
}
