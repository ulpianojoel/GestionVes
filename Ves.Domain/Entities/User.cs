using System;

namespace Ves.Domain.Entities;

/// <summary>
/// Represents a system user used for authentication purposes.
/// </summary>
public class User
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public byte[] PasswordHash { get; set; } = Array.Empty<byte>();
    public byte[] PasswordSalt { get; set; } = Array.Empty<byte>();
    public bool Active { get; set; } = true;
    public DateTime? LastLogin { get; set; }
}
