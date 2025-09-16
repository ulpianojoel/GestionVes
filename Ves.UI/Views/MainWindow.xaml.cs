using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Ves.UI.Bootstrap;
using Ves.UI.Models;
using Ves.UI.Pages;
using Ves.UI.Services;
using Ves.UI.ViewModels;

namespace Ves.UI.Views
{
    public partial class MainWindow : Window
    {
        private readonly AppEnvironment _environment;
        private readonly UserAccount _currentUser;
        private readonly IAuthService _authService;
        private readonly SampleDataService _sampleData;
        private readonly List<OperationItem> _operations;
        private readonly IReadOnlyCollection<NotificationMessage> _notifications;
        private readonly IReadOnlyCollection<ReportSnapshot> _reports;

        public MainWindow(AppEnvironment environment, UserAccount currentUser, IAuthService authService)
        {
            InitializeComponent();
            _environment = environment;
            _currentUser = currentUser;
            _authService = authService;
            _sampleData = new SampleDataService();

            _operations = _sampleData.GetOperationPipeline().ToList();
            _notifications = _sampleData.GetNotifications();
            _reports = _sampleData.GetReportSnapshots();

            GreetingText.Text = "Hola, " + GetFirstName(_currentUser.DisplayName);
            UserNameText.Text = _currentUser.DisplayName;
            UserRoleText.Text = _currentUser.Role;

            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            NavigateTo("Dashboard");
        }

        private void OnNavigationChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = NavigationList.SelectedItem as ListBoxItem;
            if (item == null)
            {
                return;
            }

            var tag = item.Tag != null ? item.Tag.ToString() : string.Empty;
            NavigateTo(tag);
        }

        private void NavigateTo(string tag)
        {
            if (string.IsNullOrEmpty(tag))
            {
                return;
            }

            switch (tag)
            {
                case "Dashboard":
                    var summary = _sampleData.BuildSummary(_authService.GetAllUsers(), _operations);
                    var dashboardViewModel = new DashboardViewModel(summary, _operations, _notifications);
                    ContentFrame.Content = new DashboardPage(dashboardViewModel);
                    break;
                case "Users":
                    ContentFrame.Content = new UsersPage(_authService);
                    break;
                case "Operations":
                    ContentFrame.Content = new OperationsPage(_operations);
                    break;
                case "Reports":
                    ContentFrame.Content = new ReportsPage(_reports);
                    break;
                case "Connections":
                    ContentFrame.Content = new ConnectionsPage(_environment);
                    break;
            }
        }

        private static string GetFirstName(string displayName)
        {
            if (string.IsNullOrWhiteSpace(displayName))
            {
                return "usuario";
            }

            var parts = displayName.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            return parts.Length > 0 ? parts[0] : displayName;
        }
    }
}
