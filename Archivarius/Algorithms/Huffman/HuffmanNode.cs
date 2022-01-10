using System.Collections.Generic;
using System.Linq;

namespace Archivarius.Algorithms.Huffman
{
    public class HuffmanNode
    {
        public char Symbol { get; init; }
        public int Frequency { get; init; }
        public HuffmanNode Right { get; init; }
        public HuffmanNode Left { get; init; }

        public HuffmanNode()
        {
        }
        public HuffmanNode(char symbol, int frequency)
        {
            Frequency = frequency;
            Symbol = symbol;
        }
        public static void Traverse(HuffmanNode root, string data, Dictionary<char, List<bool>> result)
        {
            if (root == null)
                return;

            if (root.Left == null && root.Right == null)
            {
                result.Add(root.Symbol, GetBinary(data));
                return;
            }
            
            Traverse(root.Left, data + "0", result);
            Traverse(root.Right,data + "1", result);
        }
        
        public static void ReverseTraverse(HuffmanNode root, string data, Dictionary<string, char> result)
        {
            if (root == null)
                return;

            if (root.Left == null && root.Right == null)
            {
                result.Add(data, root.Symbol);
                return;
            }
            
            ReverseTraverse(root.Left, data + "0", result);
            ReverseTraverse(root.Right,data + "1", result);
        }

        private static List<bool> GetBinary(string src) => src.Select(s => s.ToString() == "1").ToList();
    }
}