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
        
        private readonly IFileManager fileManager;
        public readonly AlgorithmManager AlgorithmManager;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public Archivator(IFileManager fileManager, AlgorithmManager algorithmManager)
        {
            this.fileManager = fileManager;
            AlgorithmManager = algorithmManager;
        }

        public void Compress(FileInfo file, AlgorithmType algorithmType)
        {
            Logger.Info($"Started reading {file.Name}");

            var textFromFile = Encoding.UTF8.GetString(fileManager.ReadFile(file.FullName));
            
            Logger.Info($"Started compressing {file.Name} via {algorithmType}");
            var algorithm = AlgorithmManager.GetAlgorithmByType(algorithmType);
            var compressed = algorithm.Compress(textFromFile, file.Name);

            Logger.Info($"Successfully compressed {file.Name}. Writing output into {Path.GetFileNameWithoutExtension(file.Name)}{algorithm.Extension}");

            //  записываем массив байтов в файл, сохраняем сжатый файл
            var filepath = Path.Combine(file.DirectoryName!,
                Path.GetFileNameWithoutExtension(file.Name) + algorithm.Extension);
            fileManager.WriteFile(filepath, compressed);
        }

        public void AppendFile(FileInfo file, FileInfo currentArchive)
        {
            Logger.Info($"Started reading {file.Name}, {currentArchive.Name}");

            var textFromFile = Encoding.UTF8.GetString(fileManager.ReadFile(file.FullName));
            var archive = fileManager.ReadFile(currentArchive.FullName);

            Logger.Info($"Started appending archive {currentArchive.Name} with {file.Name}");

            var updatedArchive = AlgorithmManager.GetAlgorithmByExtension(currentArchive.Extension)
                .AppendFile(archive, textFromFile, file.Name);

            Logger.Info($"Successfully appended. Deleting {currentArchive.Name}");            

            fileManager.DeleteFile(currentArchive.FullName);

            Logger.Info($"Successfully deleted. Writing {currentArchive.Name}");

            fileManager.WriteFile(currentArchive.FullName, updatedArchive);
        }

        public void Decompress(FileInfo archive)
        {
            Logger.Info($"Started reading {archive.Name}");

            var textFromFile = fileManager.ReadFile(archive.FullName);

            Logger.Info($"Started decompressing {archive.Name}");

            var compressed = AlgorithmManager.GetAlgorithmByExtension(archive.Extension)
                .Decompress(textFromFile);

            Logger.Info($"Successfully extracted {compressed.Keys.Count} files from {archive.Name}");

            fileManager.WriteFile(archive.Directory?.FullName, compressed);
        }
    }
}