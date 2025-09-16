namespace Ves.UI.Models;

public sealed class ReportSnapshot
{
    public required string Title { get; init; }

    public required string Description { get; init; }

    public required string Period { get; init; }

    public required string Trend { get; init; }
}
