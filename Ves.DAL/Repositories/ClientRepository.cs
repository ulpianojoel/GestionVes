using System.Data;
using Ves.DAL.Interfaces;
using Ves.Domain.Entities;

namespace Ves.DAL.Repositories;

/// <summary>
/// ADO.NET implementation of <see cref="IClientRepository"/>.
/// </summary>
public class ClientRepository : IClientRepository
{
    private readonly IDbConnectionFactory _factory;

    public ClientRepository(IDbConnectionFactory factory)
    {
        _factory = factory;
    }

    public int Add(Client client)
    {
        using var conn = _factory.CreateBusinessConnection();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "INSERT INTO Clients(Name,Email,Active,CreatedAt) OUTPUT INSERTED.ID VALUES(@Name,@Email,@Active,@CreatedAt)";
        var pName = cmd.CreateParameter(); pName.ParameterName = "@Name"; pName.Value = client.Name; cmd.Parameters.Add(pName);
        var pEmail = cmd.CreateParameter(); pEmail.ParameterName = "@Email"; pEmail.Value = client.Email; cmd.Parameters.Add(pEmail);
        var pActive = cmd.CreateParameter(); pActive.ParameterName = "@Active"; pActive.Value = client.Active; cmd.Parameters.Add(pActive);
        var pCreated = cmd.CreateParameter(); pCreated.ParameterName = "@CreatedAt"; pCreated.Value = client.CreatedAt; cmd.Parameters.Add(pCreated);
        conn.Open();
        return (int)(cmd.ExecuteScalar() ?? 0);
    }
}
