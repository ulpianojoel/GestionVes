using System;

namespace Ves.Domain.Entities;

/// <summary>
/// Represents a file and its hash stored for integrity verification.
/// </summary>
public class FileRecord
{
    public int Id { get; set; }
    public string Path { get; set; } = string.Empty;
    public string Hash { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
