using System;
using System.Collections.Generic;
using System.Linq;
using Ves.BLL.Interfaces;
using Ves.BLL.Requests;
using Ves.DAL.Interfaces;
using Ves.Domain.Entities;
using Ves.Domain.ValueObjects;
using Ves.Services.Interfaces;

namespace Ves.BLL.Services;

/// <summary>
/// Coordinates the creation of orders ensuring stock is respected.
/// </summary>
public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IOrderDetailRepository _orderDetailRepository;
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAuditService _audit;

    public OrderService(
        IOrderRepository orderRepository,
        IOrderDetailRepository orderDetailRepository,
        IProductRepository productRepository,
        IUnitOfWork unitOfWork,
        IAuditService audit)
    {
        _orderRepository = orderRepository;
        _orderDetailRepository = orderDetailRepository;
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
        _audit = audit;
    }

    public Order CreateOrder(CreateOrderRequest request)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        if (request.Details is null || !request.Details.Any())
        {
            throw new ArgumentException("Order must include at least one detail.", nameof(request));
        }

        _unitOfWork.Begin();
        var order = new Order { ClientId = request.ClientId };
        var productRequests = new Dictionary<int, ProductRequestState>();

        try
        {
            foreach (var detailRequest in request.Details)
            {
                var productState = GetOrAddProductState(detailRequest.ProductId, productRequests);
                var potentialTotal = productState.Requested + detailRequest.Quantity;

                if (potentialTotal.Value > productState.OriginalStock.Value)
                {
                    throw new InvalidOperationException(
                        $"Insufficient stock for product {productState.Product.Id}. " +
                        $"Requested {potentialTotal.Value} but only {productState.OriginalStock.Value} available.");
                }

                productState.Register(detailRequest.Quantity);

                var detail = new OrderDetail
                {
                    ProductId = detailRequest.ProductId,
                    Quantity = detailRequest.Quantity,
                    UnitPrice = productState.Product.UnitPrice
                };

                order.AddItem(detail);
            }

            var orderId = _orderRepository.Create(order);
            order.Id = orderId;

            foreach (var detail in order.Details)
            {
                detail.OrderId = orderId;
                _orderDetailRepository.Add(detail);
            }

            foreach (var productState in productRequests.Values)
            {
                _productRepository.UpdateStock(productState.Product.Id, productState.Requested);
            }

            _unitOfWork.Commit();

            _audit.Write("OrderService", "Order.Create", new
            {
                order.Id,
                order.ClientId,
                order.Total,
                TotalsByProduct = productRequests.Values.Select(state => new
                {
                    ProductId = state.Product.Id,
                    state.Product.Name,
                    Requested = state.Requested.Value,
                    StockBefore = state.OriginalStock.Value,
                    StockAfter = state.RemainingStock.Value
                }).ToList(),
                Details = order.Details.Select(d => new
                {
                    d.ProductId,
                    Quantity = d.Quantity.Value,
                    UnitPrice = d.UnitPrice.Amount,
                    Subtotal = d.Subtotal().Amount
                }).ToList()
            });

            return order;
        }
        catch
        {
            _unitOfWork.Rollback();
            throw;
        }
    }

    private ProductRequestState GetOrAddProductState(
        int productId,
        IDictionary<int, ProductRequestState> productRequests)
    {
        if (!productRequests.TryGetValue(productId, out var state))
        {
            var product = _productRepository.GetById(productId)
                ?? throw new InvalidOperationException($"Product {productId} was not found.");
            state = new ProductRequestState(product);
            productRequests[productId] = state;
        }

        return state;
    }

    private sealed class ProductRequestState
    {
        public ProductRequestState(Product product)
        {
            Product = product;
            OriginalStock = product.Stock;
            Requested = new Quantity(0);
        }

        public Product Product { get; }
        public Quantity OriginalStock { get; }
        public Quantity Requested { get; private set; }
        public Quantity RemainingStock => OriginalStock - Requested;

        public void Register(Quantity quantity)
        {
            Requested = Requested + quantity;
        }
    }
}
