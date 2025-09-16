using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Ves.UI.Models;

namespace Ves.UI.Pages;

public partial class OperationsPage : Page
{
    private readonly ObservableCollection<OperationItem> _operations;
    private OperationItem? _selected;

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
        if (OperationsList.SelectedItem is not OperationItem operation)
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
        if (_selected is null)
        {
            return;
        }

        OperationTitle.Text = _selected.Title;
        OperationDescription.Text = _selected.Description;
        OperationMessage.Text = string.Empty;

        var statusItem = StatusCombo.Items.OfType<ComboBoxItem>()
            .FirstOrDefault(item => string.Equals(item.Content?.ToString(), _selected.Status, StringComparison.OrdinalIgnoreCase));
        StatusCombo.SelectedItem = statusItem;
        DueDatePicker.SelectedDate = _selected.DueDate;
    }

    private void OnStatusChanged(object sender, SelectionChangedEventArgs e)
    {
        if (_selected is null || StatusCombo.SelectedItem is not ComboBoxItem item)
        {
            return;
        }

        _selected.Status = item.Content?.ToString() ?? _selected.Status;
        OperationMessage.Text = $"Estado actualizado a '{_selected.Status}'.";
        OperationsList.Items.Refresh();
    }

    private void OnDueDateChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (_selected is null)
        {
            return;
        }

        _selected.DueDate = DueDatePicker.SelectedDate;
        OperationMessage.Text = _selected.DueDate is null
            ? "Se eliminó la fecha de vencimiento."
            : $"Vence el {_selected.DueDate:dd/MM/yyyy}.";
        OperationsList.Items.Refresh();
    }

    private void OnCompleteClicked(object sender, RoutedEventArgs e)
    {
        if (_selected is null)
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
