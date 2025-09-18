using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Ves.Domain.Entities;

namespace Ves.DAL.Repositories
{
    public class ClientRepository
    {
        private readonly string _cs;
        public ClientRepository(string connectionString) => _cs = connectionString;

        public async Task<IEnumerable<Cliente>> GetAllAsync()
        {
            const string sql = "SELECT Id, Nombre, FechaAlta, 1 AS Activo FROM Clientes ORDER BY Id";
            using var cn = new SqlConnection(_cs);
            using var cmd = new SqlCommand(sql, cn);
            await cn.OpenAsync();
            using var rd = await cmd.ExecuteReaderAsync();

            var list = new List<Cliente>();
            while (await rd.ReadAsync())
            {
                list.Add(new Cliente
                {
                    Id = rd.GetInt32(0),
                    Nombre = rd.GetString(1),
                    FechaAlta = rd.GetDateTime(2),
                    Activo = rd.GetInt32(3) == 1
                });
            }
            return list;
        }

        public async Task InsertAsync(Cliente c)
        {
            const string sql = "INSERT INTO Clientes (Nombre, FechaAlta) VALUES (@n,@f)";
            using var cn = new SqlConnection(_cs);
            using var cmd = new SqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@n", c.Nombre);
            cmd.Parameters.AddWithValue("@f", c.FechaAlta);
            await cn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }
    }
}
