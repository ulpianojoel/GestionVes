using System;

namespace Ves.UI.Models
{
    public sealed class UserAccount
    {
        public string Username { get; set; }

        public string DisplayName { get; set; }

        public string Role { get; set; }

        public string Email { get; set; }

        public bool IsActive { get; set; }

        public DateTime LastAccessUtc { get; set; }
    }
}
