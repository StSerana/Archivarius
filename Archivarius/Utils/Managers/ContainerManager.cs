using Archivarius.Algorithms;
using Archivarius.Algorithms.Huffman;
using Archivarius.Algorithms.LZW;
using Ninject;

namespace Archivarius.Utils.Managers
{
    public static class ContainerManager
    {
        public static StandardKernel CreateStandardContainer()
        {
            var container = new StandardKernel();
            container.Bind<IFileManager>().To<FileManager>();
            container.Bind<AbstractAlgorithm>().To<AbstractAlgorithmHuffman>();
            container.Bind<AbstractAlgorithm>().To<AbstractAlgorithmLzw>();
            
            return container;
        }
    }
}