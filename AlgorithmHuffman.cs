using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Archivarius
{
    public class AlgorithmHuffman : Algorithm
    {
        protected override string Prefix  => "h";
        private HuffmanTree HuffmanTree { get; set; }
        
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
            var output = BitArrayToByteArray(encoded);
            
            // сохраняем сжатый файл
            WriteFile(encodedFile, output);
        }

        public override void Decompress(string encodedFile, string decodedFile)
        {
            var bits = new BitArray(ReadFile(encodedFile));

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