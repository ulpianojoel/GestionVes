using System.Windows.Forms;

namespace Ves.UI.WinForms.Forms
{
    public class ReportsForm : Form
    {
        public ReportsForm()
        {
            Text = "Reportes";
            Width = 900;
            Height = 550;
            StartPosition = FormStartPosition.CenterScreen;

            Controls.Add(new Label
            {
                Text = "(WIP) Reportes",
                Dock = DockStyle.Fill,
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            });
        }
    }
}
