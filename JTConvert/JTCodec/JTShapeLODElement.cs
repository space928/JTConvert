namespace JTConvert.JTCodec
{
    public interface IJTShapeLODElement
    {

    }

    public struct JTTriStripSetShapeLODElement : IJTShapeLODElement
    {
        public JTLogicalElementHeader header;
        public JTVertexShapeLODData vertexShapeLODData;
        public byte version;
    }

    public struct JTPolylineSetShapeLODElement : IJTShapeLODElement
    {
        public JTLogicalElementHeader header;
        public JTVertexShapeLODData vertexShapeLODData;
        public byte version;
    }

    public struct JTPointSetShapeLODElement : IJTShapeLODElement
    {
        public JTLogicalElementHeader header;
        public JTVertexShapeLODData vertexShapeLODData;
        public byte version;
    }

    public struct JTPolygonSetShapeLODElement : IJTShapeLODElement
    {
        public JTLogicalElementHeader header;
        public JTVertexShapeLODData vertexShapeLODData;
        public byte version;
    }

    public struct JTVertexShapeLODData
    {
        public JTBaseShapeLODData baseData;
        public sbyte version;
        public JTVertexBindings vertexBindings;
        public JTLogicalElementHeader header;
        public JTTopoMeshCompressedLODData meshData;
        public JTTopoMeshTopologicallyCompressedLODData meshDataCompressed;
    }

    public struct JTBaseShapeLODData
    {
        public byte version;
    }

    public struct JTTopoMeshCompressedLODData
    {
        public JTTopoMeshLODData meshLODData;
        public byte version;
        public JTTopoMeshCompressedRepData data;
    }

    public struct JTTopoMeshTopologicallyCompressedLODData
    {

    }

    public struct JTTopoMeshLODData
    {
        public byte version;
        /// <summary>
        /// Identifier for the vertex records associated with this object.
        /// </summary>
        public uint vertexRecordsObjectID;
    }

    public struct JTTopoMeshCompressedRepData
    {

    }

    [Flags]
    public enum JTVertexBindings : ulong
    {
        TwoComponentVertexCoords = 1ul,
        ThreeComponentVertexCoords = 1ul << 1,
        FourComponentVertexCoords = 1ul << 2,

        NormalBinding = 1ul << 3,

        ThreeComponentVColour = 1ul << 4,
        FourComponentVColour = 1ul << 5,

        VertexFlagBinding = 1ul << 6,

        OneComponentTexcoord0 = 1ul << 8,
        TwoComponentTexcoord0 = 1ul << 9,
        ThreeComponentTexcoord0 = 1ul << 10,
        FourComponentTexcoord0 = 1ul << 11,

        OneComponentTexcoord1 = 1ul << 12,
        TwoComponentTexcoord1 = 1ul << 13,
        ThreeComponentTexcoord1 = 1ul << 14,
        FourComponentTexcoord1 = 1ul << 15,

        OneComponentTexcoord2 = 1ul << 16,
        TwoComponentTexcoord2 = 1ul << 17,
        ThreeComponentTexcoord2 = 1ul << 18,
        FourComponentTexcoord2 = 1ul << 19,

        OneComponentTexcoord3 = 1ul << 20,
        TwoComponentTexcoord3 = 1ul << 21,
        ThreeComponentTexcoord3 = 1ul << 22,
        FourComponentTexcoord3 = 1ul << 23,

        OneComponentTexcoord4 = 1ul << 24,
        TwoComponentTexcoord4 = 1ul << 25,
        ThreeComponentTexcoord4 = 1ul << 26,
        FourComponentTexcoord4 = 1ul << 27,

        OneComponentTexcoord5 = 1ul << 28,
        TwoComponentTexcoord5 = 1ul << 29,
        ThreeComponentTexcoord5 = 1ul << 30,
        FourComponentTexcoord5 = 1ul << 31,

        OneComponentTexcoord6 = 1ul << 32,
        TwoComponentTexcoord6 = 1ul << 33,
        ThreeComponentTexcoord6 = 1ul << 34,
        FourComponentTexcoord6 = 1ul << 35,

        OneComponentTexcoord7 = 1ul << 36,
        TwoComponentTexcoord7 = 1ul << 37,
        ThreeComponentTexcoord7 = 1ul << 38,
        FourComponentTexcoord7 = 1ul << 39,

        AuxiliaryVertexFieldBinding = 1ul << 63
    }
}