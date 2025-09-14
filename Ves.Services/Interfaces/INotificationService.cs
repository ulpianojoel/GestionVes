namespace Ves.Services.Interfaces;

/// <summary>
/// Sends notifications to users.
/// </summary>
public interface INotificationService
{
    void SendWelcome(string email, string name);
}
