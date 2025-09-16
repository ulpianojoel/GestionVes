using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Ves.BLL.Services;
using Ves.DAL.Data;
using Ves.UI.Bootstrap;

namespace Ves.UI.Pages
{
    public partial class ConnectionsPage : Page
    {
        public ConnectionsPage(AppEnvironment environment)
        {
            InitializeComponent();
            DiagnosticsText.Text = environment.Diagnostics.BuildReport();

            var connections = new List<ConnectionDisplay>();
            foreach (var name in environment.Registry.RegisteredNames)
            {
                ConnectionDisplay display;
                if (TryBuildConnection(environment.Registry, name, out display))
                {
                    connections.Add(display);
                }
            }

            ConnectionList.ItemsSource = connections;
        }

        private static bool TryBuildConnection(ConnectionFactoryRegistry registry, string name, out ConnectionDisplay display)
        {
            ISqlConnectionFactory factory;
            if (registry.TryGetFactory(name, out factory) && factory != null)
            {
                display = new ConnectionDisplay(factory.Name, factory.ConnectionString);
                return true;
            }

            display = null;
            return false;
        }

        private void OnCopyClicked(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var display = button != null ? button.DataContext as ConnectionDisplay : null;
            if (display != null)
            {
                Clipboard.SetText(display.ConnectionString);
                MessageBox.Show("Se copió la cadena '" + display.Name + "'.", "Conexiones", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private sealed class ConnectionDisplay
        {
            public ConnectionDisplay(string name, string connectionString)
            {
                Name = name;
                ConnectionString = connectionString;
            }

            public string Name { get; private set; }

            public string ConnectionString { get; private set; }
        }
    }
}
