using System.Data;

namespace Ves.DAL.Interfaces;

/// <summary>
/// Provides database connections for repositories.
/// </summary>
public interface IDbConnectionFactory
{
    IDbConnection CreateBusinessConnection();
    IDbConnection CreateHashConnection();
}
