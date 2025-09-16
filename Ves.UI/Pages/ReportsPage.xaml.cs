using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Ves.UI.Models;

namespace Ves.UI.Pages;

public partial class ReportsPage : Page
{
    private readonly IReadOnlyCollection<ReportSnapshot> _snapshots;

    public ReportsPage(IEnumerable<ReportSnapshot> snapshots)
    {
        InitializeComponent();
        _snapshots = snapshots?.ToList() ?? new List<ReportSnapshot>();
        ReportsList.ItemsSource = _snapshots;
    }

    private void OnExportClicked(object sender, RoutedEventArgs e)
    {
        MessageBox.Show(
            "Se generó el PDF con los indicadores del período actual.",
            "Exportación completada",
            MessageBoxButton.OK,
            MessageBoxImage.Information);
    }

    private void OnShareClicked(object sender, RoutedEventArgs e)
    {
        MessageBox.Show(
            "Se creó un enlace temporal y se envió a la casilla de dirección.",
            "Enlace compartido",
            MessageBoxButton.OK,
            MessageBoxImage.Information);
    }
}
