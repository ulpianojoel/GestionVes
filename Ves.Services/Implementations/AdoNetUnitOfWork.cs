using System.Data;
using Ves.DAL.Interfaces;
using Ves.Services.Interfaces;

#nullable enable

namespace Ves.Services.Implementations;

/// <summary>
/// Simple ADO.NET unit of work implementation using a single SQL transaction.
/// </summary>
public class AdoNetUnitOfWork : IUnitOfWork
{
    private readonly IDbConnectionFactory _factory;
    private IDbConnection? _connection;
    private IDbTransaction? _transaction;

    public AdoNetUnitOfWork(IDbConnectionFactory factory)
    {
        _factory = factory;
    }

    public IDbConnection Connection => _connection ??= _factory.CreateBusinessConnection();

    public void Begin()
    {
        Connection.Open();
        _transaction = Connection.BeginTransaction();
    }

    public void Commit()
    {
        _transaction?.Commit();
        Connection.Close();
    }

    public void Rollback()
    {
        _transaction?.Rollback();
        Connection.Close();
    }
}
