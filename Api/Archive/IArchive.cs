using Archivarius.Algorithms;

namespace Archivarius
{
    public interface IArchive
    {
        Algorithm SelectedAlgorithm { get; }
        AlgorithmType SelectedAlgorithmKey { set; }

        public void Compress(string filePath, string filename);

        public void AppendFile(string filePath, string filename, string currentArchiveName);

        public void Decompress(string filepath, string inputFile);
    }
}