using System;
using System.Collections.Generic;
using System.Linq;
using Ves.BLL.Interfaces;
using Ves.BLL.Models;
using Ves.DAL.Interfaces;
using Ves.Domain.Entities;
using Ves.Domain.Enums;
using Ves.Domain.ValueObjects;
using Ves.Services.Interfaces;

namespace Ves.BLL.Services;

/// <summary>
/// Implements order related orchestration applying business rules.
/// </summary>
public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IOrderDetailRepository _detailRepository;
    private readonly IProductRepository _productRepository;
    private readonly ISaleRepository _saleRepository;
    private readonly IAuditService _audit;

    public OrderService(
        IOrderRepository orderRepository,
        IOrderDetailRepository detailRepository,
        IProductRepository productRepository,
        ISaleRepository saleRepository,
        IAuditService audit)
    {
        _orderRepository = orderRepository;
        _detailRepository = detailRepository;
        _productRepository = productRepository;
        _saleRepository = saleRepository;
        _audit = audit;
    }

    public Order CreateOrder(int clientId, IEnumerable<OrderItemRequest> items)
    {
        ArgumentNullException.ThrowIfNull(items);
        var materialized = items.ToList();
        if (materialized.Count == 0)
        {
            throw new ArgumentException("Se requiere al menos un producto para crear el pedido.", nameof(items));
        }

        var order = new Order
        {
            ClientId = clientId,
            Status = OrderStatus.Pending
        };

        var productSnapshots = new List<(Product Product, Quantity Quantity)>();
        foreach (var item in materialized)
        {
            if (item.Quantity <= 0)
            {
                throw new InvalidOperationException($"La cantidad para el producto {item.ProductId} debe ser positiva.");
            }

            var product = _productRepository.GetById(item.ProductId)
                ?? throw new InvalidOperationException($"Producto con ID {item.ProductId} no encontrado.");

            if (!product.Active)
            {
                throw new InvalidOperationException($"El producto {product.Name} se encuentra inactivo.");
            }

            var requestedQty = new Quantity(item.Quantity);
            if (product.Stock.Value < requestedQty.Value)
            {
                throw new InvalidOperationException($"Stock insuficiente para {product.Name}. Disponible: {product.Stock.Value}.");
            }

            var detail = new OrderDetail
            {
                ProductId = product.Id,
                Quantity = requestedQty,
                UnitPrice = product.UnitPrice
            };

            order.AddItem(detail);
            productSnapshots.Add((product, requestedQty));
        }

        var orderId = _orderRepository.Add(order);
        order.Id = orderId;

        foreach (var detail in order.Details)
        {
            detail.OrderId = orderId;
            _detailRepository.Add(detail);
        }

        foreach (var (product, qty) in productSnapshots)
        {
            var remaining = product.Stock - qty;
            _productRepository.UpdateStock(product.Id, remaining);
        }

        _audit.Write("BLL", "Order.Create", new { orderId, clientId, items = materialized.Count, total = order.Total.ToString() });

        return order;
    }

    public Sale ConfirmOrder(int orderId)
    {
        var order = _orderRepository.GetById(orderId)
            ?? throw new InvalidOperationException($"Pedido con ID {orderId} no encontrado.");

        if (order.Status != OrderStatus.Pending)
        {
            throw new InvalidOperationException("Solo es posible confirmar pedidos en estado pendiente.");
        }

        var details = _detailRepository.GetByOrder(orderId).ToList();
        if (details.Count == 0)
        {
            throw new InvalidOperationException("El pedido no posee detalles registrados.");
        }

        order.ReplaceDetails(details);

        var sale = new Sale
        {
            ClientId = order.ClientId,
            Date = DateTime.UtcNow,
            Total = order.Total
        };

        var saleId = _saleRepository.Add(sale);
        sale.Id = saleId;

        _orderRepository.UpdateStatus(orderId, OrderStatus.Confirmed);

        _audit.Write("BLL", "Order.Confirm", new { orderId, saleId, total = sale.Total.ToString() });

        return sale;
    }

    public void CancelOrder(int orderId, string reason)
    {
        var order = _orderRepository.GetById(orderId)
            ?? throw new InvalidOperationException($"Pedido con ID {orderId} no encontrado.");

        if (order.Status != OrderStatus.Pending)
        {
            throw new InvalidOperationException("Solo es posible cancelar pedidos en estado pendiente.");
        }

        var details = _detailRepository.GetByOrder(orderId).ToList();
        foreach (var detail in details)
        {
            var product = _productRepository.GetById(detail.ProductId);
            if (product is null)
            {
                continue;
            }

            var restoredStock = product.Stock + detail.Quantity;
            _productRepository.UpdateStock(product.Id, restoredStock);
        }

        _orderRepository.UpdateStatus(orderId, OrderStatus.Cancelled);

        _audit.Write("BLL", "Order.Cancel", new { orderId, reason });
    }
}
