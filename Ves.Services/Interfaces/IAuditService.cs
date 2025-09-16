namespace Ves.Services.Interfaces;

#nullable enable

/// <summary>
/// Writes audit information to a log sink.
/// </summary>
public interface IAuditService
{
    void Write(string actor, string action, object? data = null);
}
