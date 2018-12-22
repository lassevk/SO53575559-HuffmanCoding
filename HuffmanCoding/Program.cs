using System;
using System.IO;
using System.Linq;

namespace HuffmanCoding
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var source = File.Open("sample-input.txt", FileMode.Open))
            using (var target = File.Create("temp-compressed.bin"))
            {
                new HuffmanCompressor().Compress(source, target);

                Console.WriteLine($"compression of {source.Length} bytes --> {target.Length} bytes");
            }
            
            using (var source = File.Open("temp-compressed.bin", FileMode.Open))
            using (var target = File.Create("sample-output.txt"))
            {
                new HuffmanDecompressor().Decompress(source, target);
                
                Console.WriteLine($"decompression of {source.Length} bytes --> {target.Length} bytes");
            }

            var sourceBytes = File.ReadAllBytes("sample-input.txt");
            var targetBytes = File.ReadAllBytes("sample-output.txt");

            if (!sourceBytes.SequenceEqual(targetBytes))
                throw new InvalidOperationException();

            Console.WriteLine("Done");
        }
    }
}