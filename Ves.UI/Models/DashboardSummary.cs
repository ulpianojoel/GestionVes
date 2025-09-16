using System;

namespace Ves.UI.Models
{
    public sealed class DashboardSummary
    {
        public DashboardSummary()
        {
            LastSyncLabel = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
        }

        public int ActiveUsers { get; set; }

        public int InactiveUsers { get; set; }

        public int PendingOperations { get; set; }

        public int Alerts { get; set; }

        public string LastSyncLabel { get; set; }
    }
}
