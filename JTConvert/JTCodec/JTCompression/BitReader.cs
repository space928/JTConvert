using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace JTConvert.JTCodec.JTCompression
{
    /// <summary>
    /// Extension of a BinaryJTReader for reading unaligned binary data from a JT file.
    /// </summary>
    public class BitReader : BinaryJTReader
    {
        /// <summary>
        /// Keep a shift register of the last read byte from the stream and shift bits 
        /// out of it as needed and shift a byte in when we run out of bits.
        /// </summary>
        private int shiftRegister;
        /// <summary>
        /// How many bits are available in the shift register. Never >= 8.
        /// </summary>
        private byte available;

        /// <summary>
        /// Absolute position in bits in the stream.
        /// </summary>
        public long BitPosition => base.BaseStream.Position * 8 - available;

        public BitReader(Stream input, bool bigEndian = false) : base(input, bigEndian)
        {
        }

        /// <summary>
        /// Skips bits until the stream is realigned with the next byte.
        /// </summary>
        public void ByteAlign()
        {
            available = 0;
            shiftRegister = 0;
        }

        /// <summary>
        /// Reads unaligned bits from the stream into a byte.
        /// </summary>
        /// <param name="bits">Number of bits to read, never read more than 8 bits!</param>
        /// <returns></returns>
        public byte ReadBits(byte bits)
        {
#if DEBUG
            if (bits > 8)
                throw new ArgumentException("ReadBits() Tried to read more than 8 bits! Attempted to read " + bits + " bits!");
#endif

            // Clear any previous output bits from the shift register
            shiftRegister &= 0xff;

            // Refill if needed
            if (bits > available)
            {
                shiftRegister <<= available;
                bits -= available;
                shiftRegister |= base.ReadByte();
                available = 8;
            }

            shiftRegister <<= bits;
            available -= bits;

            return (byte)(shiftRegister>>8);
        }

        /// <summary>
        /// Reads unaligned bits from the stream into a byte.
        /// Optimized to read up to 4 bytes at a time into an int.
        /// </summary>
        /// <param name="bits"></param>
        /// <returns></returns>
        public int ReadIntBits(byte bits)
        {
#if DEBUG
            if (bits > 32)
                throw new ArgumentException("ReadIntBits() Tried to read more than 32 bits! Attempted to read " + bits + " bits!");
#endif
            if (bits <= 8)
                return ReadBits(bits);

            // Read any full bytes
            int ret = 0;
            do
            {
                ret <<= 8;
                ret |= ReadByte();
                bits -= 8;
            } while (bits > 0);

            // Get the last few bits if needed
            if (bits != 0)
            {
                bits += 8;
                ret <<= bits;
                ret |= ReadBits(bits);
            }

            return ret;
        }

        public override int Read() => throw new NotImplementedException();

        public override int Read(byte[] buffer, int index, int count)
        {
#if DEBUG
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));
            if (index < 0 || index >= buffer.Length)
                throw new ArgumentOutOfRangeException(nameof(index));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            if (index + count > buffer.Length)
                throw new ArgumentOutOfRangeException(nameof(count));

            long maxCount = BaseStream.Length - BaseStream.Position;
            if (count > maxCount)
                count = (int)maxCount;
#endif

            for (int i = index; i < count; i++)
                buffer[i] = ReadByte();

            return count;
        }

        public override int PeekChar() => throw new NotImplementedException();

        public override int Read(char[] buffer, int index, int count) => throw new NotImplementedException();

        public override int Read(Span<byte> buffer) => throw new NotImplementedException();

        public override int Read(Span<char> buffer) => throw new NotImplementedException();

        public override bool ReadBoolean() => throw new NotImplementedException();

        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        public override byte ReadByte()
        {
            shiftRegister <<= available;
            shiftRegister |= base.ReadByte();
            shiftRegister <<= 8-available;

            return (byte)(shiftRegister >> 8);
        }
        

        public override byte[] ReadBytes(int count) => throw new NotImplementedException();

        public override char ReadChar() => throw new NotImplementedException();

        public override char[] ReadChars(int count) => throw new NotImplementedException();

        public override decimal ReadDecimal() => throw new NotImplementedException();

        public override sbyte ReadSByte() => throw new NotImplementedException();
    }
}
