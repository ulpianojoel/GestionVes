using Ves.Domain.Entities;

namespace Ves.DAL.Interfaces;

/// <summary>
/// Repository abstraction for clients.
/// </summary>
public interface IClientRepository
{
    int Add(Client client);
}
