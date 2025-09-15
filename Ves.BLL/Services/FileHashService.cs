using System.IO;
using System.Security.Cryptography;
using System.Text;
using Ves.BLL.Interfaces;
using Ves.DAL.Interfaces;
using Ves.Domain.Entities;

namespace Ves.BLL.Services;

/// <summary>
/// Computes hashes for files and persists them.
/// </summary>
public class FileHashService : IFileHashService
{
    private readonly IFileRecordRepository _repository;

    public FileHashService(IFileRecordRepository repository)
    {
        _repository = repository;
    }

    public void SaveHash(string filePath)
    {
        using var sha = SHA256.Create();
        var bytes = File.ReadAllBytes(filePath);
        var hash = sha.ComputeHash(bytes);
        var record = new FileRecord
        {
            Path = filePath,
            Hash = BitConverter.ToString(hash).Replace("-", string.Empty)
        };
        _repository.Save(record);
    }

    public void ExportHashesToFile(string destinationPath)
    {
        var records = _repository.GetAll();
        using var writer = new StreamWriter(destinationPath, false, Encoding.UTF8);
        foreach (var r in records)
        {
            writer.WriteLine($"{r.Path} {r.Hash}");
        }
    }
}
