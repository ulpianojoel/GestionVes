namespace Ves.Services.Interfaces
{
    public interface IAuditService
    {
        void Write(string title, string message, object? metadata = null);
    }
}
