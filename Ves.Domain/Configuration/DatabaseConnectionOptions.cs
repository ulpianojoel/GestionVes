using System;

namespace Ves.Domain.Configuration
{
    public sealed class DatabaseConnectionOptions
    {
        public DatabaseConnectionOptions(string business, string hash)
        {
            Business = business;
            Hash = hash;
        }

        public string Business { get; private set; }

        public string Hash { get; private set; }

        public static bool TryCreate(string business, string hash, out DatabaseConnectionOptions options, out string errorMessage)
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
}
