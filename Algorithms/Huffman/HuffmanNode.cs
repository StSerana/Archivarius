using System;
using System.Collections.Generic;
using System.Linq;

namespace Archivarius
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
            this.Frequency = frequency;
            this.Symbol = symbol;
        }
        public static void Traverse(HuffmanNode root, string data, Dictionary<char, List<bool>> result)
        {
            if (root == null)
                return;

            // found a leaf node
            if (root.Left == null && root.Right == null)
            {
                result.Add(root.Symbol, getBinary(data));
                return;
            }


            Traverse(root.Left, data + "0", result);
            Traverse(root.Right,data + "1", result);
        }
        
        public static void ReverseTraverse(HuffmanNode root, string data, Dictionary<string, char> result)
        {
            if (root == null)
                return;

            // found a leaf node
            if (root.Left == null && root.Right == null)
            {
                result.Add(data, root.Symbol);
                return;
            }


            ReverseTraverse(root.Left, data + "0", result);
            ReverseTraverse(root.Right,data + "1", result);
        }

        private static List<bool> getBinary(string src)
        {
            return src.Select(s => s.ToString() == "1").ToList();
        }
    }
}