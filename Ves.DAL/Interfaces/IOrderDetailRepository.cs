using System.Collections.Generic;
using Ves.Domain.Entities;

namespace Ves.DAL.Interfaces;

/// <summary>
/// Handles persistence for order detail records.
/// </summary>
public interface IOrderDetailRepository
{
    void Add(OrderDetail detail);
    IEnumerable<OrderDetail> GetByOrder(int orderId);
}
