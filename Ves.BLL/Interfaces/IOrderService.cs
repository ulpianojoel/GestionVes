using System.Collections.Generic;
using Ves.BLL.Models;
using Ves.Domain.Entities;

namespace Ves.BLL.Interfaces;

/// <summary>
/// Handles orchestration of order related use cases.
/// </summary>
public interface IOrderService
{
    /// <summary>
    /// Creates a new order for a client using the provided items.
    /// </summary>
    Order CreateOrder(int clientId, IEnumerable<OrderItemRequest> items);

    /// <summary>
    /// Confirms a pending order and records a sale.
    /// </summary>
    Sale ConfirmOrder(int orderId);

    /// <summary>
    /// Cancels a pending order restoring product stock.
    /// </summary>
    void CancelOrder(int orderId, string reason);
}
