using System;
using System.Text.Json;
using Ves.Services.Interfaces;

namespace Ves.Services.Implementations
{
    public class ConsoleAuditService : IAuditService
    {
        public void Write(string title, string message, object? metadata = null)
        {
            Console.WriteLine($"[AUDIT] {title} :: {message}");
            if (metadata is not null)
            {
                try
                {
                    Console.WriteLine($"[AUDIT|meta] {JsonSerializer.Serialize(metadata)}");
                }
                catch
                {
                    Console.WriteLine("[AUDIT|meta] (no serializable)");
                }
            }
        }
    }
}
