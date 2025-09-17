using System.Collections.Generic;

namespace Ves.DAL.Interfaces
{
    public interface IFileRecordRepository
    {
        void SaveHash(string filePath, string hashHex);
        IEnumerable<(string FilePath, string HashHex)> GetAll();
    }
}
