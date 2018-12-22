namespace HuffmanCoding
{
    public abstract class Node
    {
        protected Node(int count) => Count = count;

        public int Count { get; }
    }
}