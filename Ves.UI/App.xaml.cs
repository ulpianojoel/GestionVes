using System.Windows;
using Ves.UI.Bootstrap;
using Ves.UI.Views;

namespace Ves.UI
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            AppEnvironment environment;
            if (!ConfigurationBootstrapper.TryInitialize(out environment))
            {
                Shutdown(-1);
                return;
            }

            var loginWindow = new LoginWindow(environment);
            MainWindow = loginWindow;
            loginWindow.Show();
        }
    }
}
