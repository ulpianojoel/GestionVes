using Ves.BLL.Interfaces;
using Ves.DAL.Interfaces;
using Ves.Domain.Entities;
using Ves.Services.Interfaces;

namespace Ves.BLL.Services;

/// <summary>
/// Implements client-related business rules.
/// </summary>
public class ClientService : IClientService
{
    private readonly IClientRepository _repository;
    private readonly INotificationService _notifier;
    private readonly IAuditService _audit;

    public ClientService(IClientRepository repository, INotificationService notifier, IAuditService audit)
    {
        _repository = repository;
        _notifier = notifier;
        _audit = audit;
    }

    public int Register(Client client)
    {
        var id = _repository.Add(client);
        _notifier.SendWelcome(client.Email, client.Name);
        _audit.Write("UI", "Client.Register", new { id, client.Email });
        return id;
    }
}
