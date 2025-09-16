using System;

namespace Ves.UI.Models;

public sealed class OperationItem
{
    public required string Title { get; init; }

    public required string Description { get; init; }

    public string Status { get; set; } = "Pendiente";

    public DateTime? DueDate { get; set; }

    public string DueLabel => DueDate?.ToString("dd/MM/yyyy") ?? "Sin fecha";
}
