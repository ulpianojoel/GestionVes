using System;
using System.Diagnostics.CodeAnalysis;

namespace Ves.Domain.Configuration;

public sealed record DatabaseConnectionOptions(string Business, string Hash)
{
    public static bool TryCreate(
        string? business,
        string? hash,
        [NotNullWhen(true)] out DatabaseConnectionOptions? options,
        out string? errorMessage)
    {
        if (string.IsNullOrWhiteSpace(business))
        {
            errorMessage = "La cadena de conexión 'Business' no está configurada.";
            options = null;
            return false;
        }

        if (string.IsNullOrWhiteSpace(hash))
        {
            errorMessage = "La cadena de conexión 'Hash' no está configurada.";
            options = null;
            return false;
        }

        options = new DatabaseConnectionOptions(business, hash);
        errorMessage = null;
        return true;
    }
}
