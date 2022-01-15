using System.IO;
using System.IO.Compression;
using System.Text;
using NLog;

namespace Archivarius.Algorithms.SystemCompression
{
    
    
    public class SystemCompressionAlgorithm : AbstractAlgorithm
    {
        
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        
        public override string Extension => ".zip";

        public override AlgorithmType Type  => AlgorithmType.Gzip;
        
        public override byte[] Compress(string text, string filename) => _Compress(text, filename);

        private static byte[] _Compress(string text, string filename)
        {
            using (var outStream = new MemoryStream())
            {
                using (var tinyStream = new GZipStream(outStream, CompressionMode.Compress))
                using (var mStream = new MemoryStream(Encoding.UTF8.GetBytes(filename + Delimiter + text)))
                    mStream.CopyTo(tinyStream);
                
                return outStream.ToArray();
            }
        }

        public override byte[] DecompressOneFile(byte[] bytes)
        {
            using (var inStream = new MemoryStream(bytes))
            using (var bigStream = new GZipStream(inStream, CompressionMode.Decompress))
            using (var bigStreamOut = new MemoryStream())
            {
                bigStream.CopyTo(bigStreamOut);
                return bigStreamOut.ToArray();
            }
        }
    }
}