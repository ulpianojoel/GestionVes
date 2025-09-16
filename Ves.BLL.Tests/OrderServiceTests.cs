using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ves.BLL.Requests;
using Ves.BLL.Services;
using Ves.DAL.Interfaces;
using Ves.Domain.Entities;
using Ves.Domain.ValueObjects;
using Ves.Services.Interfaces;
using Xunit;

namespace Ves.BLL.Tests;

public class OrderServiceTests
{
    [Fact]
    public void CreateOrder_AggregatesQuantities_WhenProductIsRepeated()
    {
        // Arrange
        var product = new Product
        {
            Id = 1,
            Name = "Producto de prueba",
            UnitPrice = new Money(10m, "USD"),
            Stock = new Quantity(10)
        };

        var initialStock = product.Stock.Value;

        var productRepository = new FakeProductRepository(product);
        var orderRepository = new FakeOrderRepository();
        var detailRepository = new FakeOrderDetailRepository();
        var unitOfWork = new FakeUnitOfWork();
        var audit = new FakeAuditService();
        var service = new OrderService(orderRepository, detailRepository, productRepository, unitOfWork, audit);

        var request = new CreateOrderRequest
        {
            ClientId = 42,
            Details = new[]
            {
                new CreateOrderDetailRequest(product.Id, new Quantity(2)),
                new CreateOrderDetailRequest(product.Id, new Quantity(3))
            }
        };

        // Act
        var order = service.CreateOrder(request);

        // Assert
        Assert.Equal(1, productRepository.LoadCounts[product.Id]);
        Assert.Single(productRepository.UpdateStockCalls);
        Assert.Equal(5, productRepository.UpdateStockCalls[0].Quantity.Value);
        Assert.All(detailRepository.AddedDetails, d => Assert.Equal(product.Id, d.ProductId));
        Assert.Equal(2, detailRepository.AddedDetails.Count);
        Assert.Equal(2, order.Details.Count);
        Assert.True(unitOfWork.Began);
        Assert.True(unitOfWork.Committed);
        Assert.Equal(initialStock - 5, product.Stock.Value);

        var auditEntry = Assert.Single(audit.Entries);
        Assert.Equal("OrderService", auditEntry.Actor);
        Assert.Equal("Order.Create", auditEntry.Action);
        var totalsProperty = auditEntry.Data?.GetType().GetProperty("TotalsByProduct");
        var totals = Assert.IsAssignableFrom<IEnumerable>(totalsProperty?.GetValue(auditEntry.Data));
        var perProduct = Assert.Single(totals.Cast<object>());
        Assert.Equal(product.Id, perProduct?.GetType().GetProperty("ProductId")?.GetValue(perProduct));
        var requested = perProduct?.GetType().GetProperty("Requested")?.GetValue(perProduct);
        Assert.Equal(5, Assert.IsType<int>(requested));
        var stockBefore = perProduct?.GetType().GetProperty("StockBefore")?.GetValue(perProduct);
        Assert.Equal(initialStock, Assert.IsType<int>(stockBefore));
        var stockAfter = perProduct?.GetType().GetProperty("StockAfter")?.GetValue(perProduct);
        Assert.Equal(initialStock - 5, Assert.IsType<int>(stockAfter));
    }

    private sealed class FakeOrderRepository : IOrderRepository
    {
        private int _nextId = 1;
        public int Create(Order order)
        {
            order.Id = _nextId;
            return _nextId++;
        }
    }

    private sealed class FakeOrderDetailRepository : IOrderDetailRepository
    {
        public List<OrderDetail> AddedDetails { get; } = new();

        public void Add(OrderDetail detail)
        {
            AddedDetails.Add(detail);
        }
    }

    private sealed class FakeProductRepository : IProductRepository
    {
        private readonly Dictionary<int, Product> _products;

        public FakeProductRepository(params Product[] products)
        {
            _products = products.ToDictionary(p => p.Id);
        }

        public List<(int ProductId, Quantity Quantity)> UpdateStockCalls { get; } = new();

        public Dictionary<int, int> LoadCounts { get; } = new();

        public Product? GetById(int id)
        {
            if (_products.TryGetValue(id, out var product))
            {
                LoadCounts[id] = LoadCounts.GetValueOrDefault(id) + 1;
                return product;
            }

            return null;
        }

        public void UpdateStock(int id, Quantity quantityToSubtract)
        {
            UpdateStockCalls.Add((id, quantityToSubtract));
            if (_products.TryGetValue(id, out var product))
            {
                product.Stock = new Quantity(product.Stock.Value - quantityToSubtract.Value);
            }
        }
    }

    private sealed class FakeUnitOfWork : IUnitOfWork
    {
        public bool Began { get; private set; }
        public bool Committed { get; private set; }
        public bool RolledBack { get; private set; }

        public void Begin() => Began = true;

        public void Commit() => Committed = true;

        public void Rollback() => RolledBack = true;
    }

    private sealed class FakeAuditService : IAuditService
    {
        public List<AuditEntry> Entries { get; } = new();

        public void Write(string actor, string action, object? data = null)
        {
            Entries.Add(new AuditEntry(actor, action, data));
        }

        public sealed record AuditEntry(string Actor, string Action, object? Data);
    }
}
