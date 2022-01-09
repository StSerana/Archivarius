using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Archivarius.Utils.Converters;

namespace Archivarius.Algorithms.Huffman
{
    public class AlgorithmHuffman : Algorithm
    {
        public override string Extension => ".huf";
        public override AlgorithmType? Type  => AlgorithmType.Huffman;

        public override byte[] Compress(string text, string filename)
        {
            // создаем дерево Хаффмана на основе полученного файла
            var tree = new HuffmanTree();
            tree.Build(text);

            // сжимаем файл
            var encoded = tree.Encode(text);
            
            // преобразуем дерево в строку
            var encodedTree = Encoding.UTF8.GetBytes(HuffmanTree.TreeToString(tree.Root, new StringBuilder()) + DELIMITER);

            // преобразуем строку в байты, добавляем дерево
            var output = encodedTree.Concat(ByteArrayConverter.BitArrayToByteArray(encoded)).ToArray();
            return Encoding.UTF8.GetBytes(filename + DELIMITER).Concat(output).ToArray();
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
                var fileName = string.Join("", Encoding.UTF8.GetString(source.Take(index).ToArray()));
                source = source.Skip(index + BYTES_DELIMITER.Length).ToList();
                var treeIndex = ByteArrayConverter.ByteArrayPatternSearch(BYTES_DELIMITER, source);
                index = ByteArrayConverter.ByteArrayPatternSearch(BYTES_DELIMITER,
                    source.Skip(treeIndex + BYTES_DELIMITER.Length).ToArray());
                if (index == -1)
                    compressedFiles.Add(fileName, source.ToArray());
                else
                {
                    var encodedFile = source.Take(treeIndex + index).ToArray();
                    source = source.Skip(treeIndex + index + 2 * BYTES_DELIMITER.Length).ToList();
                    compressedFiles.Add(fileName, encodedFile);
                    index = 0;
                }
            }

            foreach (var (name, file) in compressedFiles) decompressedFiles.Add("d_" + name, DecompressOneFile(file));

            return decompressedFiles;
        }
        
        public override byte[] DecompressOneFile(byte[] bytes)
        {
            var (stringTree, encoded) = HuffmanTree.FindTree(bytes, BYTES_DELIMITER);
            var bits = new BitArray(encoded);
            var tree = HuffmanTree.TreeFromString(stringTree);
            var decoded = tree.Decode(bits);

            // преобразуем строку в байты, записываем массива байтов в файл
            var output = Encoding.UTF8.GetBytes(decoded);
            
            return output;
        }
    }
}