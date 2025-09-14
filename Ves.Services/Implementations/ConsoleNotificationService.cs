using Ves.Services.Interfaces;

namespace Ves.Services.Implementations;

/// <summary>
/// Simplistic notification service that writes to console.
/// </summary>
public class ConsoleNotificationService : INotificationService
{
    public void SendWelcome(string email, string name)
        => Console.WriteLine($"Welcome {name}! ({email})");
}
