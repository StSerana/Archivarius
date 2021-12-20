using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Archivarius
{
    public class FileManager
    {
        public byte[] ReadFile(string filePath) => File.ReadAllBytes(filePath);

        public void WriteFile(string filePath, byte[] output) => File.WriteAllBytes(filePath, output);
    }
}