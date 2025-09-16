using System.Windows;
using Ves.UI.Bootstrap;
using Ves.UI.Models;
using Ves.UI.Services;

namespace Ves.UI.Views;

public partial class LoginWindow : Window
{
    private readonly AppEnvironment _environment;
    private readonly IAuthService _authService;

    public LoginWindow(AppEnvironment environment)
    {
        InitializeComponent();
        _environment = environment;
        _authService = new FakeAuthService();
    }

    private void OnLoginClicked(object sender, RoutedEventArgs e)
    {
        ErrorText.Visibility = Visibility.Collapsed;
        var username = UsernameTextBox.Text.Trim();
        var password = PasswordBox.Password;

        if (!_authService.TryAuthenticate(username, password, out var account, out var error))
        {
            ErrorText.Text = error;
            ErrorText.Visibility = Visibility.Visible;
            PasswordBox.SelectAll();
            PasswordBox.Focus();
            return;
        }

        var mainWindow = new MainWindow(_environment, account!, _authService)
        {
            Owner = this
        };

        mainWindow.Closed += (_, _) => Close();
        Hide();
        mainWindow.Show();
    }

    private void OnRecoverPasswordClicked(object sender, RoutedEventArgs e)
    {
        var recoveryWindow = new PasswordRecoveryWindow(_authService)
        {
            Owner = this
        };

        recoveryWindow.ShowDialog();
    }
}
