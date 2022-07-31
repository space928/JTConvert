namespace JTConvert.JTCodec
{
    #region Shape LOD Elements

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

    public struct JTNullShapeLODElement : IJTShapeLODElement
    {
        public JTLogicalElementHeader header;
        public byte version;
        public BBoxF32 untransformedBBox;
    }

    public struct JTPrimitiveSetShapeElement : IJTShapeLODElement
    {
        public JTLogicalElementHeader header;
        public byte baseShapeVersion;
        public byte basePrimSetVersion;
        public JTVertexBindings vertexBindings;
        public JTTexCoordGenType texcoordGenType;
        public byte version;
        public int bitsPerVertex;
        public JTLossyQuantizedPrimitiveSetData lossySetData;
        public JTLosslessCompressedPrimitiveSetData losslessSetData;
    }

    #endregion

    #region Shape Data

    public struct JTVertexShapeLODData
    {
        public JTBaseShapeLODData baseData;
        public sbyte version;
        public JTVertexBindings vertexBindings;
        public JTLogicalElementHeader header;
        /// <summary>
        /// Compressed TopoMesh data. Exists on most ShapeNode elements,
        /// with the exception of <seealso cref="JTTriStripSetShapeLODElement"/>.
        /// </summary>
        public JTTopoMeshCompressedLODData meshDataCompressed;
        /// <summary>
        /// Topologically compressed TopoMesh data. Only exists on 
        /// <seealso cref="JTTriStripSetShapeLODElement"/>
        /// </summary>
        public JTTopoMeshTopologicallyCompressedLODData meshDataTopologicallyCompressed;
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
        public JTTopoMeshLODData meshLODData;
        public byte version;
        public JTTopoMeshTopologicallyCompressedRepData data;
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
        public uint numberOfFaceGroupListIndices;
        public uint numberOfPrimitiveListIndices;
        public uint numberOfVertexListIndices;
        public int[] faceGroupListIndices;
        public int[] primitiveListIndices;
        public int[] vertexListIndices;
        public int fgpvListIndicesHash;
        public JTVertexBindings vertexBindings;
        public JTQuantizationParameters quantizationParameters;
        public int numberOfVertexRecords;
        public int[] uniqueVertexRecordCoordinateLengths;
        public int uniqueVertexListMapHash;
        public JTCompressedVertexArray vertices;
        public JTCompressedNormalArray normals;
        public JTCompressedColourArray colours;
        public JTCompressedTexcoordArray texcoords;
        public JTCompressedVertexFlagArray vertexFlags;
        public JTCompressedFieldArray auxiliaryFields;
    }

    public struct JTTopoMeshTopologicallyCompressedRepData
    {
        public int[] faceDegreesA;
        public int[] faceDegreesB;
        public int[] faceDegreesC;
        public int[] faceDegreesD;
        public int[] faceDegreesE;
        public int[] faceDegreesF;
        public int[] faceDegreesG;
        public int[] faceDegreesH;
        public int[] vertexValences;
        public int[] vertexFlags;
        public int[] faceAttributeMasksA;
        public int[] faceAttributeMasksB;
        public int[] faceAttributeMasksC;
        public int[] faceAttributeMasksD;
        public int[] faceAttributeMasksE;
        public int[] faceAttributeMasksF;
        public int[] faceAttributeMasksG;
        public int[] faceAttributeMasksH;
        public int[] faceAttributeMask8;
        public uint[] highDegreeFaceAttributeMasks;
        public int[] splitFaceSyms;
        public int[] splitFacePositions;
        public uint compositeHash;
        public JTTopologicallyCompressedVertexRecords compressedVertexRecords;
    }

    public struct JTTopologicallyCompressedVertexRecords
    {
        public JTVertexBindings vertexBindings;
        public JTQuantizationParameters quantizationParameters;
        public int numberOfTopoVerts;
        public int numberOfVertAttributes;
        public JTCompressedVertexArray vertices;
        public JTCompressedNormalArray normals;
        public JTCompressedColourArray colours;
        public JTCompressedTexcoordArray texcoords0;
        public JTCompressedTexcoordArray texcoords1;
        public JTCompressedTexcoordArray texcoords2;
        public JTCompressedTexcoordArray texcoords3;
        public JTCompressedTexcoordArray texcoords4;
        public JTCompressedTexcoordArray texcoords5;
        public JTCompressedTexcoordArray texcoords6;
        public JTCompressedTexcoordArray texcoords7;
        public JTCompressedVertexFlagArray vertexFlags;
        public JTCompressedFieldArray auxiliaryFields;
    }

    public struct JTLosslessCompressedPrimitiveSetData
    {
        public int uncompressedDataSize;
        public int compressedDataSize;
        public byte[] compressedPrimitiveData;
        public byte[] primitiveData;
    }

    public struct JTLossyQuantizedPrimitiveSetData
    {
        public int primitiveCount;
        public JTLossyQuantizedPrimitiveParams[] primitiveParams;
        public uint bitsPerColour;
        // Bottom of page 129
        /*public JTCompressedParams1 compressedParams1;
        public JTCompressedParams3 compressedParams3;
        public JTCompressedParams2 compressedParams2;
        public JTCompressedColours compressedColours;*/
        public JTPrimitiveType compressedTypes;
    }

    public struct JTLossyQuantizedPrimitiveParams
    {
        public Quaternion params3;
        public CoordF32 params1;
        public DirF32 params2;
        public RGB colour;
        public JTPrimitiveType type;
    }

    public struct JTCompressedVertexArray
    {

    }

    public struct JTCompressedNormalArray
    {

    }

    public struct JTCompressedColourArray
    {

    }

    public struct JTCompressedTexcoordArray
    {

    }

    public struct JTCompressedVertexFlagArray
    {

    }

    public struct JTCompressedFieldArray
    {

    }

    #endregion

    #region Enums

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

    public enum JTPrimitiveType : int
    {
        Box = 0,
        Cylinder = 1,
        Pyramid = 2,
        Sphere = 3,
        TriPrism = 4,
    }

    #endregion
}