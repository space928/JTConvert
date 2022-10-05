using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace JTConvert.JTCodec.JTCompression
{
    public static class JTCompressedDataPacketCodec
    {
        /// <summary>
        /// Applies a predictor to decompressed data from an Int32CDP.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="predictor"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static float[] ApplyPredictor(uint[] data, JTPredictor predictor)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Decodes a compressed data packet into an array of uints.
        /// Automatically selects between the 3 versions of Int32CDP.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="jtVersion"></param>
        /// <param name="uint64"></param>
        /// <param name="mark2">In JT V9.x CDPs can either be mk1 or mk2, generally most packets are mk2.</param>
        /// <returns></returns>
        public static int[] ReadIntCDP(BinaryJTReader reader, int jtVersion, bool uint64 = false, bool mark2 = true)
        {
            int valueCount = (jtVersion >= 10 || mark2) ? reader.ReadInt32() : 0;
            JTCodecType codec = (JTCodecType)reader.ReadByte();

            Logger.Debug((int)codec);

            switch(codec)
            {
                case JTCodecType.Null:
                    // TODO: In JT V10, we might need to read an extra int32 for the CodeTextLength (unused)
                    // TODO: Normally we would use a simple reader.ReadVecU32() here, but the spec seems to indicate this might not work?
                    int encodedLength = reader.ReadInt32() / 32;
                    return reader.ReadVecI32(encodedLength);

                case JTCodecType.Bitlength:
                    encodedLength = reader.ReadInt32();
                    if(!mark2 && jtVersion <= 9)
                        valueCount = reader.ReadInt32();
                    return JTBitLengthCodec.Decode(reader, encodedLength, valueCount, mark2 || jtVersion >= 10);

                case JTCodecType.Arithmetic:
                    // In JT V10 (and mk2) we read the encoded data first and in V9 Mk1 it's the other way around
                    if (jtVersion >= 10 || mark2)
                    {
                        encodedLength = reader.ReadInt32() / 32;
                        var codeText = reader.ReadVecU32(encodedLength);
                    }

                    var probabilityContext = ReadProbabilityContext(reader, jtVersion, uint64);
                    if (jtVersion <= 9)
                    {
                        int outOfBandCount = reader.ReadInt32();
                        for(int i = 0; i < outOfBandCount; i++)
                        {
                            // TODO: return value
                            ReadIntCDP(reader, jtVersion, uint64, mark2);
                        }
                    } else if (/*TODO: if SegmentIsExternallyCompressed*/true)
                    {
                        // JT V10
                        //int outOfBandCount = reader.ReadInt32();
                        var outOfBandValues = reader.ReadVecI32();
                        // TODO: return value
                    }
                    else 
                    {
                        // JT V10
                        return ReadIntCDP(reader, jtVersion, uint64, false);
                    }

                    break;

                case JTCodecType.MoveToFront:
                    //throw new NotImplementedException();
                    break;

                case JTCodecType.Chopper:
                    //throw new NotImplementedException();
                    break;

                default:
                    Logger.Log($"Unrecognised CDP codec type: {codec}", Logger.VerbosityLevel.WARN);
                    break;
            }

            return null;
        }

        private static JTProbabilityContext[] ReadProbabilityContext(BinaryJTReader reader, int jtVersion, bool uint64)
        {
            var bReader = new BitReader(reader.BaseStream, reader.BigEndian);
            int tableCount = jtVersion >= 10 ? 1 : reader.ReadByte();
            JTProbabilityContext[] tables = new JTProbabilityContext[tableCount];
            for (int i = 0; i < tableCount; i++)
            {
                JTProbabilityContext table = new();
                if (jtVersion >= 10)
                {
                    table.probabilityContextTableEntryCount = reader.ReadUInt16();
                    table.numberOccurenceCountBits = bReader.ReadBits(6);
                    table.numberValueBits = bReader.ReadBits(7);
                    if (uint64)
                        table.minValue = bReader.ReadUInt64();
                    else
                        table.minValue = bReader.ReadUInt32();
                } else
                {
                    // Some very subtle differences between V9 and V10 just to mess with us...
                    table.probabilityContextTableEntryCount = reader.ReadUInt32();
                    table.numberSymbolBits = bReader.ReadBits(6);
                    table.numberOccurenceCountBits = bReader.ReadBits(6);
                    if(i == 0)
                        table.numberValueBits = bReader.ReadBits(6);
                    table.numberNextContextBits = bReader.ReadBits(6);
                    if (i == 0)
                        table.minValue = bReader.ReadUInt32();
                }

                table.probabilityContextTable = new JTProbabilityContextTableEntry[table.probabilityContextTableEntryCount];
                for (int j = 0; j < table.probabilityContextTable.Length; j++)
                {
                    JTProbabilityContextTableEntry entry = new()
                    {
                        symbol = jtVersion <= 9 ? bReader.ReadBits(table.numberSymbolBits) - 2 : 0,
                        isEscapeSymbol = jtVersion >= 10 ? bReader.ReadBits(1) == 1 : false,
                        occurenceCount = bReader.ReadBits(table.numberOccurenceCountBits),
                        associatedValue = bReader.ReadBits(table.numberValueBits),
                        nextContext = jtVersion <= 9 ? bReader.ReadBits(table.numberNextContextBits) : 0u
                    };
                    table.probabilityContextTable[j] = entry;
                }

                tables[i] = table;
            }

            bReader.ByteAlign();
            return tables;
        }
    }

    public struct JTProbabilityContext
    {
        public uint probabilityContextTableEntryCount;
        public byte numberSymbolBits;
        public byte numberOccurenceCountBits;
        public byte numberValueBits;
        public byte numberNextContextBits;
        public ulong minValue;
        public JTProbabilityContextTableEntry[] probabilityContextTable;
    }

    public struct JTProbabilityContextTableEntry
    {
        public int symbol;
        public bool isEscapeSymbol;
        public uint occurenceCount;
        public int associatedValue;
        public uint nextContext;
    }

    public enum JTCodecType : byte
    {
        Null = 0,
        Bitlength = 1,
        Arithmetic = 3,
        Chopper = 4,
        MoveToFront = 5,
    }

    public enum JTPredictor : byte
    {
        Lag1,
        XOR1,
        Null
    }
}
