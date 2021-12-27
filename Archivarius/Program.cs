using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Archivarius.Algorithms;
using Archivarius.Algorithms.Huffman;
using Archivarius.Algorithms.LZW;
using Archivarius.Utils.Managers;
using Ninject;
using Ninject.Syntax;

namespace Archivarius
{
    class Program
    {
        private static readonly List<string> Filenames  = new() {"input.txt", "arch.txt", "out.txt"};
        private const string PathToDirectory = "/Users/serana/RiderProjects/Archivarius/ArchivariusTest/TestSource";

        private static void Main(string[] args)
        {
            var container = new StandardKernel();
            container.Bind<IFileManager>().To<FileManager>();
            container.Bind<AbstarctAlgorithmHuffman>().To<AlgorithmHuffman>();
            container.Bind<AbstractAlgorithmLZW>().To<AlgorithmLZW>();
            
            var archivarius = container.Get<Archive>();

            Console.WriteLine("Hello World! Know you can test different compression algorithms");
            
            Console.WriteLine("Let's check Huffman's algorithm");
            archivarius.SelectedAlgorithmKey = AlgorithmType.Huffman;
            RunCurrentAlgorithm(archivarius);
            
            Console.WriteLine("Let's check LZW algorithm");
            archivarius.SelectedAlgorithmKey = AlgorithmType.Lzw;
            //RunCurrentAlgorithm();
        }

        private static void RunCurrentAlgorithm(IArchive archivarius)
        {
            for (var i = 1; i < 2; i++)
            {
                try
                {
                    var filenames = GetFilenames(archivarius.SelectedAlgorithm.Prefix, i);
                    var additionalFilenames = GetFilenames(archivarius.SelectedAlgorithm.Prefix, i + 1);
                    archivarius.Compress(PathToDirectory, filenames[0]);
                    archivarius.AppendFile(PathToDirectory, additionalFilenames[0], "arch_" + filenames[0]);
                    archivarius.Decompress(PathToDirectory, "arch_" + filenames[0]);
                }
                catch (Exception e)
                {
                    Console.WriteLine("some error");
                    Console.WriteLine(e);
                }
                
            }
        }

        private static List<string> GetFilenames(string prefix, int testNumber) => (from name in Filenames let testPrefix = $"{prefix}_{testNumber}_" select testPrefix + name).ToList();
    }
}