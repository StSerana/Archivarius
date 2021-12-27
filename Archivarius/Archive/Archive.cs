using System;
using System.Collections.Generic;
using System.Text;
using Archivarius.Algorithms;
using Archivarius.Algorithms.Huffman;
using Archivarius.Algorithms.LZW;
using Archivarius.Utils.Managers;

namespace Archivarius
{
    public class Archive : IArchive
    {
        public Archive(IFileManager fileManager, AbstractAlgorithmLZW algorithmLzw, AbstarctAlgorithmHuffman algorithmHuffman)
        {
            _fileManager = fileManager;
            _algorithmLzw = algorithmLzw;
            _algorithmHuffman = algorithmHuffman;
        }
        
        public AlgorithmType SelectedAlgorithmKey = AlgorithmType.Lzw;
        public Algorithm SelectedAlgorithm => _algorithms[SelectedAlgorithmKey];
        
        private readonly IFileManager _fileManager;
        private static AbstractAlgorithmLZW _algorithmLzw;
        private static AbstarctAlgorithmHuffman _algorithmHuffman;

        private Dictionary<AlgorithmType, Algorithm> _algorithms = new()
       {
           {AlgorithmType.Lzw, _algorithmLzw },
           {AlgorithmType.Huffman, _algorithmHuffman }
       };

        public void Compress(string filePath, string filename)
        {
            var textFromFile = Encoding.Default.GetString(_fileManager.ReadFile($"{filePath}/{filename}"));
            Console.WriteLine(textFromFile);
            var compressed = SelectedAlgorithm.Compress(textFromFile, filename);
            //  записываем массив байтов в файл, сохраняем сжатый файл
            _fileManager.WriteFile($"{filePath}/arch_{filename}", compressed);
        }

        public void AppendFile(string filePath, string filename, string currentArchiveName)
        {
            var textFromFile = Encoding.Default.GetString(_fileManager.ReadFile($"{filePath}/{filename}"));
            var archive = _fileManager.ReadFile($"{filePath}/{currentArchiveName}");
            var updatedArchive = SelectedAlgorithm.AppendFile(archive, textFromFile, filename);
            _fileManager.DeleteFile($"{filePath}/{currentArchiveName}");
            _fileManager.WriteFile($"{filePath}/{currentArchiveName}", updatedArchive);
        }

        public void Decompress(string filepath, string inputFile)
        {
            var textFromFile = _fileManager.ReadFile($"{filepath}/{inputFile}");
            var compressed = SelectedAlgorithm.Decompress(textFromFile);
            //  записываем массив байтов в файл, сохраняем сжатый файл
            _fileManager.WriteFile(filepath, compressed);
        }
    }
}