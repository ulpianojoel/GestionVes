using Ves.Domain.ValueObjects;

namespace Ves.Domain.Entities;

/// <summary>
/// Represents a client of the business.
/// </summary>
public class Client
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool Active { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
