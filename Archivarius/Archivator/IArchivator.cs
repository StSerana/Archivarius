using System.IO;
using Archivarius.Algorithms;

namespace Archivarius
{
    public interface IArchivator
    {
        public void Compress(FileInfo file, AlgorithmType algorithmType);

        public void AppendFile(FileInfo file, FileInfo currentArchive);

        public void Decompress(FileInfo archive);
    }
}