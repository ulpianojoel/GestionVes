using Microsoft.Data.SqlClient;

namespace Ves.DAL.Interfaces
{
    public interface IDbConnectionFactory
    {
        SqlConnection CreateOpenConnection();
    }
}
