using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Archivarius.Utils.Converters;

namespace Archivarius.Algorithms.LZW
{
    public class AlgorithmLZW : AbstractAlgorithmLZW
    {
        public override byte[] Compress(string text, string filename)
        {
            // строим словарь
            var dictionary = new Dictionary<string, int>();
            for (var i = 0; i < 256; i++)
                dictionary.Add(((char)i).ToString(), i);

            var w = string.Empty;
            var compressed = new List<int>();

            foreach (var c in text)
            {
                var wc = w + c;
                if (dictionary.ContainsKey(wc))
                    w = wc;
                else
                {
                    // добавляем новый символ в архив
                    compressed.Add(dictionary[w]);
                    // добавляем новую подстроку в словарь
                    dictionary.Add(wc, dictionary.Count);
                    w = c.ToString();
                }
            }

            // дополняем оставшийся символ, если он есть
            if (!string.IsNullOrEmpty(w))
                compressed.Add(dictionary[w]);

            return Encoding.Default.GetBytes("arch_" + filename + DELIMITER)
                .Concat(compressed.SelectMany(BitConverter.GetBytes).ToArray()).ToArray();
        }

        public override Dictionary<string, byte[]> Decompress(byte[] bytes)
        {
            var compressedFiles = new Dictionary<string, byte[]>();
            var decompressedFiles = new Dictionary<string, byte[]>();
            var index = 0;
            var source = new List<byte>(bytes);
            
            while (index != -1)
            {
                index = ByteArrayConverter.ByteArrayPatternSearch(BYTES_DELIMITER, source);
                var fileName = string.Join("", Encoding.Default.GetString(source.Take(index).ToArray()));
                source = source.Skip(index + BYTES_DELIMITER.Length).ToList();
                index = ByteArrayConverter.ByteArrayPatternSearch(BYTES_DELIMITER,
                    source);
                if (index == -1)
                    compressedFiles.Add(fileName, source.ToArray());
                else
                {
                    var encodedFile = source.Take(index).ToArray();
                    source = source.Skip(index + BYTES_DELIMITER.Length).ToList();
                    compressedFiles.Add(fileName, encodedFile);
                    index = 0;
                }
            }

            foreach (var (name, file) in compressedFiles) decompressedFiles.Add("new" + name, DecompressOneFile(file));

            return decompressedFiles;
        }

        public override byte[] DecompressOneFile(byte[] bytes)
        {
            var compressed= Enumerable.Range(0, bytes.Length / 4)
                .Select(i => BitConverter.ToInt32(bytes, i * 4))
                .ToList();
            
            // построим словарь
            var dictionary = new Dictionary<int, string>();
            for (var i = 0; i < 256; i++)
                dictionary.Add(i, ((char)i).ToString());

            var w = dictionary[compressed[0]];
            compressed.RemoveAt(0);
            var decompressed = new StringBuilder(w);

            foreach (var k in compressed)
            {
                string entry = null;
                if (dictionary.ContainsKey(k))
                    entry = dictionary[k];
                else if (k == dictionary.Count)
                    entry = w + w[0];

                decompressed.Append(entry);

                // добавим новую подстроку в словарь
                if (entry == null) continue;
                dictionary.Add(dictionary.Count, w + entry[0]);

                w = entry;
            }

            // преобразуем строку в байты, записываем массива байтов в файл
            var output = Encoding.Default.GetBytes(decompressed.ToString());
            
            return output;
        }
    }
}