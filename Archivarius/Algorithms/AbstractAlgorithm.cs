using System.Collections.Generic;
using System.Linq;
using System.Text;
using Archivarius.Utils.Converters;

namespace Archivarius.Algorithms
{
    public abstract class AbstractAlgorithm : IAlgorithm
    {
        protected const string Delimiter = "###";
        protected static readonly byte[] BytesDelimiter = Encoding.UTF8.GetBytes(Delimiter);
        public virtual AlgorithmType Type => AlgorithmType.Default;
        public virtual string Extension => "";
        public abstract byte[] Compress(string text, string filename);

        public abstract byte[] DecompressOneFile(byte[] bytes);
        public virtual Dictionary<string, byte[]> Decompress(byte[] bytes)
        {
            var decompressedFiles = new Dictionary<string, byte[]>();
            var index = 0;
            var source = new List<byte>(bytes);
            
            while (index != -1)
            {
                // находим имя сжатого файла
                index = ByteArrayConverter.ByteArrayPatternSearch(BytesDelimiter, source);
                var fileName = string.Join("", Encoding.UTF8.GetString(source.Take(index).ToArray()));
                source = source.Skip(index + BytesDelimiter.Length).ToList();
                // проверяем есть ли еще сжатые файлы
                index = ByteArrayConverter.ByteArrayPatternSearch(BytesDelimiter, source);
                if (index == -1)
                    decompressedFiles.Add("d_" + fileName, DecompressOneFile(source.ToArray()));
                else
                {
                    var encodedFile = source.Take(index).ToArray();
                    source = source.Skip(index + BytesDelimiter.Length).ToList();
                    decompressedFiles.Add("d_" + fileName, DecompressOneFile(encodedFile));
                    index = 0;
                }
            }
            
            return decompressedFiles;
        }

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