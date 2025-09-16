using Ves.Domain.Entities;

namespace Ves.DAL.Interfaces;

/// <summary>
/// Provides persistence operations for order details.
/// </summary>
public interface IOrderDetailRepository
{
    /// <summary>
    /// Adds a detail line to an order.
    /// </summary>
    void Add(OrderDetail detail);
}
