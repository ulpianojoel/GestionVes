using Ves.Domain.Entities;

namespace Ves.DAL.Interfaces;

/// <summary>
/// Repository abstraction for sales.
/// </summary>
public interface ISaleRepository
{
    int Add(Sale sale);
}
