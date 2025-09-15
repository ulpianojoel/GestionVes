using System.Collections.Generic;
using Ves.Domain.Entities;

namespace Ves.DAL.Interfaces;

/// <summary>
/// Repository for storing file hashes.
/// </summary>
public interface IFileRecordRepository
{
    void Save(FileRecord record);
    IEnumerable<FileRecord> GetAll();
}
