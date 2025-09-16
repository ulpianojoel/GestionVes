using System.Collections.Generic;
using Ves.UI.Models;

namespace Ves.UI.ViewModels
{
    public sealed class DashboardViewModel
    {
        public DashboardViewModel(
            DashboardSummary summary,
            IReadOnlyCollection<OperationItem> operations,
            IReadOnlyCollection<NotificationMessage> notifications)
        {
            Summary = summary;
            Operations = operations;
            Notifications = notifications;
        }

        public DashboardSummary Summary { get; private set; }

        public IReadOnlyCollection<OperationItem> Operations { get; private set; }

        public IReadOnlyCollection<NotificationMessage> Notifications { get; private set; }
    }
}
