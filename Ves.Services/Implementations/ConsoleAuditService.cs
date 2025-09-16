using Ves.Services.Interfaces;

#nullable enable

namespace Ves.Services.Implementations;

/// <summary>
/// Writes audit lines to the console.
/// </summary>
public class ConsoleAuditService : IAuditService
{
    public void Write(string actor, string action, object? data = null)
        => Console.WriteLine($"AUDIT [{actor}] {action} {System.Text.Json.JsonSerializer.Serialize(data)}");
}
