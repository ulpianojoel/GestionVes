using Ves.Domain.Entities;

namespace Ves.DAL.Interfaces;

/// <summary>
/// Handles persistence of order aggregates.
/// </summary>
public interface IOrderRepository
{
    /// <summary>
    /// Persists an order and returns the generated identifier.
    /// </summary>
    int Create(Order order);
}
