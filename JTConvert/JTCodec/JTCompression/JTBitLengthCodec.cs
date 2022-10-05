namespace JTConvert.JTCodec.JTCompression
{
    internal static class JTBitLengthCodec
    {
        internal static int[] Decode(BinaryJTReader reader, int codeTextLength, int numberOfSymbols, bool mark2)
        {
            if (mark2)
                return DecodeMark2(reader, codeTextLength, numberOfSymbols);
            else
                return DecodeMark1(reader, codeTextLength, numberOfSymbols);
        }

        private static int[] DecodeMark1(BinaryJTReader reader, int codeTextLength, int numberOfSymbols)
        {
            // The reference source is not very correct so much of so some of this is derived from the
            // JTViewerGC project from Jerome Fuselier and Fabio Corubolo
            BitReader bitReader = new BitReader(reader.BaseStream, reader.BigEndian);
            int[] ret = new int[numberOfSymbols];

            long startPos = bitReader.BitPosition;
            byte bitFieldWidth = 0;
            int i = 0;
            while (bitReader.BitPosition - startPos < codeTextLength)
            {
                if (bitReader.ReadBits(1) == 1)
                {
                    // Read new bit field length
                    // A sequence of bits tells us how many bits to adjust the bit length by.
                    // The end of the sequence is indicated by having one bit different
                    // ie: 0001 => subtract 6 from the bitlength
                    //     111110 => add 10 to the bitlength
                    byte adjustmentBit = bitReader.ReadBits(1);
                    do
                    {
                        if (adjustmentBit == 1)
                            bitFieldWidth += 2;
                        else
                            bitFieldWidth -= 2;
                    } while (bitReader.ReadBits(1) == adjustmentBit);
                }

                // Decode the symbol using the bit field width
                int symbol = -1;
                if (bitFieldWidth == 0)
                    symbol = 0;
                else
                {
                    symbol = bitReader.ReadIntBits(bitFieldWidth);

                    // Convert and sign-extend the symbol
                    symbol <<= (32 - bitFieldWidth);
                    symbol >>= (32 - bitFieldWidth);
                }
                ret[i++] = symbol;
            }

            return ret;
        }

        private static int[] DecodeMark2(BinaryJTReader reader, int codeTextLength, int numberOfSymbols)
        {
            BitReader bitReader = new BitReader(reader.BaseStream, reader.BigEndian);
            int[] ret = new int[numberOfSymbols];

            if (bitReader.ReadBits(1) == 1)
                DecodeVariableWidth(bitReader, ret, codeTextLength);
            else
                DecodeFixedWidth(bitReader, ret, codeTextLength);

            return ret;
        }

        private static void DecodeVariableWidth(BitReader bitReader, int[] ret, int codeTextLength)
        {

        }

        private static void DecodeFixedWidth(BitReader bitReader, int[] ret, int codeTextLength)
        {
            byte minBits = bitReader.ReadBits(6);
            byte maxBits = bitReader.ReadBits(6);
            int minVal = bitReader.ReadIntBits(minBits);
            int maxVal = bitReader.ReadIntBits(maxBits);

            int range = (maxVal - minVal);
            if (range <= 0)
            {
                for (int i = 0; i < ret.Length; i++)
                    ret[i] = minVal;
            } else
            {
                byte fieldWidth = (byte)System.Runtime.Intrinsics.Arm.ArmBase.LeadingZeroCount(range);
                fieldWidth |= (byte)System.Runtime.Intrinsics.X86.Lzcnt.LeadingZeroCount((uint)range);
                fieldWidth = (byte)(32 - fieldWidth);

                for (int i = 0; i < ret.Length; i++)
                    ret[i] = bitReader.ReadIntBits(fieldWidth);
            }
        }
    }
}