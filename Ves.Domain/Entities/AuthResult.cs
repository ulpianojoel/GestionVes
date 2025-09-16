#nullable enable

namespace Ves.Domain.Entities;

/// <summary>
/// Result of an authentication attempt.
/// </summary>
public class AuthResult
{
    public bool Ok { get; set; }
    public string? Token { get; set; }
    public string? Reason { get; set; }
    public int? UserId { get; set; }
}
