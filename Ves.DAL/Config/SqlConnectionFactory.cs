using System.Data;
using System.Data.SqlClient;
using Ves.DAL.Interfaces;

namespace Ves.DAL.Config;

/// <summary>
/// Provides SQL Server connections for business and hash databases.
/// </summary>
public class SqlConnectionFactory : IDbConnectionFactory
{
    private readonly string _businessCs;
    private readonly string _hashCs;

    public SqlConnectionFactory(string businessCs, string hashCs)
    {
        _businessCs = businessCs;
        _hashCs = hashCs;
    }

    public IDbConnection CreateBusinessConnection() => new SqlConnection(_businessCs);

    public IDbConnection CreateHashConnection() => new SqlConnection(_hashCs);
}
