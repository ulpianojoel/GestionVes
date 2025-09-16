using Ves.BLL.Requests;
using Ves.Domain.Entities;

namespace Ves.BLL.Interfaces;

/// <summary>
/// Exposes use cases related to order management.
/// </summary>
public interface IOrderService
{
    /// <summary>
    /// Creates a new order based on the provided request data.
    /// </summary>
    Order CreateOrder(CreateOrderRequest request);
}
