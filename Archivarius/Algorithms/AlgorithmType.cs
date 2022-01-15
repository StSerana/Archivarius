using System;

namespace Archivarius.Algorithms
{
    [Flags]
    public enum AlgorithmType
    {
        Default,
        Huffman, 
        Lzw,
        Gzip,
    }
}