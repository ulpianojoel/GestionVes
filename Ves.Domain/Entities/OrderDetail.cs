using Ves.Domain.ValueObjects;

namespace Ves.Domain.Entities;

/// <summary>
/// Line item belonging to an order.
/// </summary>
public class OrderDetail
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public Quantity Quantity { get; set; } = new(0);
    public Money UnitPrice { get; set; } = new(0m, "USD");
    public Money Subtotal() => new(UnitPrice.Amount * Quantity.Value, UnitPrice.Currency);
}
