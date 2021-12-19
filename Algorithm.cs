using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Archivarius
{
    public abstract class Algorithm
    {
        private readonly List<string> _filenames  = new() {"input.txt", "archive.txt", "output.txt"};
        private const string PathToDirectory = "/Users/serana/RiderProjects/Archivarius/testSource";

        protected const string DELIMITER = "###";
        protected static readonly byte[] BYTES_DELIMITER = Encoding.Default.GetBytes(DELIMITER);
        protected virtual string Prefix => "";
        public abstract void Compress(string inputFile, string encodedFile);
        public abstract void Decompress(string encodedFile, string decodedFile);

        protected static byte[] ReadFile(string filename)
        {
            /*using (var inputStream = File.OpenRead($"{PathToDirectory}{filename}"))
            {
                var input = new byte[inputStream.Length];
                inputStream.Read(input, 0, input.Length);
                return input;
            }*/
            return File.ReadAllBytes($"{PathToDirectory}{filename}");;
        }

        protected static void WriteFile(string filename, byte[] output)
        {
            using (var outputStream = new FileStream($"{PathToDirectory}{filename}", FileMode.OpenOrCreate))
            {
                outputStream.Write(output, 0, output.Length);
                Console.WriteLine("Текст записан в файл");
            }
        }
        
        public List<string> GetFilenames(int testNumber) => (from name in _filenames let prefix = $"/{Prefix}_{testNumber}_" select prefix + name).ToList();
    }
}