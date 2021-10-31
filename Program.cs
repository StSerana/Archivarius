using System;

namespace Archivarius
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var huffman = new Algorithm_Huffman();
            huffman.compress();
        }
    }
}