using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Archivarius
{
    public class FileManager
    {
        public byte[] ReadFile(string filePath)
        {
            using (var inputStream = File.OpenRead(filePath))
            {
                var input = new byte[inputStream.Length];
                inputStream.Read(input, 0, input.Length);
                return input;
            }
        }

        public void WriteFile(string filePath, byte[] output)
        {
            using (var outputStream = new FileStream(filePath, FileMode.OpenOrCreate))
            {
                outputStream.Write(output, 0, output.Length);
                Console.WriteLine("Текст записан в файл");
            }
        }
    }
}