using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Ves.UI.Models;
using Ves.UI.Services;

namespace Ves.UI.Pages;

public partial class UsersPage : Page
{
    private readonly IAuthService _authService;
    private IReadOnlyCollection<UserAccount> _allUsers;

    public UsersPage(IAuthService authService)
    {
        InitializeComponent();
        _authService = authService;
        _allUsers = authService.GetAllUsers();
        UsersGrid.ItemsSource = _allUsers;
    }

    private void OnSearchClicked(object sender, RoutedEventArgs e)
    {
        string term = SearchTextBox.Text.Trim();
        if (string.IsNullOrWhiteSpace(term))
        {
            UsersGrid.ItemsSource = _allUsers;
            return;
        }

        var filtered = _allUsers
            .Where(u => u.Username.Contains(term, StringComparison.OrdinalIgnoreCase)
                || u.DisplayName.Contains(term, StringComparison.OrdinalIgnoreCase)
                || u.Email.Contains(term, StringComparison.OrdinalIgnoreCase))
            .ToList();

        UsersGrid.ItemsSource = filtered;
    }

    private void OnClearFilterClicked(object sender, RoutedEventArgs e)
    {
        SearchTextBox.Text = string.Empty;
        UsersGrid.ItemsSource = _allUsers;
        UsersGrid.SelectedItem = null;
        DetailPanel.Visibility = Visibility.Collapsed;
    }

    private void OnUserSelected(object sender, SelectionChangedEventArgs e)
    {
        if (UsersGrid.SelectedItem is not UserAccount account)
        {
            DetailPanel.Visibility = Visibility.Collapsed;
            return;
        }

        DetailPanel.Visibility = Visibility.Visible;
        DetailName.Text = account.DisplayName;
        DetailEmail.Text = account.Email;
        DetailRole.Text = $"Rol: {account.Role}";
        DetailStatus.Text = account.IsActive ? "Estado: Activo" : "Estado: Inactivo";
        DetailLastAccess.Text = $"Último acceso: {account.LastAccessUtc:dd/MM/yyyy HH:mm}";
    }

    private void OnResetPasswordClicked(object sender, RoutedEventArgs e)
    {
        if (UsersGrid.SelectedItem is not UserAccount account)
        {
            MessageBox.Show("Seleccioná un usuario antes de continuar.", "Recuperación", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        string message = _authService.BuildRecoveryMessage(account.Email);
        MessageBox.Show(message, "Recuperación enviada", MessageBoxButton.OK, MessageBoxImage.Information);
    }
}
