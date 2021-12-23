using System.Text;

namespace Archivarius
{
    public abstract class Algorithm
    {
        protected const string DELIMITER = "###";
        protected static readonly byte[] BYTES_DELIMITER = Encoding.Default.GetBytes(DELIMITER);
        public virtual string Prefix => "";
        public abstract byte[] Compress(string text);
        public abstract  byte[] Decompress(byte[] bytes);
    }
}