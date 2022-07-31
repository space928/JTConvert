using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JTConvert.JTCodec.JTCompression
{
    public static class JTCompressedDataPacketCodec
    {
        public static uint[] ReadIntCDP(BinaryJTReader reader, int jtVersion, bool uint64 = false)
        {
            int valueCount = jtVersion >= 10 ? reader.ReadInt32() : 0;
            JTCodecType codec = (JTCodecType)reader.ReadByte();

            switch(codec)
            {
                case JTCodecType.Null:
                    int encodedLength = reader.ReadInt32();
                    return reader.ReadVecU32();

                case JTCodecType.Bitlength:
                    encodedLength = reader.ReadInt32();
                    return JTBitLengthCodec.Decode(reader.ReadVecU32());

                case JTCodecType.Arithmetic:
                    encodedLength = reader.ReadInt32();
                    var codeText = reader.ReadVecU32();
                    var probabilityContext = ReadProbabilityContext(reader, jtVersion, uint64);
                    if(/*TODO: if SegmentIsExternallyCompressed*/ true)
                    {
                        //int outOfBandCount = reader.ReadInt32();
                        var outOfBandValues = reader.ReadVecI32();
                    } else
                    {

                    }

                    break;

                case JTCodecType.MoveToFront:
                    break;

                case JTCodecType.Chopper:
                    break;

                default:
                    Logger.Log($"Unrecognised CDP codec type: {codec}", Logger.VerbosityLevel.WARN);
                    break;
            }

            return null;
        }

        private static JTProbabilityContext ReadProbabilityContext(BinaryJTReader reader, int jtVersion, bool uint64)
        {
            JTProbabilityContext ret = new();
            var bReader = new BitReader(reader.BaseStream, reader.BigEndian);
            ret.probabilityContextTableEntryCount = reader.ReadUInt16();
            ret.numberOccurenceCountBits = bReader.ReadBits(6);
            ret.numberValueBits = bReader.ReadBits(7);
            if (uint64)
                ret.minValueLong = bReader.ReadUInt64();
            else
                ret.minValue = bReader.ReadUInt32();

            ret.probabilityContextTable = new JTProbabilityContextTableEntry[ret.probabilityContextTableEntryCount];
            for (int i = 0; i < ret.probabilityContextTable.Length; i++)
            {
                JTProbabilityContextTableEntry entry = new()
                {
                    isEscapeSymbol = bReader.ReadBits(1) == 1,
                    occurenceCount = bReader.ReadBits(ret.numberOccurenceCountBits),
                    associatedValue = bReader.ReadBits(ret.numberValueBits)
                };
                ret.probabilityContextTable[i] = entry;
            }

            bReader.ByteAlign();
            return ret;
        }
    }

    public struct JTProbabilityContext
    {
        public ushort probabilityContextTableEntryCount;
        public byte numberOccurenceCountBits;
        public byte numberValueBits;
        public uint minValue;
        public ulong minValueLong;
        public JTProbabilityContextTableEntry[] probabilityContextTable;
    }

    public struct JTProbabilityContextTableEntry
    {
        public bool isEscapeSymbol;
        public uint occurenceCount;
        public int associatedValue;
    }

    public enum JTCodecType : byte
    {
        Null = 0,
        Bitlength = 1,
        Arithmetic = 3,
        Chopper = 4,
        MoveToFront = 5,
    }
}
