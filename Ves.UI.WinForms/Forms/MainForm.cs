using System;
using System.Windows.Forms;

namespace Ves.UI.WinForms.Forms
{
    public class MainForm : Form
    {
        public MainForm()
        {
            Text = "VES - Sistema";
            Width = 1200;
            Height = 700;
            StartPosition = FormStartPosition.CenterScreen;

            // Cuadrícula 2x2 con botones centrados
            var grid = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 2
            };
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            grid.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
            grid.RowStyles.Add(new RowStyle(SizeType.Percent, 50));

            Button Tile(string texto, Action onClick)
            {
                var b = new Button
                {
                    Text = texto,
                    Width = 260,
                    Height = 120,
                    Anchor = AnchorStyles.None
                };
                b.Font = new System.Drawing.Font("Segoe UI", 16, System.Drawing.FontStyle.Bold);
                b.Click += (_, __) => onClick();
                return b;
            }

            Panel Center(Control c)
            {
                var p = new Panel { Dock = DockStyle.Fill };
                p.Resize += (_, __) => { c.Left = (p.Width - c.Width) / 2; c.Top = (p.Height - c.Height) / 2; };
                p.Controls.Add(c);
                return p;
            }

            grid.Controls.Add(Center(Tile("Clientes", () => new ClientsForm().ShowDialog(this))), 0, 0);
            grid.Controls.Add(Center(Tile("Productos", () => new ProductsForm().ShowDialog(this))), 1, 0);
            grid.Controls.Add(Center(Tile("Pedidos", () => new OrdersForm().ShowDialog(this))), 0, 1);
            grid.Controls.Add(Center(Tile("Reportes", () => new ReportsForm().ShowDialog(this))), 1, 1);

            Controls.Add(grid);
        }
    }
}
