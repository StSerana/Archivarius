using Archivarius.Algorithms;
using Archivarius.Algorithms.Huffman;
using Archivarius.Algorithms.LZW;
using NUnit.Framework;

namespace Archivarius.ArchivariusTest
{
    
    [TestFixture]
    public class AlgorithmTest
    {
        [Test]
        public void AlgorithmHuffman_Compress_NotEmpty()
        {
            Algorithm algorithm = new AlgorithmHuffman();
            TestAlgorithmCompress(algorithm, "SOVA");
        }
        
        [Test]
        public void AlgorithmLZW_Compress_NotEmpty()
        {
            Algorithm algorithm = new AlgorithmLZW();
            TestAlgorithmCompress(algorithm, "SOVA");
        }

        [Test]
        public void AlgorithmHuffman_HaveDelimiter()
        {
            Algorithm algorithm = new AlgorithmHuffman();
            var compressed = System.Text.Encoding.UTF8.GetString(algorithm.Compress("Sova", "sova.txt"));
            HaveDelimiter(compressed);
        }
        
        [Test]
        public void AlgorithmHuffman_Decompress_NotEmpty()
        {
            Algorithm algorithm = new AlgorithmHuffman();
            var compressed = algorithm.Compress("Sova", "sova.txt");
            TestAlgorithmDecompress(algorithm, compressed);
        }
        
        [Test]
        public void AlgorithmLZW_Decompress_NotEmpty()
        {
            Algorithm algorithm = new AlgorithmLZW();
            var compressed = algorithm.Compress("Sova", "sova.txt");
            TestAlgorithmDecompress(algorithm, compressed);
        }
        
        [Test]
        public void AlgorithmHuffman_Compress_Decompress_Equals()
        {
            const string text = "Sova";
            Algorithm algorithm = new AlgorithmHuffman();
            TestAlgorithmDecompressedEqualsInput(algorithm, text);
        }
        
        [Test]
        public void AlgorithmLZW_Compress_Decompress_Equals()
        {
            const string text = "Sova";
            Algorithm algorithm = new AlgorithmLZW();
            TestAlgorithmDecompressedEqualsInput(algorithm, text);
        }

        private static void TestAlgorithmDecompressedEqualsInput(Algorithm algorithm, string text)
        {
            var compressed = algorithm.Compress(text, "sova.txt");
            var decompressedBytes = algorithm.Decompress(compressed);
            var decompressed = System.Text.Encoding.UTF8.GetString(decompressedBytes["d_sova.txt"]);
            Assert.AreEqual(decompressed, text);
        }

        private static void TestAlgorithmDecompress(Algorithm algorithm, byte[] compressed)
        {
            var decompressed = algorithm.Decompress(compressed);
            Assert.IsNotNull(decompressed);
            Assert.IsNotEmpty(decompressed);
        }

        private static void TestAlgorithmCompress(Algorithm algorithm, string text)
        {
            var compressed = System.Text.Encoding.UTF8.GetString(algorithm.Compress(text, "sova.text"));
            Assert.IsNotNull(compressed);
            Assert.IsNotEmpty(compressed);
        }

        private static void HaveDelimiter(string compressed)
        {
            var haveDelimiter = compressed.Contains("###");
            Assert.AreEqual(haveDelimiter, true);
        }
    }
}