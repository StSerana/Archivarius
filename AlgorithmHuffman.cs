using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace Archivarius
{
    public class AlgorithmHuffman : Algorithm
    {
        public override string Prefix  => "h";
        private string encodestr = "";
        private string decodestr = "";

        public override byte[] Compress(string text)
        {
            // создаем дерево Хаффмана на основе полученного файла
            var tree = new HuffmanTree();
            tree.Build(text);

            // сжимаем файл
            var encoded = tree.Encode(text);
            
            // преобразуем дерево в строку
            encodestr = HuffmanTree.TreeToString(tree.Root, new StringBuilder()).ToString();
            var encodedTree = Encoding.Default.GetBytes(HuffmanTree.TreeToString(tree.Root, new StringBuilder()) + DELIMITER);

            // преобразуем строку в байты, добавляем дерево
            var output = encodedTree.Concat(ByteArrayConverter.BitArrayToByteArray(encoded)).ToArray();
            return output;
        }
        
        public override byte[] Decompress(byte[] bytes)
        {
            var (stringTree, encoded) = HuffmanTree.FindTree(bytes, BYTES_DELIMITER);
            var bits = new BitArray(encoded);
            var tree = HuffmanTree.TreeFromString(stringTree);
            decodestr = stringTree;
            // Декодируем файл
            var decoded = tree.Decode(bits);

            // преобразуем строку в байты, записываем массива байтов в файл
            var output = Encoding.Default.GetBytes(decoded);
            
            return output;
        }
        
    }
}