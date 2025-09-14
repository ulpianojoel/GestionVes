namespace Ves.BLL.Interfaces;

/// <summary>
/// Handles hashing of files and persistence of the hashes.
/// </summary>
public interface IFileHashService
{
    void SaveHash(string filePath);
    void ExportHashesToFile(string destinationPath);
}
