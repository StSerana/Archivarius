using System.Collections.Generic;

namespace Archivarius.Algorithms
{
    public interface IAlgorithm
    {
        public byte[] Compress(string text, string filename);
        public Dictionary<string, byte[]> Decompress(byte[] bytes);
        public byte[] AppendFile(IEnumerable<byte> currentArchive, string additionalText, string additionalName);
    }
}