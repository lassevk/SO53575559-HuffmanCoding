namespace HuffmanCoding
{
    public class InternalNode : Node
    {
        public InternalNode(Node leftChild, Node rightChild)
            : base(leftChild.Count + rightChild.Count)
            => (LeftChild, RightChild) = (leftChild, rightChild);

        public Node LeftChild { get; }
        public Node RightChild { get; }

        public override string ToString() => $"internal × {Count}";
    }
}