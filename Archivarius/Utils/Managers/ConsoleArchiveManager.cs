using System.IO;
using Archivarius.UserOptions;
using Ninject;

namespace Archivarius.Utils.Managers
{
    public class ConsoleArchiveManager
    {
        private readonly Archivator archivator;
        public ConsoleArchiveManager()
        {
            var container = ContainerManager.CreateStandardContainer();
            archivator = container.Get<Archivator>();
        }
        
        public void Create(CreateArchiveOptions archiveOptions)
        {
            var file = new FileInfo(archiveOptions.InputFile);
            archivator.Compress(file, archiveOptions.Algorithm);
        }

        public void Append(AddToArchiveOptions toArchiveOptions)
        {
            var file = new FileInfo(toArchiveOptions.InputFile);
            var archive = new FileInfo(toArchiveOptions.ArchiveFile);
            archivator.AppendFile(file, archive);
        }

        public void Decompress(DecompressArchiveOptions decompressArchiveOptions)
        {
            var archive = new FileInfo(decompressArchiveOptions.InputFile);
            archivator.Decompress(archive);
        }
    }
}