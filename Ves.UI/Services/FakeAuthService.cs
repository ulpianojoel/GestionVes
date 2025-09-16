using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Ves.UI.Models;

namespace Ves.UI.Services
{
    public interface IAuthService
    {
        bool TryAuthenticate(string username, string password, out UserAccount account, out string errorMessage);

        string BuildRecoveryMessage(string identifier);

        IReadOnlyCollection<UserAccount> GetAllUsers();
    }

    public sealed class FakeAuthService : IAuthService
    {
        private readonly List<UserAccount> _users;
        private readonly Dictionary<string, string> _passwordHashes;

        public FakeAuthService()
        {
            _users = new List<UserAccount>
            {
                new UserAccount
                {
                    Username = "admin",
                    DisplayName = "Administrador General",
                    Email = "admin@ves.local",
                    Role = "Administrador",
                    IsActive = true,
                    LastAccessUtc = DateTime.UtcNow.AddMinutes(-35)
                },
                new UserAccount
                {
                    Username = "analista",
                    DisplayName = "Analista Comercial",
                    Email = "analista@ves.local",
                    Role = "Analista",
                    IsActive = true,
                    LastAccessUtc = DateTime.UtcNow.AddHours(-4)
                },
                new UserAccount
                {
                    Username = "auditoria",
                    DisplayName = "Auditoría",
                    Email = "auditoria@ves.local",
                    Role = "Auditor",
                    IsActive = false,
                    LastAccessUtc = DateTime.UtcNow.AddDays(-2)
                }
            };

            _passwordHashes = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "admin", ComputeHash("Admin@2024") },
                { "analista", ComputeHash("Analista@2024") },
                { "auditoria", ComputeHash("Auditoria@2024") }
            };
        }

        public IReadOnlyCollection<UserAccount> GetAllUsers()
        {
            return _users;
        }

        public bool TryAuthenticate(string username, string password, out UserAccount account, out string errorMessage)
        {
            account = null;
            errorMessage = null;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                errorMessage = "Ingresá usuario y contraseña.";
                return false;
            }

            var matched = _users.FirstOrDefault(u => string.Equals(u.Username, username, StringComparison.OrdinalIgnoreCase));
            if (matched == null)
            {
                errorMessage = "El usuario no existe.";
                return false;
            }

            if (!matched.IsActive)
            {
                errorMessage = "El usuario se encuentra inhabilitado.";
                return false;
            }

            string expectedHash;
            if (!_passwordHashes.TryGetValue(matched.Username, out expectedHash))
            {
                errorMessage = "No hay contraseña configurada.";
                return false;
            }

            var providedHash = ComputeHash(password);
            if (!string.Equals(providedHash, expectedHash, StringComparison.OrdinalIgnoreCase))
            {
                errorMessage = "Contraseña incorrecta.";
                return false;
            }

            account = matched;
            errorMessage = null;
            return true;
        }

        public string BuildRecoveryMessage(string identifier)
        {
            if (string.IsNullOrWhiteSpace(identifier))
            {
                return "Ingresá tu usuario o correo registrado para continuar.";
            }

            var user = _users.FirstOrDefault(u => string.Equals(u.Username, identifier, StringComparison.OrdinalIgnoreCase)
                || string.Equals(u.Email, identifier, StringComparison.OrdinalIgnoreCase));

            if (user == null)
            {
                return "No encontramos coincidencias. Contactá a soporte para recuperar el acceso.";
            }

            return string.Format("Enviamos instrucciones de recuperación a {0}. Revisá tu bandeja de entrada.", user.Email);
        }

        private static string ComputeHash(string value)
        {
            using (var sha = SHA256.Create())
            {
                var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(value));
                var builder = new StringBuilder(bytes.Length * 2);
                foreach (var b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }

                return builder.ToString();
            }
        }
    }
}
