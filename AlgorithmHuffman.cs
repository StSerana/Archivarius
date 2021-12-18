using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace Archivarius
{
    public class AlgorithmHuffman : Algorithm
    {
        protected override string Prefix  => "h";

        public override void Compress(string inputFile, string encodedFile)
        {
            var file = ReadFile(inputFile);
            var textFromFile = Encoding.Default.GetString(file);
            
            // создаем дерево Хаффмана на основе полученного файла
            var tree = new HuffmanTree();
            tree.Build(textFromFile);

            // сжимаем файл
            var encoded = tree.Encode(textFromFile);
            
            // преобразуем дерево в строку
            var encodedTree = Encoding.Default.GetBytes(HuffmanTree.TreeToString(tree.Root, new StringBuilder()) + DELIMITER);

            // преобразуем строку в байты, добавляем дерево
            var output = encodedTree.Concat(ByteArrayConverter.BitArrayToByteArray(encoded)).ToArray();
            
            //  записываем массив байтов в файл, сохраняем сжатый файл
            WriteFile(encodedFile, output);
        }

        

        

        public override void Decompress(string encodedFile, string decodedFile)
        {
            var (stringTree, encoded) = HuffmanTree.FindTree(ReadFile(encodedFile), BYTES_DELIMITER);
            var bits = new BitArray(encoded);
            var tree = HuffmanTree.TreeFromString(stringTree);
            
            // Декодируем файл
            var decoded = tree.Decode(bits);

            // преобразуем строку в байты, записываем массива байтов в файл
            var output = Encoding.Default.GetBytes(decoded);
            
            // сохраняем декодированный файл
            WriteFile(decodedFile, output);
        }
        
    }
}