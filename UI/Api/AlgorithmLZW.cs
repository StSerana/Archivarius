using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Archivarius
{
    public class AlgorithmLZW : Algorithm
    {
        protected override string Prefix  => "l";

        public override void Compress(string inputPath, string outputPath)
        {
            var file = ReadFile(inputPath);
            var textFromFile = Encoding.Default.GetString(file);
            
            // строим словарь
            var dictionary = new Dictionary<string, int>();
            for (var i = 0; i < 256; i++)
                dictionary.Add(((char)i).ToString(), i);

            var w = string.Empty;
            var compressed = new List<int>();

            foreach (var c in textFromFile)
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

            WriteFile(outputPath, compressed.SelectMany(BitConverter.GetBytes).ToArray());
        }

        public override void Decompress(string encodedFile, string decodedFile)
        {
            var bytes = ReadFile(encodedFile);
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
            
            // сохраняем декодированный файл
            WriteFile(decodedFile, output);
        }
        
    }
}