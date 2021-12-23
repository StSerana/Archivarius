using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Archivarius
{
    public abstract class Algorithm
    {
        protected const string DELIMITER = "###";
        protected static readonly byte[] BYTES_DELIMITER = Encoding.Default.GetBytes(DELIMITER);
        public virtual string Prefix => "";
        public abstract byte[] Compress(string text, string filename);
        public abstract Dictionary<string, byte[]> Decompress(byte[] bytes);
        public abstract byte[] DecompressOneFile(byte[] bytes);
        public byte[] AppendFile(IEnumerable<byte> currentArchive, string additionalText, string additionalName)
        {
            var encodedAdditionalText = Compress(additionalText, additionalName);
            var result = currentArchive.Concat(BYTES_DELIMITER)
                                            .Concat(encodedAdditionalText)
                                            .ToArray();
            return result;
        }
    }
}