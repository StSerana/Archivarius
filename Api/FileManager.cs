using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Archivarius
{
    public class FileManager
    {
        public byte[] ReadFile(string filePath) => File.ReadAllBytes(filePath);

        public void DeleteFile(string filepath) => File.Delete(filepath);

        public void WriteFile(string filePath, byte[] output) => File.WriteAllBytes(filePath, output);
        public void WriteFile(string filePath, Dictionary<string, byte[]> output) {
            foreach (var (name, file) in output) File.WriteAllBytes($"{filePath}/{name}", file);
        }

        public void ChangeFileExtension(string filePath, string targetExtension)
        {
            var file = new FileInfo(filePath);
            var newFileName = file.Name.Replace(file.Extension, targetExtension);
            var newFilePath = Path.Combine(file.DirectoryName, newFileName);

            File.Move(filePath, newFilePath);
        }
            
    }
}