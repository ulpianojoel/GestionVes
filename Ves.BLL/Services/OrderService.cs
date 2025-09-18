using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Ves.DAL.Config;
using Ves.Domain.Entities;

namespace Ves.BLL.Services
{
    public class OrderService
    {
        private readonly ISqlConnectionFactory _factory;
        public OrderService(string cs) => _factory = new SqlConnectionFactory(cs);

        public async Task<int> CreateAsync(Order o)
        {
            using var cn = _factory.Create();
            await cn.OpenAsync();
            using var tx = await cn.BeginTransactionAsync();
            try
            {
                const string sqlP = "INSERT INTO Pedidos (ClienteId, Fecha, Observaciones) OUTPUT INSERTED.Id VALUES (@c,@f,@obs);";
                using (var cmd = new SqlCommand(sqlP, cn, (SqlTransaction)tx))
                {
                    cmd.Parameters.AddWithValue("@c", o.ClienteId);
                    cmd.Parameters.AddWithValue("@f", o.Fecha);
                    cmd.Parameters.AddWithValue("@obs", (object?)o.Observaciones ?? System.DBNull.Value);
                    var idObj = await cmd.ExecuteScalarAsync();
                    o.Id = (idObj is int i) ? i : System.Convert.ToInt32(idObj);
                }

                const string sqlI = "INSERT INTO PedidoItems (PedidoId, ProductoId, Cantidad, PrecioUnitario) VALUES (@p,@prod,@cant,@price);";
                foreach (var it in o.Items)
                {
                    using var cmdI = new SqlCommand(sqlI, cn, (SqlTransaction)tx);
                    cmdI.Parameters.AddWithValue("@p", o.Id);
                    cmdI.Parameters.AddWithValue("@prod", it.ProductoId);
                    cmdI.Parameters.AddWithValue("@cant", it.Cantidad);
                    cmdI.Parameters.AddWithValue("@price", it.PrecioUnitario);
                    await cmdI.ExecuteNonQueryAsync();
                }

                await tx.CommitAsync();
                return o.Id;
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }
        }

        public async Task<decimal> GetTotalAsync(int pedidoId)
        {
            const string sql = "SELECT SUM(Cantidad*PrecioUnitario) FROM PedidoItems WHERE PedidoId=@p;";
            using var cn = _factory.Create();
            using var cmd = new SqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@p", pedidoId);
            await cn.OpenAsync();
            var obj = await cmd.ExecuteScalarAsync();
            if (obj == null || obj is System.DBNull) return 0m;
            return (obj is decimal d) ? d : System.Convert.ToDecimal(obj);
        }
    }
}
