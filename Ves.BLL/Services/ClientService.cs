using Ves.DAL.Interfaces;
using Ves.Domain.Entities;
using Ves.Services.Interfaces;

namespace Ves.BLL.Services
{
    public class ClientService
    {
        private readonly IClientRepository _repo;
        private readonly INotificationService _notifier;
        private readonly IAuditService _audit;

        public ClientService(IClientRepository repo, INotificationService notifier, IAuditService audit)
        {
            _repo = repo;
            _notifier = notifier;
            _audit = audit;
        }

        public int Register(Client client)
        {
            var id = _repo.Insert(client);             // <-- antes: Add()
            _notifier.SendWelcome(client.Name, client.Email);
            _audit.Write("ClientRegistered", $"Client {client.Name} ({client.Email}) registered with id {id}");
            return id;
        }
    }
}
