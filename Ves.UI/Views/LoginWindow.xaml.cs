using System.Windows;
using Ves.UI.Bootstrap;
using Ves.UI.Models;
using Ves.UI.Services;

namespace Ves.UI.Views
{
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
            string username = UsernameTextBox.Text.Trim();
            string password = PasswordBox.Password;

            UserAccount account;
            string error;
            if (!_authService.TryAuthenticate(username, password, out account, out error))
            {
                ErrorText.Text = string.IsNullOrEmpty(error) ? "No se pudo iniciar sesión." : error;
                ErrorText.Visibility = Visibility.Visible;
                PasswordBox.SelectAll();
                PasswordBox.Focus();
                return;
            }

            var mainWindow = new MainWindow(_environment, account, _authService);
            mainWindow.Owner = this;
            mainWindow.Closed += delegate { Close(); };

            Hide();
            mainWindow.Show();
        }

        private void OnRecoverPasswordClicked(object sender, RoutedEventArgs e)
        {
            var recoveryWindow = new PasswordRecoveryWindow(_authService);
            recoveryWindow.Owner = this;
            recoveryWindow.ShowDialog();
        }
    }
}
