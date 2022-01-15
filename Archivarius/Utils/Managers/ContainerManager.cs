using Archivarius.Algorithms;
using Archivarius.Algorithms.Huffman;
using Archivarius.Algorithms.LZW;
using Archivarius.Algorithms.SystemCompression;
using Ninject;

namespace Archivarius.Utils.Managers
{
    public static class ContainerManager
    {
        public static StandardKernel CreateStandardContainer()
        {
            var container = new StandardKernel();
            container.Bind<IFileManager>().To<FileManager>();
            container.Bind<AbstractAlgorithm>().To<AlgorithmHuffman>();
            container.Bind<AbstractAlgorithm>().To<AlgorithmLzw>();
            container.Bind<AbstractAlgorithm>().To<SystemCompressionAlgorithm>();
            
            return container;
        }
    }
}