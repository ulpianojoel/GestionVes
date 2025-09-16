namespace Ves.BLL.Models;

/// <summary>
/// Represents a product request for an order creation flow.
/// </summary>
/// <param name="ProductId">Identifier of the product to add.</param>
/// <param name="Quantity">Desired quantity.</param>
public record OrderItemRequest(int ProductId, int Quantity);
