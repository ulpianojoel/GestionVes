using Ves.Domain.Entities;
using Ves.Domain.ValueObjects;

namespace Ves.DAL.Interfaces;

/// <summary>
/// Provides data access operations for products.
/// </summary>
public interface IProductRepository
{
    /// <summary>
    /// Retrieves a product by its identifier.
    /// </summary>
    Product? GetById(int id);

    /// <summary>
    /// Decreases the stock of the product by the provided quantity.
    /// </summary>
    void UpdateStock(int id, Quantity quantityToSubtract);
}
