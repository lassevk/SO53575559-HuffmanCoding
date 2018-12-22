namespace HuffmanCoding
{
    public class LeafNode : Node
    {
        public LeafNode(byte symbol, int count)
            : base(count)
            => Symbol = symbol;

        public byte Symbol { get; }

        public override string ToString() => $"leaf: {Symbol} × {Count}";
    }
}