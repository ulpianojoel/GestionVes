using Ves.Domain.Entities;
using Ves.Domain.ValueObjects;

namespace Ves.DAL.Interfaces;

/// <summary>
/// Repository abstraction for products.
/// </summary>
public interface IProductRepository
{
    Product? GetById(int id);
    void UpdateStock(int id, Quantity newStock);
}
