using System;
using System.IO;

namespace HuffmanCoding
{
    public sealed class BitStreamWriter : IDisposable
    {
        private readonly Stream _Stream;
        private readonly bool _OwnsStream;

        private byte _Buffer;
        private byte _NextBit = 0x80;

        public BitStreamWriter(Stream stream, bool ownsStream = true)
        {
            _Stream = stream ?? throw new ArgumentNullException(nameof(stream));
            _OwnsStream = ownsStream;
        }

        public void WriteBit(bool bit)
        {
            if (bit)
                _Buffer |= _NextBit;

            _NextBit >>= 1;
            if (_NextBit == 0x00)
            {
                _Stream.WriteByte(_Buffer);
                
                _Buffer = 0;
                _NextBit = 0x80;
            }
        }

        public void WriteByte(Byte symbol)
        {
            for (int nextBit = 0x80; nextBit > 0; nextBit >>= 1)
                WriteBit((symbol & nextBit) != 0);
        }

        public void Dispose()
        {
            if (_NextBit != 0x80)
                _Stream.WriteByte(_Buffer);

            if (_OwnsStream) _Stream.Dispose();
        }
    }
}