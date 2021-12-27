using System.Collections.Generic;

namespace Archivarius.Utils.Managers
{
    public interface IFileManager
    {
        public byte[] ReadFile(string filePath);

        public void DeleteFile(string filepath);

        public void WriteFile(string filePath, byte[] output);
        public void WriteFile(string filePath, Dictionary<string, byte[]> output);
    }
}