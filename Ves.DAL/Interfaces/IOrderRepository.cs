using Ves.Domain.Entities;
using Ves.Domain.Enums;

#nullable enable

namespace Ves.DAL.Interfaces;

/// <summary>
/// Provides persistence capabilities for orders.
/// </summary>
public interface IOrderRepository
{
    int Add(Order order);
    Order? GetById(int id);
    void UpdateStatus(int id, OrderStatus status);
}
