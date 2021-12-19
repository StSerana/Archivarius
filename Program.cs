using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Archivarius
{
    class Program
    {
        private static Archivarius _archivarius = new Archivarius();
        private static readonly List<string> Filenames  = new() {"input.txt", "arch.txt", "out.txt"};
        private const string PathToDirectory = "/Users/stanislavilin/RiderProjects/Archivarius/testSource";

        private static void Main(string[] args)
        {
            Console.WriteLine("Hello World! Know you can test different compression algorithms");
            
            Console.WriteLine("Let's check Huffman's algorithm");
            _archivarius.SelectedAlgorithmKey = AlgorithmType.Huffman;
            RunCurrentAlgorithm();
            
            Console.WriteLine("Let's check LZW algorithm");
            _archivarius.SelectedAlgorithmKey = AlgorithmType.Lzw;
            RunCurrentAlgorithm();
        }

        private static void RunCurrentAlgorithm()
        {
            for (var i = 1; i < 2; i++)
            {
                try
                {
                    var filenames = GetFilenames(_archivarius.SelectedAlgorithm.Prefix, i);
                    
                    _archivarius.Compress($"{PathToDirectory}{filenames[0]}", $"{PathToDirectory}{filenames[1]}");
                    _archivarius.Decompress($"{PathToDirectory}{filenames[1]}", $"{PathToDirectory}{filenames[2]}");
                }
                catch (Exception e)
                {
                    Console.WriteLine("some error");
                    Console.WriteLine(e);
                }
                
            }
        }

        private static List<string> GetFilenames(string prefix, int testNumber) => (from name in Filenames let testPrefix = $"/{prefix}_{testNumber}_" select testPrefix + name).ToList();
    }
}