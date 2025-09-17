using Ves.Domain.Entities;

namespace Ves.DAL.Interfaces
{
    public interface IClientRepository
    {
        int Insert(Client client);
        // agrega aquí otros métodos si los necesitás (GetById, etc.)
    }
}
