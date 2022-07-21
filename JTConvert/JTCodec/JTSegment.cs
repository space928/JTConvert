namespace JTConvert.JTCodec
{
    /// <summary>
    /// Encapsulates any chunk of data within a JT file.
    /// </summary>
    public abstract class JTSegment : IJTSegmentLoader
    {
        /// <summary>
        /// Globally unique identifier of this segment.
        /// </summary>
        public GUID segmentID;
        public JTSegmentType segmentType;
        public int length;
        public JTLogicalElementHeader logicalElementHeader;
        public abstract bool Compressable { get; }

        /// <summary>
        /// Loads the Logical Element Header and decompresses the block if needed. 
        /// Replaces the reader with one using the decompressed stream if needed.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="compressable"></param>
        /// <param name="logicalElementHeader"></param>
        internal static void LoadLEHeader(ref BinaryJTReader reader, bool compressable, ref JTLogicalElementHeader logicalElementHeader)
        {
            if (compressable)
            {
                logicalElementHeader.compressed = reader.ReadUInt32() > 1;
                logicalElementHeader.compressedDataLength = reader.ReadInt32();
                logicalElementHeader.compressionAlgorithm = (JTCompressionAlgorithm)reader.ReadByte();
            }
            // Annoyingly in JT, if the compression flag is set then the rest of the header needs to be decompressed
            if (logicalElementHeader.compressed)
                reader = reader.DecompressIntoNewReader(logicalElementHeader.compressedDataLength, logicalElementHeader.compressionAlgorithm);

            //Logger.Log($"Buff pos: {reader.BaseStream.Position} compressed: {logicalElementHeader.compressed}", Logger.VerbosityLevel.DEBUG);
            logicalElementHeader.elementLength = reader.ReadInt32();
            logicalElementHeader.objectTypeID = reader.ReadGUID();
            logicalElementHeader.objectBaseType = (JTObjectBaseType)reader.ReadByte();
            logicalElementHeader.objectID = reader.ReadInt32();
        }
    }
}
