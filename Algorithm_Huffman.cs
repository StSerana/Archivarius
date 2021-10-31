using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Archivarius
{
    public class Algorithm_Huffman
    {
        public void compress()
        {
            var inputFileName = "/cats_input.jpg";
            var archiveName = "/cats_archive.txt";
            var outputFileName = "/cats_output.jpg";
            var pathToDirectory = "/Users/serana/RiderProjects/Archivarius/testSource";
            using (var inputStream = File.OpenRead($"{pathToDirectory}{inputFileName}"))
            {
                // преобразуем строку в байты, считываем данные, декодируем байты в строку
                var input = new byte[inputStream.Length];
                inputStream.Read(input, 0, input.Length);
                var textFromFile = Encoding.Default.GetString(input);
                
                Console.WriteLine($"Текст из файла: {textFromFile}");

                // создаем дерево Хаффмана на основе полученного файла
                var huffmanTree = new HuffmanTree();
                huffmanTree.Build(textFromFile);

                // сжимаем файл
                var encoded = huffmanTree.Encode(textFromFile);
                Console.Write("Encoded: \n");
                foreach (bool bit in encoded)
                {
                    Console.Write((bit ? 1 : 0) + "");
                }
                Console.WriteLine();
                
                // сохраняем сжатый файл
                using (var outputStream = new FileStream($"{pathToDirectory}{archiveName}", FileMode.OpenOrCreate))
                {
                    // преобразуем строку в байты, записываем массива байтов в файл
                    var output = ToByteArray(encoded);
                    outputStream.Write(output, 0, output.Length);
                    Console.WriteLine("Текст записан в файл");
                }

                // Декодируем файл
                var decoded = huffmanTree.Decode(encoded);
                Console.WriteLine("Decoded: \n" + decoded);

                // сохраняем декодированный файл
                using (var outputStream = new FileStream($"{pathToDirectory}{outputFileName}", FileMode.OpenOrCreate))
                {
                    // преобразуем строку в байты, записываем массива байтов в файл
                    var output = Encoding.Default.GetBytes(decoded);
                    outputStream.Write(output, 0, output.Length);
                    Console.WriteLine("Текст записан в файл");
                }
            }
        }
        
        private static byte[] ToByteArray(BitArray bits)
        {
            var numBytes = bits.Count / 8;
            if (bits.Count % 8 != 0) numBytes++;
            
            var bytes = new byte[numBytes];
            int byteIndex = 0, bitIndex = 0;

            for (var i = 0; i < bits.Count; i++)
            {
                if (bits[i])
                    bytes[byteIndex] |= (byte)(1 << (7 - bitIndex));

                bitIndex++;
                if (bitIndex != 8) continue;
                bitIndex = 0;
                byteIndex++;
            }
            return bytes;
        }
    }
}