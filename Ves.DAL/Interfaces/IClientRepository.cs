using Ves.Domain.Entities;

namespace Ves.DAL.Interfaces
{
    public interface IClientRepository
    {
        int Insert(Client client);
        // agrega aqu� otros m�todos si los necesit�s (GetById, etc.)
    }
}
