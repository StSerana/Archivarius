using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Archivarius
{
    public class HuffmanTree
    {
        private List<HuffmanNode> nodes = new();
        private HuffmanNode Root { get; set; }
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

            while (nodes.Count > 1)
            {
                var orderedNodes = nodes.OrderBy(node => node.Frequency).ToList();

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

                    nodes.Remove(taken[0]);
                    nodes.Remove(taken[1]);
                    nodes.Add(parent);
                }

                Root = nodes.FirstOrDefault();

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

        private static bool IsLeaf(HuffmanNode huffmanNode) => huffmanNode.Left == null && huffmanNode.Right == null;
    }
}