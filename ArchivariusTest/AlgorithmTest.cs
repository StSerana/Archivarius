using Archivarius.Algorithms;
using Archivarius.Algorithms.Huffman;
using Archivarius.Algorithms.LZW;
using Archivarius.Algorithms.SystemCompression;
using NUnit.Framework;

namespace ArchivariusTest
{
    
    [TestFixture]
    public class AlgorithmTest
    {
        [Test]
        public void AlgorithmSystemCompression_Compress_NotEmpty()
        {
            AbstractAlgorithm abstractAlgorithm = new SystemCompressionAlgorithm();
            TestAlgorithmCompress(abstractAlgorithm, "Contrary to popular belief, Lorem Ipsum is not simply random text. It has roots in a piece of classical Latin literature from 45 BC, making it over 2000 years old. Richard McClintock, a Latin professor at Hampden-Sydney College in Virginia, looked up one of the more obscure Latin words, consectetur, from a Lorem Ipsum passage, and going through the cites of the word in classical literature, discovered the undoubtable source. Lorem Ipsum comes from sections 1.10.32 and 1.10.33 of 'de Finibus Bonorum et Malorum' (The Extremes of Good and Evil) by Cicero, written in 45 BC. This book is a treatise on the theory of ethics, very popular during the Renaissance. The first line of Lorem Ipsum, 'Lorem ipsum dolor sit amet..', comes from a line in section 1.10.32.The standard chunk of Lorem Ipsum used since the 1500s is reproduced below for those interested. Sections 1.10.32 and 1.10.33 from 'de Finibus Bonorum et Malorum' by Cicero are also reproduced in their exact original form, accompanied by English versions from the 1914 translation by H. Rackham.");
        }
        
        [Test]
        public void AlgorithmHuffman_Compress_NotEmpty()
        {
            AbstractAlgorithm abstractAlgorithm = new AlgorithmHuffman();
            TestAlgorithmCompress(abstractAlgorithm, "Contrary to popular belief, Lorem Ipsum is not simply random text. It has roots in a piece of classical Latin literature from 45 BC, making it over 2000 years old. Richard McClintock, a Latin professor at Hampden-Sydney College in Virginia, looked up one of the more obscure Latin words, consectetur, from a Lorem Ipsum passage, and going through the cites of the word in classical literature, discovered the undoubtable source. Lorem Ipsum comes from sections 1.10.32 and 1.10.33 of 'de Finibus Bonorum et Malorum' (The Extremes of Good and Evil) by Cicero, written in 45 BC. This book is a treatise on the theory of ethics, very popular during the Renaissance. The first line of Lorem Ipsum, 'Lorem ipsum dolor sit amet..', comes from a line in section 1.10.32.The standard chunk of Lorem Ipsum used since the 1500s is reproduced below for those interested. Sections 1.10.32 and 1.10.33 from 'de Finibus Bonorum et Malorum' by Cicero are also reproduced in their exact original form, accompanied by English versions from the 1914 translation by H. Rackham.");
        }
    
        
        [Test]
        public void AlgorithmLZW_Compress_NotEmpty()
        {
            AbstractAlgorithm abstractAlgorithm = new AlgorithmLzw();
            TestAlgorithmCompress(abstractAlgorithm, 
                "Contrary to popular belief, Lorem Ipsum is not simply random text. It has roots in a piece of classical Latin literature from 45 BC, making it over 2000 years old. Richard McClintock, a Latin professor at Hampden-Sydney College in Virginia, looked up one of the more obscure Latin words, consectetur, from a Lorem Ipsum passage, and going through the cites of the word in classical literature, discovered the undoubtable source. Lorem Ipsum comes from sections 1.10.32 and 1.10.33 of 'de Finibus Bonorum et Malorum' (The Extremes of Good and Evil) by Cicero, written in 45 BC. This book is a treatise on the theory of ethics, very popular during the Renaissance. The first line of Lorem Ipsum, 'Lorem ipsum dolor sit amet..', comes from a line in section 1.10.32.The standard chunk of Lorem Ipsum used since the 1500s is reproduced below for those interested. Sections 1.10.32 and 1.10.33 from 'de Finibus Bonorum et Malorum' by Cicero are also reproduced in their exact original form, accompanied by English versions from the 1914 translation by H. Rackham.");
        }

        [Test]
        public void AlgorithmHuffman_HaveDelimiter()
        {
            AlgorithmHaveDelimiter( new AlgorithmHuffman());
        }
        
        [Test]
        public void SystemCompressionAlgorithm_HaveDelimiter()
        {
            AlgorithmHaveDelimiter( new SystemCompressionAlgorithm());
        }

        [Test]
        public void AlgorithmLzw_HaveDelimiter()
        {
            AlgorithmHaveDelimiter( new AlgorithmLzw());
        }

        
        private static void AlgorithmHaveDelimiter(AbstractAlgorithm algorithm)
        {
            var compressed = System.Text.Encoding.UTF8.GetString(algorithm.Compress("Sova", "sova.txt"));
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
        public void SystemCompressionAlgorithmDecompress_NotEmpty()
        {
            AbstractAlgorithm abstractAlgorithm = new SystemCompressionAlgorithm();
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