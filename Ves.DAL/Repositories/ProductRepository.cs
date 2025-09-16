using System.Data;
using Ves.DAL.Interfaces;
using Ves.Domain.Entities;
using Ves.Domain.ValueObjects;

namespace Ves.DAL.Repositories;

/// <summary>
/// ADO.NET implementation for product persistence.
/// </summary>
public class ProductRepository : IProductRepository
{
    private readonly IDbConnectionFactory _factory;

    public ProductRepository(IDbConnectionFactory factory)
    {
        _factory = factory;
    }

    public Product? GetById(int id)
    {
        using var conn = _factory.CreateBusinessConnection();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT Id,Name,UnitPriceAmount,UnitPriceCurrency,Stock,Active FROM Products WHERE Id=@Id";
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

        return new Product
        {
            Id = reader.GetInt32(0),
            Name = reader.GetString(1),
            UnitPrice = new Money(reader.GetDecimal(2), reader.GetString(3)),
            Stock = new Quantity(reader.GetInt32(4)),
            Active = reader.GetBoolean(5)
        };
    }

    public void UpdateStock(int id, Quantity newStock)
    {
        using var conn = _factory.CreateBusinessConnection();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "UPDATE Products SET Stock=@Stock WHERE Id=@Id";
        var pStock = cmd.CreateParameter();
        pStock.ParameterName = "@Stock";
        pStock.Value = newStock.Value;
        cmd.Parameters.Add(pStock);
        var pId = cmd.CreateParameter();
        pId.ParameterName = "@Id";
        pId.Value = id;
        cmd.Parameters.Add(pId);
        conn.Open();
        cmd.ExecuteNonQuery();
    }
}
