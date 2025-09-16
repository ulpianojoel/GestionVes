using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Ves.UI.Models;

namespace Ves.UI.Pages
{
    public partial class OperationsPage : Page
    {
        private readonly ObservableCollection<OperationItem> _operations;
        private OperationItem _selected;

        public OperationsPage(IEnumerable<OperationItem> operations)
        {
            InitializeComponent();
            _operations = new ObservableCollection<OperationItem>(operations ?? Enumerable.Empty<OperationItem>());
            OperationsList.ItemsSource = _operations;
            if (_operations.Count > 0)
            {
                OperationsList.SelectedIndex = 0;
            }
        }

        private void OnOperationSelected(object sender, SelectionChangedEventArgs e)
        {
            var operation = OperationsList.SelectedItem as OperationItem;
            if (operation == null)
            {
                _selected = null;
                OperationTitle.Text = string.Empty;
                OperationDescription.Text = string.Empty;
                OperationMessage.Text = "Seleccioná una operación para editarla.";
                StatusCombo.SelectedItem = null;
                DueDatePicker.SelectedDate = null;
                return;
            }

            _selected = operation;
            RefreshDetails();
        }

        private void RefreshDetails()
        {
            if (_selected == null)
            {
                return;
            }

            OperationTitle.Text = _selected.Title;
            OperationDescription.Text = _selected.Description;
            OperationMessage.Text = string.Empty;

            var statusItem = StatusCombo.Items.OfType<ComboBoxItem>()
                .FirstOrDefault(item => string.Equals(item.Content != null ? item.Content.ToString() : string.Empty, _selected.Status, StringComparison.OrdinalIgnoreCase));
            StatusCombo.SelectedItem = statusItem;
            DueDatePicker.SelectedDate = _selected.DueDate;
        }

        private void OnStatusChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_selected == null)
            {
                return;
            }

            var item = StatusCombo.SelectedItem as ComboBoxItem;
            if (item == null)
            {
                return;
            }

            var content = item.Content != null ? item.Content.ToString() : null;
            if (!string.IsNullOrEmpty(content))
            {
                _selected.Status = content;
            }

            OperationMessage.Text = "Estado actualizado a '" + _selected.Status + "'.";
            OperationsList.Items.Refresh();
        }

        private void OnDueDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_selected == null)
            {
                return;
            }

            _selected.DueDate = DueDatePicker.SelectedDate;
            if (_selected.DueDate == null)
            {
                OperationMessage.Text = "Se eliminó la fecha de vencimiento.";
            }
            else
            {
                OperationMessage.Text = "Vence el " + _selected.DueDate.Value.ToString("dd/MM/yyyy") + ".";
            }

            OperationsList.Items.Refresh();
        }

        private void OnCompleteClicked(object sender, RoutedEventArgs e)
        {
            if (_selected == null)
            {
                MessageBox.Show("Seleccioná una operación primero.", "Operaciones", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            _selected.Status = "Completado";
            OperationMessage.Text = "Marcaste la operación como completada.";
            OperationsList.Items.Refresh();
            RefreshDetails();
        }
    }
}
