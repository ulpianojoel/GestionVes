using System.Collections.Generic;
using System.Data;
using Ves.DAL.Interfaces;
using Ves.Domain.Entities;
using Ves.Domain.ValueObjects;

namespace Ves.DAL.Repositories;

/// <summary>
/// ADO.NET repository for order detail rows.
/// </summary>
public class OrderDetailRepository : IOrderDetailRepository
{
    private readonly IDbConnectionFactory _factory;

    public OrderDetailRepository(IDbConnectionFactory factory)
    {
        _factory = factory;
    }

    public void Add(OrderDetail detail)
    {
        using var conn = _factory.CreateBusinessConnection();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "INSERT INTO OrderDetails(OrderId,ProductId,Quantity,UnitPriceAmount,UnitPriceCurrency) VALUES(@OrderId,@ProductId,@Quantity,@UnitPriceAmount,@UnitPriceCurrency)";
        var pOrder = cmd.CreateParameter();
        pOrder.ParameterName = "@OrderId";
        pOrder.Value = detail.OrderId;
        cmd.Parameters.Add(pOrder);
        var pProduct = cmd.CreateParameter();
        pProduct.ParameterName = "@ProductId";
        pProduct.Value = detail.ProductId;
        cmd.Parameters.Add(pProduct);
        var pQuantity = cmd.CreateParameter();
        pQuantity.ParameterName = "@Quantity";
        pQuantity.Value = detail.Quantity.Value;
        cmd.Parameters.Add(pQuantity);
        var pAmount = cmd.CreateParameter();
        pAmount.ParameterName = "@UnitPriceAmount";
        pAmount.Value = detail.UnitPrice.Amount;
        cmd.Parameters.Add(pAmount);
        var pCurrency = cmd.CreateParameter();
        pCurrency.ParameterName = "@UnitPriceCurrency";
        pCurrency.Value = detail.UnitPrice.Currency;
        cmd.Parameters.Add(pCurrency);
        conn.Open();
        cmd.ExecuteNonQuery();
    }

    public IEnumerable<OrderDetail> GetByOrder(int orderId)
    {
        using var conn = _factory.CreateBusinessConnection();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT Id,OrderId,ProductId,Quantity,UnitPriceAmount,UnitPriceCurrency FROM OrderDetails WHERE OrderId=@OrderId";
        var pOrder = cmd.CreateParameter();
        pOrder.ParameterName = "@OrderId";
        pOrder.Value = orderId;
        cmd.Parameters.Add(pOrder);
        conn.Open();
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            yield return new OrderDetail
            {
                Id = reader.GetInt32(0),
                OrderId = reader.GetInt32(1),
                ProductId = reader.GetInt32(2),
                Quantity = new Quantity(reader.GetInt32(3)),
                UnitPrice = new Money(reader.GetDecimal(4), reader.GetString(5))
            };
        }
    }
}
