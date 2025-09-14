using Ves.Domain.Entities;

namespace Ves.BLL.Interfaces;

/// <summary>
/// Business operations related to clients.
/// </summary>
public interface IClientService
{
    int Register(Client client);
}
