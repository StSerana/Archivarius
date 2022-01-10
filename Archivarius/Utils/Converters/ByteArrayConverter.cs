using System.Collections;
using System.Collections.Generic;

namespace Archivarius.Utils.Converters
{
    public static class ByteArrayConverter
    {
        public static int ByteArrayPatternSearch(IReadOnlyList<byte> pattern, IReadOnlyList<byte> src)
        {
            var maxFirstCharSlot = src.Count - pattern.Count + 1;
            for (var i = 0; i < maxFirstCharSlot; i++)
            {
                if (src[i] != pattern[0]) continue;
        
                for (var j = pattern.Count - 1; j >= 1; j--) 
                {
                    if (src[i + j] != pattern[j]) break;
                    if (j == 1) return i;
                }
            }
            return -1;
        }
        
        public static IEnumerable<byte> BitArrayToByteArray(BitArray bits)
        {
            var ret = new byte[(bits.Length - 1) / 8 + 1];
            bits.CopyTo(ret, 0);
            return ret;
        }
    }
}