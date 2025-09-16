using System.Windows;
using Ves.UI.Services;

namespace Ves.UI.Views;

public partial class PasswordRecoveryWindow : Window
{
    private readonly IAuthService _authService;

    public PasswordRecoveryWindow(IAuthService authService)
    {
        InitializeComponent();
        _authService = authService;
    }

    private void OnSendClicked(object sender, RoutedEventArgs e)
    {
        var identifier = IdentifierTextBox.Text.Trim();
        ResultText.Text = _authService.BuildRecoveryMessage(identifier);
        ResultText.Visibility = Visibility.Visible;
    }

    private void OnCancelClicked(object sender, RoutedEventArgs e)
    {
        Close();
    }
}
