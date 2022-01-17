using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using NLog;

namespace Archivarius.Algorithms.SystemCompression
{
    
    
    public class SystemCompressionAlgorithm : AbstractAlgorithm
    {
        public override string Extension => ".gz";
        public override AlgorithmType Type  => AlgorithmType.Gzip;
        public override byte[] Compress(string text, string filename)
        {
            using (var outStream = new MemoryStream())
            {
                using (var tinyStream = new GZipStream(outStream, CompressionMode.Compress))
                using (var mStream = new MemoryStream(Encoding.UTF8.GetBytes(text)))
                    mStream.CopyTo(tinyStream);
                
                return Encoding.UTF8.GetBytes(filename + Delimiter).Concat(outStream.ToArray()).ToArray();
            }
        }

        public override byte[] DecompressOneFile(byte[] bytes)
        {
            using var inStream = new MemoryStream(bytes);
            using var bigStream = new GZipStream(inStream, CompressionMode.Decompress);
            using var bigStreamOut = new MemoryStream();
            bigStream.CopyTo(bigStreamOut);
            return bigStreamOut.ToArray();
        }
    }
}