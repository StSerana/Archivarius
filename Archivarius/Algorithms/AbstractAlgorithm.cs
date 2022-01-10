using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Archivarius.Algorithms
{
    public abstract class AbstractAlgorithm : IAlgorithm
    {
        protected const string Delimiter = "###";
        protected static readonly byte[] BytesDelimiter = Encoding.UTF8.GetBytes(Delimiter);
        public virtual AlgorithmType Type => AlgorithmType.Default;
        public virtual string Extension => "";
        public abstract byte[] Compress(string text, string filename);
        public abstract Dictionary<string, byte[]> Decompress(byte[] bytes);

        public byte[] AppendFile(IEnumerable<byte> currentArchive, string additionalText, string additionalName)
        {
            var encodedAdditionalText = Compress(additionalText, additionalName);
            var result = currentArchive.Concat(BytesDelimiter)
                                            .Concat(encodedAdditionalText)
                                            .ToArray();
            return result;
        }
    }
}