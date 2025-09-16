using System;
using System.Collections.Generic;
using Ves.Domain.ValueObjects;

namespace Ves.BLL.Requests;

/// <summary>
/// Payload describing an order to be created.
/// </summary>
public class CreateOrderRequest
{
    /// <summary>
    /// Identifier of the client placing the order.
    /// </summary>
    public int ClientId { get; init; }

    /// <summary>
    /// Collection of requested detail lines.
    /// </summary>
    public IReadOnlyCollection<CreateOrderDetailRequest> Details { get; init; } = Array.Empty<CreateOrderDetailRequest>();
}

/// <summary>
/// Represents a single detail line in an order creation request.
/// </summary>
/// <param name="ProductId">Identifier of the product being ordered.</param>
/// <param name="Quantity">Requested quantity.</param>
public record CreateOrderDetailRequest(int ProductId, Quantity Quantity);
