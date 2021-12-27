using System.Collections.Generic;
using System.IO;

namespace Archivarius.Utils.Managers
{
    public class FileManager : IFileManager
    {
        public byte[] ReadFile(string filePath) => File.ReadAllBytes(filePath);

        public void DeleteFile(string filepath) => File.Delete(filepath);

        public void WriteFile(string filePath, byte[] output) => File.WriteAllBytes(filePath, output);
        public void WriteFile(string filePath, Dictionary<string, byte[]> output) {

            foreach (var (name, file) in output) File.WriteAllBytes($"{filePath}/{name}", file);
        }
            
    }
}