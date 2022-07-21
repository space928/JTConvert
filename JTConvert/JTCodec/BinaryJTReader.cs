using Joveler.Compression.XZ;
using Joveler.Compression.ZLib;
using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JTConvert.JTCodec
{
    /// <summary>
    /// Helper class that wraps BinaryReader with methods to read 
    /// JT specific primitive data types.
    /// </summary>
    public class BinaryJTReader : BinaryReader
    {
        private bool bigEndian;
        private byte[] buffer = new byte[8];
        private readonly XZDecompressOptions xzDecompressOptions = new();
        private readonly ZLibDecompressOptions zLibDecompressOptions = new();

        public bool BigEndian { get => bigEndian; set => bigEndian = value; }

        public BinaryJTReader(Stream input, bool bigEndian = false) : base(input)
        {
            this.BigEndian = bigEndian;
        }

        /// <summary>
        /// Decompresses the specified number of bytes from the stream into a new BinaryJTReader.
        /// </summary>
        /// <remarks>
        /// Remember to dispose of the new BinaryJTReader once used.<para/>
        /// Decompression may be deffered until the data is read since it's read into a stream.
        /// </remarks>
        /// <param name="length">How many bytes to read and decompress from the stream</param>
        /// <param name="version">The major version number of the JT file</param>
        /// <returns>A new BinaryJTReader containing the decompressed data.</returns>
        public BinaryJTReader DecompressIntoNewReader(int length, JTCompressionAlgorithm algorithm)
        {
            using MemoryStream ms = new(ReadBytes(length));
            MemoryStream msDst = new(length * 2);
            switch (algorithm)
            {
                case JTCompressionAlgorithm.LZMA:
                    // BinaryJTReader *should* automatically dispose of the new MS when it gets disposed.
                    using (XZStream xz = new(ms, xzDecompressOptions))
                        xz.CopyTo(msDst);
                    msDst.Position = 0;
                    return new BinaryJTReader(msDst, bigEndian);
                case JTCompressionAlgorithm.ZLib:
                    using (ZLibStream xs = new(ms, zLibDecompressOptions))
                        xs.CopyTo(msDst);
                    msDst.Position = 0;
                    return new BinaryJTReader(msDst, bigEndian);
                default:
                    return new BinaryJTReader(ms, bigEndian);
            }
        }

        public override string ReadString()
        {
            uint length = ReadUInt32();
            //TODO: Work out if we can remove the extra copy here.
            char[] chars = new char[length];
            for (int i = 0; i < length; i++)
                chars[i] = ReadChar();

            return new string(chars);
        }

        /// <summary>
        /// Reads a constant sized char array into a new string.
        /// </summary>
        /// <param name="length">The length of the string to read.</param>
        /// <returns></returns>
        public string ReadString(int length)
        {
            //TODO: Work out if we can remove the extra copy here.
            char[] chars = new char[length];
            for (int i = 0; i < length; i++)
                chars[i] = ReadChar();

            return new string(chars);
        }

        public string ReadMbString()
        {
            int length = ReadInt32();
            //TODO: Work out if we can remove the extra copy here.
            char[] chars = new char[length];
            for (int i = 0; i < length; i++)
                chars[i] = (char)ReadUInt16();

            return new string(chars);
        }

        public byte ReadVersion(int jtVersion)
        {
            if (jtVersion >= 10)
                return ReadByte();
            else
                return (byte)ReadInt16();
        }

        public BBoxF32 ReadBBoxF32() => new()
        {
            maxCorner = ReadCoordF32(),
            minCorner = ReadCoordF32()
        };

        public CoordF32 ReadCoordF32() => new()
        {
            x = ReadSingle(),
            y = ReadSingle(),
            z = ReadSingle(),
        };

        public DirF32 ReadDirF32() => new()
        {
            x = ReadSingle(),
            y = ReadSingle(),
            z = ReadSingle(),
        };

        public void Skip(int v)
        {
            BaseStream.Seek(v, SeekOrigin.Current);
        }

        /// <summary>
        /// Reads a 16 byte guid.
        /// </summary>
        /// <returns></returns>
        public GUID ReadGUID() => new()
        {
            a = ReadUInt32(),
            b = ReadUInt16(),
            c = ReadUInt16(),
            d = ReadByte(),
            e = ReadByte(),
            f = ReadByte(),
            g = ReadByte(),
            h = ReadByte(),
            i = ReadByte(),
            j = ReadByte(),
            k = ReadByte(),
        };

        public Mx4F32 ReadMx4F32() => new()
        {
            m00 = ReadSingle(),
            m01 = ReadSingle(),
            m02 = ReadSingle(),
            m03 = ReadSingle(),
            m10 = ReadSingle(),
            m11 = ReadSingle(),
            m12 = ReadSingle(),
            m13 = ReadSingle(),
            m20 = ReadSingle(),
            m21 = ReadSingle(),
            m22 = ReadSingle(),
            m23 = ReadSingle(),
            m30 = ReadSingle(),
            m31 = ReadSingle(),
            m32 = ReadSingle(),
            m33 = ReadSingle(),
        };

        public Mx4F64 ReadMx4F64() => new()
        {
            m00 = ReadDouble(),
            m01 = ReadDouble(),
            m02 = ReadDouble(),
            m03 = ReadDouble(),
            m10 = ReadDouble(),
            m11 = ReadDouble(),
            m12 = ReadDouble(),
            m13 = ReadDouble(),
            m20 = ReadDouble(),
            m21 = ReadDouble(),
            m22 = ReadDouble(),
            m23 = ReadDouble(),
            m30 = ReadDouble(),
            m31 = ReadDouble(),
            m32 = ReadDouble(),
            m33 = ReadDouble(),
        };

        public PlaneF32 ReadPlaneF32() => new()
        {
            a = ReadSingle(),
            b = ReadSingle(),
            c = ReadSingle(),
            d = ReadSingle(),
        };

        public Quaternion ReadQuaternion() => new()
        {
            x = ReadSingle(),
            y = ReadSingle(),
            z = ReadSingle(),
            w = ReadSingle(),
        };

        public RGB ReadRGB() => new()
        {
            r = ReadSingle(),
            g = ReadSingle(),
            b = ReadSingle(),
        };

        public RGBA ReadRGBA() => new()
        {
            r = ReadSingle(),
            g = ReadSingle(),
            b = ReadSingle(),
            a = ReadSingle(),
        };

        public VecF32 ReadVecF32()
        {
            int count = ReadInt32();
            float[] data = new float[count];
            for (int i = 0; i < count; i++)
                data[i] = ReadSingle();
            return new VecF32()
            {
                count = count,
                data = data
            };
        }

        public VecF64 ReadVecF64()
        {
            int count = ReadInt32();
            double[] data = new double[count];
            for (int i = 0; i < count; i++)
                data[i] = ReadDouble();
            return new VecF64()
            {
                count = count,
                data = data
            };
        }

        public VecI16 ReadVecI16()
        {
            int count = ReadInt32();
            short[] data = new short[count];
            for (int i = 0; i < count; i++)
                data[i] = ReadInt16();
            return new VecI16()
            {
                count = count,
                data = data
            };
        }

        public VecU16 ReadVecU16()
        {
            int count = ReadInt32();
            ushort[] data = new ushort[count];
            for (int i = 0; i < count; i++)
                data[i] = ReadUInt16();
            return new VecU16()
            {
                count = count,
                data = data
            };
        }

        public VecI32 ReadVecI32()
        {
            int count = ReadInt32();
            int[] data = new int[count];
            for (int i = 0; i < count; i++)
                data[i] = ReadInt32();
            return new VecI32()
            {
                count = count,
                data = data
            };
        }

        public VecU32 ReadVecU32()
        {
            int count = ReadInt32();
            uint[] data = new uint[count];
            for (int i = 0; i < count; i++)
                data[i] = ReadUInt32();
            return new VecU32()
            {
                count = count,
                data = data
            };
        }

        public JTCountRange ReadCountRange() => new()
        {
            minCount = ReadInt32(),
            maxCount = ReadInt32()
        };

        public override short ReadInt16()
        {
            Read(buffer, 0, 2);
            if (bigEndian)
                return BinaryPrimitives.ReadInt16BigEndian(buffer);
            else
                return BinaryPrimitives.ReadInt16LittleEndian(buffer);
        }

        public override ushort ReadUInt16()
        {
            Read(buffer, 0, 2);
            if (bigEndian)
                return BinaryPrimitives.ReadUInt16BigEndian(buffer);
            else
                return BinaryPrimitives.ReadUInt16LittleEndian(buffer);
        }

        public override int ReadInt32()
        {
            Read(buffer, 0, 4);
            if (bigEndian)
                return BinaryPrimitives.ReadInt32BigEndian(buffer);
            else
                return BinaryPrimitives.ReadInt32LittleEndian(buffer);
        }

        public override uint ReadUInt32()
        {
            Read(buffer, 0, 4);
            if (bigEndian)
                return BinaryPrimitives.ReadUInt32BigEndian(buffer);
            else
                return BinaryPrimitives.ReadUInt32LittleEndian(buffer);
        }

        public override long ReadInt64()
        {
            Read(buffer, 0, 8);
            if (bigEndian)
                return BinaryPrimitives.ReadInt64BigEndian(buffer);
            else
                return BinaryPrimitives.ReadInt64LittleEndian(buffer);
        }

        public override ulong ReadUInt64()
        {
            Read(buffer, 0, 8);
            if (bigEndian)
                return BinaryPrimitives.ReadUInt64BigEndian(buffer);
            else
                return BinaryPrimitives.ReadUInt64LittleEndian(buffer);
        }

        public override float ReadSingle()
        {
            Read(buffer, 0, 4);
            if (bigEndian)
                return BinaryPrimitives.ReadSingleBigEndian(buffer);
            else
                return BinaryPrimitives.ReadSingleLittleEndian(buffer);
        }

        public override double ReadDouble()
        {
            Read(buffer, 0, 8);
            if (bigEndian)
                return BinaryPrimitives.ReadDoubleBigEndian(buffer);
            else
                return BinaryPrimitives.ReadDoubleLittleEndian(buffer);
        }
    }
}
