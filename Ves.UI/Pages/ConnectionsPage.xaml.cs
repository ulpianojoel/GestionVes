using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Ves.BLL.Services;
using Ves.DAL.Data;
using Ves.UI.Bootstrap;

namespace Ves.UI.Pages;

public partial class ConnectionsPage : Page
{
    public ConnectionsPage(AppEnvironment environment)
    {
        InitializeComponent();
        DiagnosticsText.Text = environment.Diagnostics.BuildReport();

        var connections = environment.Registry.RegisteredNames
            .Select(name => TryBuildConnection(environment.Registry, name))
            .Where(display => display is not null)
            .Select(display => display!)
            .ToList();

        ConnectionList.ItemsSource = connections;
    }

    private static ConnectionDisplay? TryBuildConnection(ConnectionFactoryRegistry registry, string name)
    {
        if (registry.TryGetFactory(name, out var factory) && factory is not null)
        {
            return new ConnectionDisplay(factory.Name, factory.ConnectionString);
        }

        return null;
    }

    private void OnCopyClicked(object sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.DataContext is ConnectionDisplay display)
        {
            Clipboard.SetText(display.ConnectionString);
            MessageBox.Show($"Se copió la cadena '{display.Name}'.", "Conexiones", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }

    private sealed record ConnectionDisplay(string Name, string ConnectionString);
}
