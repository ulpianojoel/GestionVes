using Microsoft.Data.SqlClient;
using Ves.DAL.Interfaces;
using Ves.Domain.Entities;

namespace Ves.DAL.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly IDbConnectionFactory _factory;

        public ClientRepository(IDbConnectionFactory factory)
        {
            _factory = factory;
        }

        public int Insert(Client client)
        {
            using SqlConnection conn = _factory.CreateOpenConnection();
            using var cmd = new SqlCommand(
                "INSERT INTO Clients (Name, Email) OUTPUT INSERTED.Id VALUES (@Name, @Email)", conn);
            cmd.Parameters.AddWithValue("@Name", client.Name);
            cmd.Parameters.AddWithValue("@Email", client.Email);
            return (int)cmd.ExecuteScalar();
        }
    }
}
