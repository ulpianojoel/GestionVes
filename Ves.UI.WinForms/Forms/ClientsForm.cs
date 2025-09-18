using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ves.BLL.Services;
using Ves.Domain.Entities;

namespace Ves.UI.WinForms.Forms
{
    public class ClientsForm : Form
    {
        private readonly ClientService _svc;
        private readonly DataGridView _grid = new() { Dock = DockStyle.Fill, ReadOnly = true, AutoGenerateColumns = true };
        private readonly TextBox _txtNombre = new() { Dock = DockStyle.Top, Width = 300 };
        private readonly Button _btnAgregar = new() { Text = "Agregar", Dock = DockStyle.Top, Height = 32 };
        private readonly Panel _panel = new() { Dock = DockStyle.Top, Height = 80 };

        private const string cs = @"Server=.\SQLEXPRESS;Database=GestionVes;Trusted_Connection=True;TrustServerCertificate=True;";

        public ClientsForm()
        {
            Text = "Clientes"; Width = 900; Height = 550; StartPosition = FormStartPosition.CenterScreen;
            _svc = new ClientService(cs);

            _panel.Controls.Add(_btnAgregar);
            _panel.Controls.Add(_txtNombre);
            _panel.Controls.Add(new Label { Text = "Nombre", Dock = DockStyle.Top });

            Controls.Add(_grid);
            Controls.Add(_panel);

            Load += async (_, __) => await LoadDataAsync();
            _btnAgregar.Click += async (_, __) => await AddAsync();
        }

        private async Task LoadDataAsync()
        {
            try { _grid.DataSource = await _svc.GetAllAsync(); }
            catch (Exception ex) { MessageBox.Show("Error cargando clientes: " + ex.Message); }
        }

        private async Task AddAsync()
        {
            var nombre = (_txtNombre.Text ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(nombre)) { MessageBox.Show("Ingrese un nombre"); _txtNombre.Focus(); return; }
            try
            {
                await _svc.InsertAsync(new Cliente { Nombre = nombre, FechaAlta = DateTime.Now, Activo = true });
                _txtNombre.Clear();
                await LoadDataAsync();
            }
            catch (Exception ex) { MessageBox.Show("Error agregando cliente: " + ex.Message); }
        }
    }
}
