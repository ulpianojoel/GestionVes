using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Ves.DAL.Config;
using Ves.Domain.Entities;

namespace Ves.BLL.Services
{
    public class ClientService
    {
        private readonly ISqlConnectionFactory _factory;
        public ClientService(string cs) => _factory = new SqlConnectionFactory(cs);

        public async Task<IEnumerable<Cliente>> GetAllAsync()
        {
            const string sql = "SELECT Id, Nombre, GETDATE() as FechaAlta, 1 as Activo FROM Clientes ORDER BY Id";
            var list = new List<Cliente>();
            using var cn = _factory.Create();
            using var cmd = new SqlCommand(sql, cn);
            await cn.OpenAsync();
            using var rd = await cmd.ExecuteReaderAsync();
            while (await rd.ReadAsync())
            {
                list.Add(new Cliente
                {
                    Id = rd.GetInt32(0),
                    Nombre = rd.GetString(1),
                    FechaAlta = System.DateTime.Now,
                    Activo = true
                });
            }
            return list;
        }

        public async Task InsertAsync(Cliente c)
        {
            const string sql = "INSERT INTO Clientes (Nombre, FechaAlta) VALUES (@n, GETDATE())";
            using var cn = _factory.Create();
            using var cmd = new SqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@n", c.Nombre);
            await cn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }
    }
}
