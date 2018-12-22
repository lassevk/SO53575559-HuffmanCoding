using System.IO;
using System.Text;

namespace HuffmanCoding
{
    internal class HuffmanDecompressor
    {
        public void Decompress(Stream sourceStream, FileStream targetStream)
        {
            int length;
            using (var reader = new BinaryReader(sourceStream, Encoding.Default, true))
            {
                length = reader.ReadInt32();
            }

            if (length == 0)
                return;

            using (var reader = new BitStreamReader(sourceStream, false))
            {
                var root = ReadTree(reader);

                while (length-- > 0)
                {
                    var node = root;
                    while (node is InternalNode internalNode)
                        if (reader.ReadBit())
                            node = internalNode.RightChild;
                        else
                            node = internalNode.LeftChild;

                    var leafNode = (LeafNode) node;
                    targetStream.WriteByte(leafNode.Symbol);
                }
            }
        }

        private Node ReadTree(BitStreamReader reader)
        {
            if (reader.ReadBit())
                return new LeafNode(reader.ReadByte(), 0);
            
            Node leftChild = ReadTree(reader);
            Node rightChild = ReadTree(reader);
            return new InternalNode(leftChild, rightChild);
        }
    }
}