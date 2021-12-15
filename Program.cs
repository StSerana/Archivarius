using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Archivarius
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World! Know you can test different compression algorithms");
            Console.WriteLine("Let's check Huffman's algorithm");
            RunHuffman();
            Console.WriteLine("Let's check LZW algorithm");
            RunLZW();
        }

        private static void RunHuffman()
        {
            for (var i = 1; i < 4; i++)
            {
                var huffman = new AlgorithmHuffman();
                try
                {
                    var filenames = huffman.GetFilenames(i);
                    huffman.Compress(filenames[0], filenames[1]);
                    huffman.Decompress(filenames[1], filenames[2]);
                }
                catch (Exception)
                {
                    Console.WriteLine("some error");
                }
                
            }
        }

        private static void RunLZW()
        {
            for (var i = 1; i < 4; i++)
            {
                var huffman = new AlgorithmLZW();
                try
                {
                    var filenames = huffman.GetFilenames(i);
                    huffman.Compress(filenames[0], filenames[1]);
                    huffman.Decompress(filenames[1], filenames[2]);
                }
                catch (Exception)
                {
                    Console.WriteLine("some error");
                }
                
            }
        }
    }
}