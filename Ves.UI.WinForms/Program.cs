using System;
using System.Windows.Forms;

namespace Ves.UI.WinForms
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new Ves.UI.WinForms.Forms.LoginForm());
        }
    }
}
