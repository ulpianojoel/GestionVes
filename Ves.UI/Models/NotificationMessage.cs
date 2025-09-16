using System;

namespace Ves.UI.Models;

public sealed class NotificationMessage
{
    public required string Title { get; init; }

    public required string Detail { get; init; }

    public DateTime CreatedUtc { get; init; }

    public string RelativeTime => $"hace {(int)Math.Max(1, (DateTime.UtcNow - CreatedUtc).TotalMinutes)} min";
}
