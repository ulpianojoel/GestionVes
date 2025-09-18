using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace Ves.BLL.Services
{
    public sealed class AuditService
    {
        private readonly string _cs;
        public AuditService(string connectionString) => _cs = connectionString;

        public async Task LogLoginAsync(string usuario, bool exitoso, string? ip = null, string? extra = null)
        {
            const string sql = @"INSERT INTO VesLog.dbo.LoginEvents (Usuario, Exitoso, Ip, Extra)
                                 VALUES (@u, @e, @ip, @x);";

            await using var cn = new SqlConnection(_cs);
            await using var cmd = new SqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@u", usuario);
            cmd.Parameters.AddWithValue("@e", exitoso);
            cmd.Parameters.AddWithValue("@ip", (object?)ip ?? System.DBNull.Value);
            cmd.Parameters.AddWithValue("@x", (object?)extra ?? System.DBNull.Value);

            await cn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task LogBusinessAsync(string? usuario, string tipo, string entidad, string entidadId, string? datosJson = null)
        {
            const string sql = @"INSERT INTO VesLog.dbo.BusinessEvents (Usuario, Tipo, Entidad, EntidadId, DatosJson)
                                 VALUES (@u, @t, @en, @id, @dj);";

            await using var cn = new SqlConnection(_cs);
            await using var cmd = new SqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@u", (object?)usuario ?? System.DBNull.Value);
            cmd.Parameters.AddWithValue("@t", tipo);
            cmd.Parameters.AddWithValue("@en", entidad);
            cmd.Parameters.AddWithValue("@id", entidadId);
            cmd.Parameters.AddWithValue("@dj", (object?)datosJson ?? System.DBNull.Value);

            await cn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }
    }
}
