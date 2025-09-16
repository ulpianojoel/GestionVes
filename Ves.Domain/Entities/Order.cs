using System.Linq;
using System.Collections.Generic;
using Ves.Domain.Enums;
using Ves.Domain.ValueObjects;

namespace Ves.Domain.Entities;

/// <summary>
/// Represents a customer order aggregate root.
/// </summary>
public class Order
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public DateTime Date { get; set; } = DateTime.UtcNow;
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    public Money Total { get; private set; } = new(0m, "USD");
    public List<OrderDetail> Details { get; } = new();

    /// <summary>
    /// Adds a detail to the order enforcing invariant of total.
    /// </summary>
    public void AddItem(OrderDetail detail)
    {
        if (Details.Count == 0)
        {
            Total = new Money(Total.Amount, detail.UnitPrice.Currency);
        }

        Details.Add(detail);
        RecalculateTotal();
    }

    /// <summary>
    /// Allows loading the total amount from persistence while keeping encapsulation.
    /// </summary>
    public void LoadTotal(Money total) => Total = total;

    /// <summary>
    /// Replaces the current set of details with the provided ones without altering totals.
    /// </summary>
    public void ReplaceDetails(IEnumerable<OrderDetail> details)
    {
        var materialized = details.ToList();
        Details.Clear();
        if (materialized.Count > 0)
        {
            Total = new Money(Total.Amount, materialized[0].UnitPrice.Currency);
        }

        Details.AddRange(materialized);
        RecalculateTotal();
    }

    private void RecalculateTotal()
    {
        var amount = Details.Sum(d => d.Subtotal().Amount);
        Total = new Money(amount, Total.Currency);
    }
}
