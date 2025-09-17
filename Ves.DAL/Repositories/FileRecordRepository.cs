using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using Ves.DAL.Interfaces;

namespace Ves.DAL.Repositories
{
    public class FileRecordRepository : IFileRecordRepository
    {
        private readonly IDbConnectionFactory _factory;

        public FileRecordRepository(IDbConnectionFactory factory)
        {
            _factory = factory;
        }

        public void SaveHash(string filePath, string hashHex)
        {
            using SqlConnection conn = _factory.CreateOpenConnection();
            using var cmd = new SqlCommand(
                "INSERT INTO FileHashes (FilePath, Hash) VALUES (@Path, @Hash)", conn);
            cmd.Parameters.AddWithValue("@Path", filePath);
            cmd.Parameters.AddWithValue("@Hash", hashHex);
            cmd.ExecuteNonQuery();
        }

        public IEnumerable<(string FilePath, string HashHex)> GetAll()
        {
            using SqlConnection conn = _factory.CreateOpenConnection();
            using var cmd = new SqlCommand("SELECT FilePath, Hash FROM FileHashes", conn);
            using var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                yield return (rdr.GetString(0), rdr.GetString(1));
            }
        }
    }
}
