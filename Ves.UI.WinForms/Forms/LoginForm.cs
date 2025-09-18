using System.Windows.Forms;

namespace Ves.UI.WinForms.Forms
{
    public class LoginForm : Form
    {
        private readonly TextBox _txtUser = new() { Dock = DockStyle.Top, PlaceholderText = "Usuario" };
        private readonly TextBox _txtPass = new() { Dock = DockStyle.Top, PlaceholderText = "Contraseña", UseSystemPasswordChar = true };
        private readonly Button _btn = new() { Dock = DockStyle.Top, Text = "Ingresar", Height = 36 };

        public LoginForm()
        {
            Text = "VES - Login";
            Width = 380;
            Height = 220;
            StartPosition = FormStartPosition.CenterScreen;

            Controls.Add(_btn);
            Controls.Add(_txtPass);
            Controls.Add(new Label { Text = "Contraseña", Dock = DockStyle.Top });
            Controls.Add(_txtUser);
            Controls.Add(new Label { Text = "Usuario", Dock = DockStyle.Top });

            _btn.Click += (_, __) => { new MainForm().Show(); Hide(); };
        }
    }
}
