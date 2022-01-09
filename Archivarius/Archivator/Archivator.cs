using System;
using System.IO;
using System.Text;
using Archivarius.Algorithms;
using Archivarius.Utils.Managers;

namespace Archivarius
{
    public class Archivator : IArchivator
    {
        
        private readonly IFileManager _fileManager;
        private readonly AlgorithmManager _algorithmManager;

        public Archivator(IFileManager fileManager, AlgorithmManager algorithmManager)
        {
            _fileManager = fileManager;
            _algorithmManager = algorithmManager;
        }

        public void Compress(FileInfo file, AlgorithmType algorithmType)
        {
            var textFromFile = Encoding.UTF8.GetString(_fileManager.ReadFile(file.FullName));
            var algorithm = _algorithmManager.GetAlgorithmByType(algorithmType);
            var compressed = algorithm.Compress(textFromFile, file.Name);
            
            //  записываем массив байтов в файл, сохраняем сжатый файл
            _fileManager.WriteFile($"{file.DirectoryName}/{Path.GetFileNameWithoutExtension(file.Name)}{algorithm.Extension}", compressed);
            Console.WriteLine("Process finished.");
        }

        public void AppendFile(FileInfo file, FileInfo currentArchive)
        {
            var textFromFile = Encoding.UTF8.GetString(_fileManager.ReadFile(file.FullName));
            var archive = _fileManager.ReadFile(currentArchive.FullName);
            var updatedArchive = _algorithmManager.GetAlgorithmByExtension(currentArchive.Extension)
                .AppendFile(archive, textFromFile, Path.GetFileNameWithoutExtension(file.Name));
            _fileManager.DeleteFile(currentArchive.FullName);
            _fileManager.WriteFile(currentArchive.FullName, updatedArchive);
            Console.WriteLine("Process finished.");
        }

        public void Decompress(FileInfo archive)
        {
            var textFromFile = _fileManager.ReadFile(archive.FullName);
            var compressed = _algorithmManager.GetAlgorithmByExtension(archive.Extension).Decompress(textFromFile);
            //  записываем массив байтов в файл, сохраняем сжатый файл
            _fileManager.WriteFile(archive.Directory?.FullName, compressed);
            Console.WriteLine("Process finished.");
        }
    }
}