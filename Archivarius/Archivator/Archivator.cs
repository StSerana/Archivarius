using System;
using System.IO;
using System.Text;
using Archivarius.Algorithms;
using Archivarius.Utils.Managers;
using NLog;

namespace Archivarius
{
    public class Archivator : IArchivator
    {
        
        private readonly IFileManager _fileManager;
        private readonly AlgorithmManager _algorithmManager;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public Archivator(IFileManager fileManager, AlgorithmManager algorithmManager)
        {
            _fileManager = fileManager;
            _algorithmManager = algorithmManager;
        }

        public void Compress(FileInfo file, AlgorithmType algorithmType)
        {
            logger.Info($"Started reading {file.Name}");

            var textFromFile = Encoding.UTF8.GetString(_fileManager.ReadFile(file.FullName));
            var algorithm = _algorithmManager.GetAlgorithmByType(algorithmType);

            logger.Info($"Started compressing {file.Name} via {algorithmType}");

            var compressed = algorithm.Compress(textFromFile, file.Name);

            logger.Info($"Successfully compressed {file.Name}. Writing output into {Path.GetFileNameWithoutExtension(file.Name)}{algorithm.Extension}");

            //  записываем массив байтов в файл, сохраняем сжатый файл
            _fileManager.WriteFile($"{file.DirectoryName}/{Path.GetFileNameWithoutExtension(file.Name)}{algorithm.Extension}", compressed);
            Console.WriteLine("Process finished.");
        }

        public void AppendFile(FileInfo file, FileInfo currentArchive)
        {
            logger.Info($"Started reading {file.Name}, {currentArchive.Name}");

            var textFromFile = Encoding.UTF8.GetString(_fileManager.ReadFile(file.FullName));
            var archive = _fileManager.ReadFile(currentArchive.FullName);

            logger.Info($"Started appending archive {currentArchive.Name} with {file.Name}");

            var updatedArchive = _algorithmManager.GetAlgorithmByExtension(currentArchive.Extension)
                .AppendFile(archive, textFromFile, Path.GetFileNameWithoutExtension(file.Name));

            logger.Info($"Successfully appended. Deleting {currentArchive.Name}");            

            _fileManager.DeleteFile(currentArchive.FullName);

            logger.Info($"Successfully deleted. Writing {currentArchive.Name}");

            _fileManager.WriteFile(currentArchive.FullName, updatedArchive);
            Console.WriteLine("Process finished.");
        }

        public void Decompress(FileInfo archive)
        {
            logger.Info($"Started reading {archive.Name}");

            var textFromFile = _fileManager.ReadFile(archive.FullName);

            logger.Info($"Started decompressing {archive.Name}");

            var compressed = _algorithmManager.GetAlgorithmByExtension(archive.Extension).Decompress(textFromFile);
            //  записываем массив байтов в файл, сохраняем сжатый файл

            logger.Info($"Successfully extracted {compressed.Keys.Count} files from {archive.Name}");

            _fileManager.WriteFile(archive.Directory?.FullName, compressed);
            Console.WriteLine("Process finished.");
        }
    }
}