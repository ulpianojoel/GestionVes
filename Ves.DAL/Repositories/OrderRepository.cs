using System.Data;
using Ves.DAL.Interfaces;
using Ves.Domain.Entities;
using Ves.Domain.Enums;
using Ves.Domain.ValueObjects;

namespace Ves.DAL.Repositories;

/// <summary>
/// ADO.NET implementation of the order repository.
/// </summary>
public class OrderRepository : IOrderRepository
{
    private readonly IDbConnectionFactory _factory;

    public OrderRepository(IDbConnectionFactory factory)
    {
        _factory = factory;
    }

    public int Add(Order order)
    {
        using var conn = _factory.CreateBusinessConnection();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "INSERT INTO Orders(ClientId,Date,Status,TotalAmount,TotalCurrency) OUTPUT INSERTED.Id VALUES(@ClientId,@Date,@Status,@TotalAmount,@TotalCurrency)";
        var pClient = cmd.CreateParameter();
        pClient.ParameterName = "@ClientId";
        pClient.Value = order.ClientId;
        cmd.Parameters.Add(pClient);
        var pDate = cmd.CreateParameter();
        pDate.ParameterName = "@Date";
        pDate.Value = order.Date;
        cmd.Parameters.Add(pDate);
        var pStatus = cmd.CreateParameter();
        pStatus.ParameterName = "@Status";
        pStatus.Value = (int)order.Status;
        cmd.Parameters.Add(pStatus);
        var pTotal = cmd.CreateParameter();
        pTotal.ParameterName = "@TotalAmount";
        pTotal.Value = order.Total.Amount;
        cmd.Parameters.Add(pTotal);
        var pCurrency = cmd.CreateParameter();
        pCurrency.ParameterName = "@TotalCurrency";
        pCurrency.Value = order.Total.Currency;
        cmd.Parameters.Add(pCurrency);
        conn.Open();
        return (int)(cmd.ExecuteScalar() ?? 0);
    }

    public Order? GetById(int id)
    {
        using var conn = _factory.CreateBusinessConnection();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT Id,ClientId,Date,Status,TotalAmount,TotalCurrency FROM Orders WHERE Id=@Id";
        var pId = cmd.CreateParameter();
        pId.ParameterName = "@Id";
        pId.Value = id;
        cmd.Parameters.Add(pId);
        conn.Open();
        using var reader = cmd.ExecuteReader();
        if (!reader.Read())
        {
            return null;
        }

        var order = new Order
        {
            Id = reader.GetInt32(0),
            ClientId = reader.GetInt32(1),
            Date = reader.GetDateTime(2),
            Status = (OrderStatus)reader.GetInt32(3)
        };

        order.LoadTotal(new Money(reader.GetDecimal(4), reader.GetString(5)));

        return order;
    }

    public void UpdateStatus(int id, OrderStatus status)
    {
        using var conn = _factory.CreateBusinessConnection();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "UPDATE Orders SET Status=@Status WHERE Id=@Id";
        var pStatus = cmd.CreateParameter();
        pStatus.ParameterName = "@Status";
        pStatus.Value = (int)status;
        cmd.Parameters.Add(pStatus);
        var pId = cmd.CreateParameter();
        pId.ParameterName = "@Id";
        pId.Value = id;
        cmd.Parameters.Add(pId);
        conn.Open();
        cmd.ExecuteNonQuery();
    }
}
