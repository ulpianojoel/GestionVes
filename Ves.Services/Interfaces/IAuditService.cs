namespace Ves.Services.Interfaces;

/// <summary>
/// Writes audit information to a log sink.
/// </summary>
public interface IAuditService
{
    void Write(string actor, string action, object? data = null);
}
