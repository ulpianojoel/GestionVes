using System;
using Ves.Services.Interfaces;

namespace Ves.Services.Implementations
{
    public class ConsoleNotificationService : INotificationService
    {
        public void SendWelcome(string name, string email)
        {
            Console.WriteLine($"[NOTIFY] Bienvenido {name}. Email: {email}");
        }
    }
}
