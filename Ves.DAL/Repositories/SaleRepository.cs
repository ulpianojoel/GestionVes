using System.Data;
using Ves.DAL.Interfaces;
using Ves.Domain.Entities;

namespace Ves.DAL.Repositories;

/// <summary>
/// Persists sales records.
/// </summary>
public class SaleRepository : ISaleRepository
{
    private readonly IDbConnectionFactory _factory;

    public SaleRepository(IDbConnectionFactory factory)
    {
        _factory = factory;
    }

    public int Add(Sale sale)
    {
        using var conn = _factory.CreateBusinessConnection();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "INSERT INTO Sales(Date,ClientId,TotalAmount,TotalCurrency) OUTPUT INSERTED.Id VALUES(@Date,@ClientId,@TotalAmount,@TotalCurrency)";
        var pDate = cmd.CreateParameter();
        pDate.ParameterName = "@Date";
        pDate.Value = sale.Date;
        cmd.Parameters.Add(pDate);
        var pClient = cmd.CreateParameter();
        pClient.ParameterName = "@ClientId";
        pClient.Value = sale.ClientId;
        cmd.Parameters.Add(pClient);
        var pAmount = cmd.CreateParameter();
        pAmount.ParameterName = "@TotalAmount";
        pAmount.Value = sale.Total.Amount;
        cmd.Parameters.Add(pAmount);
        var pCurrency = cmd.CreateParameter();
        pCurrency.ParameterName = "@TotalCurrency";
        pCurrency.Value = sale.Total.Currency;
        cmd.Parameters.Add(pCurrency);
        conn.Open();
        return (int)(cmd.ExecuteScalar() ?? 0);
    }
}
