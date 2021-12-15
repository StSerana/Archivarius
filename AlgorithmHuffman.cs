using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Archivarius
{
    public class AlgorithmHuffman : Algorithm
    {
        protected override string Prefix  => "h";
        public HuffmanTree HuffmanTree { get; set; }
        
        private BitArray Result { get; set; }

        public override void Compress(string inputFile, string encodedFile)
        {
            var textFromFile = Encoding.Default.GetString(ReadFile(inputFile));
            
            // создаем дерево Хаффмана на основе полученного файла
            HuffmanTree = new HuffmanTree();
            HuffmanTree.Build(textFromFile);

            // сжимаем файл
            var encoded = HuffmanTree.Encode(textFromFile);

            Result = encoded;
            // выводим результат
            /*Console.Write("Encoded: \n");
            foreach (bool bit in encoded)
            {
                Console.Write((bit ? 1 : 0) + "");
            }
            Console.WriteLine();
            */
            // преобразуем строку в байты, записываем массива байтов в файл
            var a_bld = new StringBuilder();
            var a = Encoding.Default.GetBytes(HuffmanTree.printTree(HuffmanTree.Root, a_bld) + "###");
            var output = a.Concat(BitArrayToByteArray(encoded)).ToArray();
            
            // сохраняем сжатый файл
            WriteFile(encodedFile, output);
        }

        public Tuple<string, byte[]> FindTree(byte[] c)
        {
            var o = Encoding.Default.GetBytes("###");
            var index = 0;
            for (var i = 0; i < c.Length; i++)
            {
                if (c[i] == o[0] && c[i + 1] == o[1] && c[i + 2] == o[2])
                {
                    index = i;
                    break;
                }
            }

            var tree = string.Join("", Encoding.Default.GetString(c.Take(index).ToArray()));
            var source = c.Skip(index + 3).ToArray();
            return Tuple.Create(tree, source);
        }

        public override void Decompress(string encodedFile, string decodedFile)
        {
            var result = FindTree(ReadFile(encodedFile));
            var bits = new BitArray(result.Item2);

            //var b = HuffmanTree.TreeFromString(a).Root;
            // Декодируем файл
            var decoded = HuffmanTree.Decode(bits);
            //Console.WriteLine("Decoded: \n" + decoded);

            // преобразуем строку в байты, записываем массива байтов в файл
            var output = Encoding.Default.GetBytes(decoded);
            
            // сохраняем декодированный файл
            WriteFile(decodedFile, output);
        }

        private static byte[] BitArrayToByteArray(BitArray bits)
        {
            byte[] ret = new byte[(bits.Length - 1) / 8 + 1];
            bits.CopyTo(ret, 0);
            return ret;
        }
    }
}