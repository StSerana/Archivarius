using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Archivarius
{
    public class Archivarius
    {
        public AlgorithmType SelectedAlgorithmKey = AlgorithmType.Lzw;
        public Algorithm SelectedAlgorithm => _algorithms[SelectedAlgorithmKey];
        
        private FileManager _fileManager = new FileManager();
        private static Algorithm _algorithmLZW = new AlgorithmLZW();
        private static Algorithm _algorithmHuffman = new AlgorithmHuffman();

        private Dictionary<AlgorithmType, Algorithm> _algorithms = new()
       {
           {AlgorithmType.Lzw, _algorithmLZW },
           {AlgorithmType.Huffman, _algorithmHuffman }
       };

        public void Compress(string inputFile, string output)
        {
            var textFromFile = Encoding.Default.GetString(_fileManager.ReadFile(inputFile));
            Console.WriteLine(textFromFile);
            var compressed = SelectedAlgorithm.Compress(textFromFile);
            //  записываем массив байтов в файл, сохраняем сжатый файл
            _fileManager.WriteFile(output, compressed);
        }

        public void Decompress(string inputFile, string output)
        {
            var textFromFile = _fileManager.ReadFile(inputFile);
            var compressed = SelectedAlgorithm.Decompress(textFromFile);
            //  записываем массив байтов в файл, сохраняем сжатый файл
            _fileManager.WriteFile(output, compressed);
        }
    }
}