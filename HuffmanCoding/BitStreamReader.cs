using System;
using System.IO;

namespace HuffmanCoding
{
    public sealed class BitStreamReader : IDisposable
    {
        private readonly Stream _Stream;
        private readonly bool _OwnsStream;
        private byte _Buffer;
        private int _InBuffer;

        public BitStreamReader(Stream stream, bool ownsStream = true)
        {
            _Stream = stream ?? throw new ArgumentNullException(nameof(stream));
            _OwnsStream = ownsStream;
        }

        public bool ReadBit()
        {
            FillBufferIfEmpty();

            var result = (_Buffer & 0x80) != 0;
            _Buffer = (byte) ((_Buffer & 0x7f) << 1);
            _InBuffer--;
            return result;
        }

        public byte ReadByte()
        {
            FillBufferIfEmpty();
            
            if (_InBuffer == 8)
            {
                _InBuffer = 0;
                return _Buffer;
            }

            int result = 0;
            for (int index = 0; index < 8; index++)
                result = (result << 1) | (ReadBit() ? 1 : 0);
            return (byte) result;
        }

        private void FillBufferIfEmpty()
        {
            if (_InBuffer > 0)
                return;
            
            var b = _Stream.ReadByte();
            if (b < 0)
                throw new InvalidOperationException();

            _Buffer = (byte) b;
            _InBuffer = 8;
        }

        public void Dispose()
        {
            if (_OwnsStream)
                _Stream.Dispose();
        }
    }
}