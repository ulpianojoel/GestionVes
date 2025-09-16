using System;

namespace Ves.UI.Models
{
    public sealed class OperationItem
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string Status { get; set; }

        public DateTime? DueDate { get; set; }

        public override string ToString()
        {
            return Title + " (" + Status + ")";
        }
    }
}
