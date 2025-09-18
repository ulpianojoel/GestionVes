using Microsoft.Data.SqlClient;
namespace Ves.DAL.Config
{
  public interface ISqlConnectionFactory { SqlConnection Create(); }
  public class SqlConnectionFactory : ISqlConnectionFactory
  {
    private readonly string _cs;
    public SqlConnectionFactory(string connectionString) => _cs = connectionString;
    public SqlConnection Create() => new SqlConnection(_cs);
  }
}
