using System;

namespace Ves.UI.Models;

public sealed class DashboardSummary
{
    public int ActiveUsers { get; init; }

    public int InactiveUsers { get; init; }

    public int PendingOperations { get; init; }

    public int Alerts { get; init; }

    public string LastSyncLabel { get; init; } = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
}
