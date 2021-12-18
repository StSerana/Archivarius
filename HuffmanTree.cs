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
        private static readonly Regex TREE_NODE_PATTERN = new Regex(@"(\d*)(-)(.|\n|\t|\r)");
        public HuffmanNode Root { get; private set; }
        private readonly Dictionary<char, int> Frequencies = new();

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

        private void CreateTree(ICollection<HuffmanNode> huffmanNodes)
        {
            while (huffmanNodes.Count > 1)
            {
                var orderedNodes = huffmanNodes
                    .OrderBy(node => node.Frequency)
                    .ThenBy(node => node.Symbol)
                    .ToList();

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

            foreach (var encodedSymbol in source.Select(t => Root.Traverse(t, new List<bool>())))
            {
                encodedSource.AddRange(encodedSymbol);
            }

            var bits = new BitArray(encodedSource.ToArray());

            return bits;
        }

        public string Decode(BitArray bits)
        {
            var current = Root;
            var decoded = "";

            foreach (bool bit in bits)
            {
                if (bit)
                {
                    if (current.Right != null) current = current.Right;
                }
                else if (current.Left != null) current = current.Left;

                if (!IsLeaf(current)) continue;
                decoded += current.Symbol;
                current = Root;
            }

            return decoded;
        }
        
        public static StringBuilder TreeToString(HuffmanNode node, StringBuilder result)
        {
            if (node == null)
                return result;

            if (node.Symbol != '*')
                result.Append("(" + node.Frequency + '-' + node.Symbol + "");
            
            TreeToString(node.Left, result);
            TreeToString(node.Right, result);
            return result;
        }

        public static HuffmanTree TreeFromString(string source)
        {
            var tree = new HuffmanTree();
            var matches = TREE_NODE_PATTERN.Matches(source);
            var huffmanNodes = new List<HuffmanNode>();
            
            foreach (Match match in matches)
                huffmanNodes.Add(new HuffmanNode(match.Groups[3].Value[0], int.Parse(match.Groups[1].Value)));

            tree.CreateTree(huffmanNodes);
            return tree;
        }

        private static bool IsLeaf(HuffmanNode huffmanNode) => huffmanNode.Left == null && huffmanNode.Right == null;
        
        public static Tuple<string, byte[]> FindTree(IReadOnlyList<byte> source, IReadOnlyList<byte> delimiter)
        {
            var index = ByteArrayConverter.ByteArrayPatternSearch(delimiter, source);

            var tree = string.Join("", Encoding.Default.GetString(source.Take(index).ToArray()));
            var encoded = source.Skip(index + delimiter.Count).ToArray();
            
            return Tuple.Create(tree, encoded);
        }
    }
}