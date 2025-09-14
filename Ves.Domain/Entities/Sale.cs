using Ves.Domain.ValueObjects;

namespace Ves.Domain.Entities;

/// <summary>
/// Represents a confirmed sale.
/// </summary>
public class Sale
{
    public int Id { get; set; }
    public DateTime Date { get; set; } = DateTime.UtcNow;
    public int ClientId { get; set; }
    public Money Total { get; set; } = new(0m, "USD");
}
