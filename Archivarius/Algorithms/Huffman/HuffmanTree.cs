using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Archivarius.Utils.Converters;

namespace Archivarius.Algorithms.Huffman
{
    public class HuffmanTree
    {
        private List<HuffmanNode> nodes = new();
        private static readonly Regex TREE_NODE_PATTERN = new(@"(\d*)(-)(.|\n|\t|\r|[^\u0000-\u007F]+)");
        public HuffmanNode Root { get; private set; }
        private readonly Dictionary<char, int> Frequencies = new();

        public void Build(string source)
        {
            var i = 1;
            foreach (var t in source)
            {
                //Console.WriteLine("append new node " + i + " of " + source.Length);
                if (!Frequencies.ContainsKey(t))
                {
                    Frequencies.Add(t, 0);
                }

                Frequencies[t]++;
                i++;
            }

            foreach (var (key, value) in Frequencies)
            {
                nodes.Add(new HuffmanNode {Symbol = key, Frequency = value});
            }
            //Console.WriteLine("get all symbols");
            CreateTree(nodes);
        }

        private void CreateTree(ICollection<HuffmanNode> huffmanNodes)
        {
            var i = 1;
            Console.WriteLine("create tree");
            while (huffmanNodes.Count > 1)
            {
                //Console.WriteLine("append new node " + i + " of " + huffmanNodes.Count);
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
            var treeDictionary = new Dictionary<char, List<bool>>();
            HuffmanNode.Traverse(Root, "", treeDictionary);
            foreach (var symbol in source)
            {
                encodedSource.AddRange(treeDictionary[symbol]);
            }
            var bits = new BitArray(encodedSource.ToArray());

            return bits;
        }

        public string Decode(BitArray bits)
        {
            var current = Root;
            var decoded = "";
            var treeDictionary = new Dictionary<string, char>();
            HuffmanNode.ReverseTraverse(Root, "", treeDictionary);

            for (var i = 0; i < bits.Count; i++)
            {
                //Console.WriteLine("find key for " + i);
                var key = "";
                for (var j = i; j < bits.Count; j++)
                {
                    key += bits[j] ? 1 : 0;
                    if (!treeDictionary.ContainsKey(key)) continue;
                    decoded += treeDictionary[key];
                    i = j;
                    break;
                }
            }

            return decoded;
        }
        
        public static StringBuilder TreeToString(HuffmanNode node, StringBuilder result)
        {
            //Console.WriteLine("tree to string");
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
            Console.WriteLine(source);
            var huffmanNodes = new List<HuffmanNode>();
            
            foreach (Match match in matches)
                huffmanNodes.Add(new HuffmanNode(match.Groups[3].Value[0], int.Parse(match.Groups[1].Value)));

            tree.CreateTree(huffmanNodes);
            return tree;
        }

        public static Tuple<string, byte[]> FindTree(IReadOnlyList<byte> source, IReadOnlyList<byte> delimiter)
        {
            var index = ByteArrayConverter.ByteArrayPatternSearch(delimiter, source);

            var tree = string.Join("", Encoding.Default.GetString(source.Take(index).ToArray()));
            var encoded = source.Skip(index + delimiter.Count).ToArray();
            
            return Tuple.Create(tree, encoded);
        }
    }
}