using Archivarius.Algorithms;
using Archivarius.Algorithms.Huffman;
using Archivarius.Algorithms.LZW;
using Ninject;

namespace Archivarius.Utils.Managers
{
    public class ContainerManager
    {
        public static StandardKernel CreateStandardContainer()
        {
            var container = new StandardKernel();
            container.Bind<IFileManager>().To<FileManager>();
            container.Bind<Algorithm>().To<AlgorithmHuffman>();
            container.Bind<Algorithm>().To<AlgorithmLZW>();
            
            return container;
        }
    }
}