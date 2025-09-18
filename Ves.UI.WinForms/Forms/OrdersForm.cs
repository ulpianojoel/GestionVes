using System.Windows.Forms;

namespace Ves.UI.WinForms.Forms
{
    public class OrdersForm : Form
    {
        public OrdersForm()
        {
            Text = "Pedidos";
            Width = 900;
            Height = 550;
            StartPosition = FormStartPosition.CenterScreen;

            Controls.Add(new Label
            {
                Text = "(WIP) Generación de pedidos",
                Dock = DockStyle.Fill,
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            });
        }
    }
}
