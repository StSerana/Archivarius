using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Archivarius.Utils.Converters;

namespace Archivarius.Algorithms.Huffman
{
    public class AbstractAlgorithmHuffman : AbstractAlgorithm
    {
        public override string Extension => ".huf";
        public override AlgorithmType Type  => AlgorithmType.Huffman;

        public override byte[] Compress(string text, string filename)
        {
            // создаем дерево Хаффмана на основе полученного файла
            var tree = new HuffmanTree();
            tree.Build(text);

            // сжимаем файл
            var encoded = tree.Encode(text);
            
            // преобразуем дерево в строку
            var encodedTree = Encoding.UTF8.GetBytes(HuffmanTree.TreeToString(tree.Root, new StringBuilder()) + Delimiter);

            // преобразуем строку в байты, добавляем дерево
            var output = encodedTree.Concat(ByteArrayConverter.BitArrayToByteArray(encoded)).ToArray();
            
            return Encoding.UTF8.GetBytes(filename + Delimiter).Concat(output).ToArray();
        }
        
        public override Dictionary<string, byte[]> Decompress(byte[] bytes)
        {
            var decompressedFiles = new Dictionary<string, byte[]>();
            var index = 0;
            var source = new List<byte>(bytes);
            
            while (index != -1)
            {
                // находим имя сжатого файла
                index = ByteArrayConverter.ByteArrayPatternSearch(BytesDelimiter, source);
                var fileName = string.Join("", Encoding.UTF8.GetString(source.Take(index).ToArray()));
                // "обрезаем" файл и получаем местоположение дерева Хаффмана
                source = source.Skip(index + BytesDelimiter.Length).ToList();
                var treeIndex = ByteArrayConverter.ByteArrayPatternSearch(BytesDelimiter, source);
                // проверяем есть ли еще сжатые файлы
                index = ByteArrayConverter.ByteArrayPatternSearch(BytesDelimiter,
                    source.Skip(treeIndex + BytesDelimiter.Length).ToArray());
                if (index == -1)
                    decompressedFiles.Add("d_" + fileName, DecompressOneFile(source.ToArray()));
                else
                {
                    // декодируем файл и "обрезаем" архив
                    var encodedFile = source.Take(treeIndex + index).ToArray();
                    source = source.Skip(treeIndex + index + 2 * BytesDelimiter.Length).ToList();
                    decompressedFiles.Add("d_" + fileName, DecompressOneFile(encodedFile));
                    index = 0;
                }
            }

            return decompressedFiles;
        }

        private byte[] DecompressOneFile(byte[] bytes)
        {
            var (stringTree, encoded) = HuffmanTree.FindTree(bytes, BytesDelimiter);
            var bits = new BitArray(encoded);
            var tree = HuffmanTree.TreeFromString(stringTree);
            var decoded = tree.Decode(bits);
            return Encoding.UTF8.GetBytes(decoded);
        }
    }
}