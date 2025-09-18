using System.Windows.Forms;

namespace Ves.UI.WinForms.Forms
{
    public class ProductsForm : Form
    {
        public ProductsForm()
        {
            Text = "Productos";
            Width = 800;
            Height = 500;
            StartPosition = FormStartPosition.CenterScreen;

            Controls.Add(new Label
            {
                Text = "(WIP) Pantalla de productos",
                Dock = DockStyle.Fill,
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            });
        }
    }
}
