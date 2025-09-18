using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Ves.DAL.Config;
using Ves.Domain.Entities;

namespace Ves.BLL.Services
{
    public class ProductService
    {
        private readonly ISqlConnectionFactory _factory;
        public ProductService(string cs) => _factory = new SqlConnectionFactory(cs);

        public async Task<IEnumerable<Producto>> GetAllAsync()
        {
            const string sql = "SELECT Id, Nombre, PrecioUnitario, 1 AS Activo FROM Productos ORDER BY Id";
            using var cn = _factory.Create();
            using var cmd = new SqlCommand(sql, cn);
            await cn.OpenAsync();
            using var rd = await cmd.ExecuteReaderAsync();
            var list = new List<Producto>();
            while (await rd.ReadAsync())
            {
                list.Add(new Producto
                {
                    Id = rd.GetInt32(0),
                    Nombre = rd.GetString(1),
                    PrecioUnitario = rd.GetDecimal(2),
                    Activo = rd.GetInt32(3) == 1
                });
            }
            return list;
        }
    }
}
