namespace Ves.Services.Interfaces;

/// <summary>
/// Coordinates transactional work across repositories.
/// </summary>
public interface IUnitOfWork
{
    void Begin();
    void Commit();
    void Rollback();
}
