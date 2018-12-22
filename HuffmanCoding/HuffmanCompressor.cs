using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace HuffmanCoding
{
    public class HuffmanCompressor
    {
        public void Compress(Stream sourceStream, Stream targetStream)
        {
            Span<byte> input = new byte[sourceStream.Length];
            sourceStream.Read(input);

            var occurances = CountOccurances(input);
            var root = CreateTreeFromOccurances(occurances);
            var bits = CreateBitsFromTree(root);

            using (var writer = new BinaryWriter(targetStream, Encoding.Default, true))
            {
                writer.Write(input.Length);
            }

            using (var writer = new BitStreamWriter(targetStream, false))
            {
                WriteTree(root, writer);

                foreach (byte b in input)
                foreach (bool bit in bits[b])
                    writer.WriteBit(bit);
            }
        }

        private void WriteTree(Node node, BitStreamWriter writer)
        {
            switch (node)
            {
                case LeafNode leafNode:
                    writer.WriteBit(true);
                    writer.WriteByte(leafNode.Symbol);
                    break;
                
                case InternalNode internalNode:
                    writer.WriteBit(false);
                    WriteTree(internalNode.LeftChild, writer);
                    WriteTree(internalNode.RightChild, writer);
                    break;

                default:
                    throw new InvalidOperationException();
            }
        }

        private Dictionary<byte, bool[]> CreateBitsFromTree(Node root)
        {
            var result = new Dictionary<byte, bool[]>();
            var bits = new Stack<bool>();

            traverse(root);

            return result;

            void traverse(Node node)
            {
                switch (node)
                {
                    case LeafNode leafNode:
                        result[leafNode.Symbol] = bits.Reverse().ToArray();
                        break;
                    
                    case InternalNode internalNode:
                        bits.Push(false);
                        traverse(internalNode.LeftChild);

                        bits.Pop();
                        bits.Push(true);

                        traverse(internalNode.RightChild);

                        bits.Pop();
                        break;

                    default:
                        throw new InvalidOperationException();
                }
            }
        }

        private Node CreateTreeFromOccurances(Dictionary<byte, int> occurances)
        {
            List<Node> nodes = occurances.Select(kvp => (Node)new LeafNode(kvp.Key, kvp.Value)).ToList();
            if (nodes.Count == 0)
                throw new InvalidOperationException();

            if (nodes.Count == 1)
            {
                var firstNode = (LeafNode) nodes[0];
                
                // Hack to make sure we have at least 1 internal node
                var otherSymbol = unchecked((byte) (firstNode.Symbol + 1));
                nodes.Add(new LeafNode(otherSymbol, 1));
            }

            while (nodes.Count > 1)
            {
                nodes.Sort((n1, n2) => n1.Count.CompareTo(n2.Count));

                var node1 = nodes[0];
                var node2 = nodes[1];

                nodes[0] = new InternalNode(node1, node2);
                nodes.RemoveAt(1);
            }

            return nodes[0];
        }

        private Dictionary<byte, int> CountOccurances(Span<byte> input)
        {
            var table = new int[256];
            foreach (var b in input)
                table[b]++;

            var result = new Dictionary<byte, int>();
            for (byte index = 0; index < 255; index++)
                if (table[index] > 0)
                    result[index] = table[index];
            return result;
        }
    }
}