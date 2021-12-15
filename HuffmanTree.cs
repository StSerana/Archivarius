using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Archivarius
{
    public class HuffmanTree
    {
        private List<HuffmanNode> nodes = new();
        public HuffmanNode Root { get; set; }
        private Dictionary<char, int> Frequencies = new();

        public void Build(string source)
        {
            foreach (var t in source)
            {
                if (!Frequencies.ContainsKey(t))
                {
                    Frequencies.Add(t, 0);
                }

                Frequencies[t]++;
            }

            foreach (var (key, value) in Frequencies)
            {
                nodes.Add(new HuffmanNode {Symbol = key, Frequency = value});
            }
            
            CreateTree(nodes);
        }

        private void CreateTree(List<HuffmanNode> huffmanNodes)
        {
            while (huffmanNodes.Count > 1)
            {
                var orderedNodes = huffmanNodes.OrderBy(node => node.Frequency).ToList();

                if (orderedNodes.Count >= 2)
                {
                    // Take first two items
                    var taken = orderedNodes.Take(2).ToList();

                    // Create a parent node by combining the frequencies
                    var parent = new HuffmanNode()
                    {
                        Symbol = '*',
                        Frequency = taken[0].Frequency + taken[1].Frequency,
                        Left = taken[0],
                        Right = taken[1]
                    };

                    huffmanNodes.Remove(taken[0]);
                    huffmanNodes.Remove(taken[1]);
                    huffmanNodes.Add(parent);
                }

                Root = huffmanNodes.FirstOrDefault();

            }
        }

        public BitArray Encode(string source)
        {
            var encodedSource = new List<bool>();

            for (var i = 0; i < source.Length; i++)
            {
                var encodedSymbol = this.Root.Traverse(source[i], new List<bool>());
                encodedSource.AddRange(encodedSymbol);
            }

            var bits = new BitArray(encodedSource.ToArray());

            return bits;
        }

        public string Decode(BitArray bits)
        {
            var current = this.Root;
            var decoded = "";

            foreach (bool bit in bits)
            {
                if (bit)
                {
                    if (current.Right != null)
                    {
                        current = current.Right;
                    }
                }
                else
                {
                    if (current.Left != null)
                    {
                        current = current.Left;
                    }
                }

                if (IsLeaf(current))
                {
                    decoded += current.Symbol;
                    current = this.Root;
                }
            }

            return decoded;
        }
        
        // Print tree function
        public static StringBuilder printTree(HuffmanNode node, StringBuilder result)
        {
            if (node == null)
            {
                return result;
            }

            if (node.Symbol != '*')
                result.Append("(" + node.Frequency + '-' + node.Symbol + "");
            printTree(node.Left, result);
            printTree(node.Right, result);
            return result;
        }

        public static HuffmanTree TreeFromString(string source)
        {
            var tree = new HuffmanTree();
            var regex = new Regex(@"(\d*)(-)(.)");
            var matches = regex.Matches(source);
            var huffmanNodes = new List<HuffmanNode>();
            foreach (Match match in matches)
            {
                huffmanNodes.Add(new HuffmanNode(match.Groups[3].Value[0], int.Parse(match.Groups[1].Value)));
                Console.WriteLine(match.Groups[3].Value[0] + " " +int.Parse(match.Groups[1].Value));
            }

            tree.CreateTree(huffmanNodes);
            return tree;
        }

        private static bool IsLeaf(HuffmanNode huffmanNode) => huffmanNode.Left == null && huffmanNode.Right == null;
    }
}