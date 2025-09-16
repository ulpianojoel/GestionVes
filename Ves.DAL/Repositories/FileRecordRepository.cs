using System;
using System.Collections.Generic;
using System.Data;
using Ves.DAL.Interfaces;
using Ves.Domain.Entities;

namespace Ves.DAL.Repositories;

/// <summary>
/// Stores file hashes in a dedicated database.
/// </summary>
public class FileRecordRepository : IFileRecordRepository
{
    private readonly IDbConnectionFactory _factory;

    public FileRecordRepository(IDbConnectionFactory factory)
    {
        _factory = factory;
    }

    public void Save(FileRecord record)
    {
        using var conn = _factory.CreateHashConnection();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "INSERT INTO FileRecords(Path,Hash,CreatedAt) VALUES(@Path,@Hash,@CreatedAt)";
        var pPath = cmd.CreateParameter(); pPath.ParameterName = "@Path"; pPath.Value = record.Path; cmd.Parameters.Add(pPath);
        var pHash = cmd.CreateParameter(); pHash.ParameterName = "@Hash"; pHash.Value = record.Hash; cmd.Parameters.Add(pHash);
        var pCreated = cmd.CreateParameter(); pCreated.ParameterName = "@CreatedAt"; pCreated.Value = record.CreatedAt; cmd.Parameters.Add(pCreated);
        conn.Open();
        cmd.ExecuteNonQuery();
    }

    public IEnumerable<FileRecord> GetAll()
    {
        using var conn = _factory.CreateHashConnection();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT Id,Path,Hash,CreatedAt FROM FileRecords";
        conn.Open();
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            yield return new FileRecord
            {
                Id = reader.GetInt32(0),
                Path = reader.GetString(1),
                Hash = reader.GetString(2),
                CreatedAt = reader.GetDateTime(3)
            };
        }
    }
}
