using System;

namespace Ves.UI.Models
{
    public sealed class NotificationMessage
    {
        public string Title { get; set; }

        public string Detail { get; set; }

        public DateTime CreatedUtc { get; set; }

        public string RelativeTime
        {
            get
            {
                var minutes = (int)Math.Max(1, (DateTime.UtcNow - CreatedUtc).TotalMinutes);
                return "hace " + minutes + " min";
            }
        }
    }
}
