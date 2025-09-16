using System.Collections.Generic;
using Ves.UI.Models;

namespace Ves.UI.ViewModels;

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

    public DashboardSummary Summary { get; }

    public IReadOnlyCollection<OperationItem> Operations { get; }

    public IReadOnlyCollection<NotificationMessage> Notifications { get; }
}
