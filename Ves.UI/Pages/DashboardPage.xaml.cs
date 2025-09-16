using System.Windows.Controls;
using Ves.UI.ViewModels;

namespace Ves.UI.Pages
{
    public partial class DashboardPage : Page
    {
        public DashboardPage(DashboardViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
