using System;
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
        Details.Add(detail);
        RecalculateTotal();
    }

    private void RecalculateTotal()
    {
        var amount = Details.Sum(d => d.Subtotal().Amount);
        Total = new Money(amount, Total.Currency);
    }
}
