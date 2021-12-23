using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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

        public void Compress(string filePath, string filename)
        {
            var textFromFile = Encoding.Default.GetString(_fileManager.ReadFile($"{filePath}/{filename}"));
            Console.WriteLine(textFromFile);
            var compressed = SelectedAlgorithm.Compress(textFromFile, filename);
            var outputPath = Path.Combine(filePath, "output-" + filename);
            //  записываем массив байтов в файл, сохраняем сжатый файл
            _fileManager.WriteFile(outputPath, compressed);
            _fileManager.ChangeFileExtension(outputPath, ".archivarius");
        }

        public void AppendFile(string filePath, string filename, string currentArchiveName)
        {
            var textFromFile = Encoding.Default.GetString(_fileManager.ReadFile($"{filePath}/{filename}"));
            var archive = _fileManager.ReadFile($"{filePath}/{currentArchiveName}");
            var updatedArchive = SelectedAlgorithm.AppendFile(archive, textFromFile, filename);
            _fileManager.DeleteFile($"{filePath}/{currentArchiveName}");
            _fileManager.WriteFile($"{filePath}/{currentArchiveName}", updatedArchive);
        }

        public void Decompress(string directoryPath, string archiveName)
        {
            var fullArchivePath = Path.Combine(directoryPath, archiveName);

            var textFromFile = _fileManager.ReadFile(fullArchivePath);
            var decompressed = SelectedAlgorithm.Decompress(textFromFile);
            //  записываем массив байтов в файл, сохраняем сжатый файл
            
            foreach (var fileInfo in decompressed)
            {
                var fullFilePath = Path.Combine(directoryPath, fileInfo.Key);

                _fileManager.WriteFile(directoryPath, decompressed);
                _fileManager.ChangeFileExtension(fullFilePath, ".txt");
            }
        }
    }
}