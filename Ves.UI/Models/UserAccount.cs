using System;

namespace Ves.UI.Models;

public sealed class UserAccount
{
    public required string Username { get; init; }

    public required string DisplayName { get; init; }

    public required string Role { get; init; }

    public required string Email { get; init; }

    public bool IsActive { get; init; }

    public DateTime LastAccessUtc { get; init; }

    public string Status => IsActive ? "Activo" : "Suspendido";
}
