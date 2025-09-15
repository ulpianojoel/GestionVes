using Ves.Domain.ValueObjects;

namespace Ves.Domain.Entities;

/// <summary>
/// Product available for sale.
/// </summary>
public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Money UnitPrice { get; set; } = new(0m, "USD");
    public Quantity Stock { get; set; } = new(0);
    public bool Active { get; set; } = true;
}
