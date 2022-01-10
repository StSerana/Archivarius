using Archivarius.Algorithms;
using Archivarius.Algorithms.Huffman;
using Archivarius.Algorithms.LZW;
using NUnit.Framework;

namespace ArchivariusTest
{
    
    [TestFixture]
    public class AlgorithmTest
    {
        [Test]
        public void AlgorithmHuffman_Compress_NotEmpty()
        {
            AbstractAlgorithm abstractAlgorithm = new AlgorithmHuffman();
            TestAlgorithmCompress(abstractAlgorithm, "SOVA");
        }
        
        [Test]
        public void AlgorithmLZW_Compress_NotEmpty()
        {
            AbstractAlgorithm abstractAlgorithm = new AlgorithmLzw();
            TestAlgorithmCompress(abstractAlgorithm, "SOVA");
        }

        [Test]
        public void AlgorithmHuffman_HaveDelimiter()
        {
            AbstractAlgorithm abstractAlgorithm = new AlgorithmHuffman();
            var compressed = System.Text.Encoding.UTF8.GetString(abstractAlgorithm.Compress("Sova", "sova.txt"));
            HaveDelimiter(compressed);
        }
        
        [Test]
        public void AlgorithmHuffman_Decompress_NotEmpty()
        {
            AbstractAlgorithm abstractAlgorithm = new AlgorithmHuffman();
            var compressed = abstractAlgorithm.Compress("Sova", "sova.txt");
            TestAlgorithmDecompress(abstractAlgorithm, compressed);
        }
        
        [Test]
        public void AlgorithmLZW_Decompress_NotEmpty()
        {
            AbstractAlgorithm abstractAlgorithm = new AlgorithmLzw();
            var compressed = abstractAlgorithm.Compress("Sova", "sova.txt");
            TestAlgorithmDecompress(abstractAlgorithm, compressed);
        }
        
        [Test]
        public void AlgorithmHuffman_Compress_Decompress_Equals()
        {
            const string text = "Sova";
            AbstractAlgorithm abstractAlgorithm = new AlgorithmHuffman();
            TestAlgorithmDecompressedEqualsInput(abstractAlgorithm, text);
        }
        
        [Test]
        public void AlgorithmLZW_Compress_Decompress_Equals()
        {
            const string text = "Sova";
            AbstractAlgorithm abstractAlgorithm = new AlgorithmLzw();
            TestAlgorithmDecompressedEqualsInput(abstractAlgorithm, text);
        }

        private static void TestAlgorithmDecompressedEqualsInput(AbstractAlgorithm abstractAlgorithm, string text)
        {
            var compressed = abstractAlgorithm.Compress(text, "sova.txt");
            var decompressedBytes = abstractAlgorithm.Decompress(compressed);
            var decompressed = System.Text.Encoding.UTF8.GetString(decompressedBytes["d_sova.txt"]);
            Assert.AreEqual(decompressed, text);
        }

        private static void TestAlgorithmDecompress(AbstractAlgorithm abstractAlgorithm, byte[] compressed)
        {
            var decompressed = abstractAlgorithm.Decompress(compressed);
            Assert.IsNotNull(decompressed);
            Assert.IsNotEmpty(decompressed);
        }

        private static void TestAlgorithmCompress(AbstractAlgorithm abstractAlgorithm, string text)
        {
            var compressed = System.Text.Encoding.UTF8.GetString(abstractAlgorithm.Compress(text, "sova.text"));
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