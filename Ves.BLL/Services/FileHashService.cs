using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Ves.DAL.Interfaces;

namespace Ves.BLL.Services
{
    public class FileHashService
    {
        private readonly IFileRecordRepository _repo;

        public FileHashService(IFileRecordRepository repo)
        {
            _repo = repo;
        }

        public void SaveHash(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("Archivo no encontrado", filePath);

            using var stream = File.OpenRead(filePath);
            using var sha = SHA256.Create();
            var hashBytes = sha.ComputeHash(stream);
            var hashHex = Convert.ToHexString(hashBytes);

            _repo.SaveHash(filePath, hashHex);         // <-- antes: Save(path, hash)
        }

        public void ExportHashesToFile(string outputPath)
        {
            var sb = new StringBuilder();
            foreach (var item in _repo.GetAll())
            {
                // El repo devuelve (FilePath, HashHex)
                sb.AppendLine($"{item.FilePath}|{item.HashHex}");
            }
            File.WriteAllText(outputPath, sb.ToString());
        }
    }
}
