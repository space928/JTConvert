using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JTConvert.JTCodec
{
    /// <summary>
    /// Dummy interface for all graph element structs to derive from to allow for
    /// somewhat polymorphic node graphs.
    /// </summary>
    public interface IJTGraphElement { }

    /// <summary>
    /// Dummy struct to represent an element with a an end-of-file guid. 
    /// Not in the official spec.
    /// </summary>
    public struct JTNullElement : IJTGraphElement { }

    public struct JTBaseNodeElement : IJTGraphElement
    {
        public JTLogicalElementHeader header;
        public JTBaseNodeData baseNodeData;
    }

    public struct JTPartitionNodeElement : IJTGraphElement
    {
        public JTLogicalElementHeader header;
        public JTGroupNodeData groupNodeData;
        public JTPartitionFlags partitionFlags;
        public string fileName;
        public BBoxF32 transformedBBox;
        public float area;
        public JTCountRange vertexCountRange;
        public JTCountRange nodeCountRange;
        public JTCountRange polygonCountRange;
        public BBoxF32 untransformedBBox;
    }

    /// <summary>
    /// A group node contains an ordered list of reference to other nodes (it's children).
    /// </summary>
    public struct JTGroupNodeElement : IJTGraphElement
    {
        public JTLogicalElementHeader header;
        public JTGroupNodeData groupNodeData;
    }

    /// <summary>
    /// An instance node stores a single reference to another node.
    /// </summary>
    public struct JTInstanceNodeElement : IJTGraphElement
    {
        public JTLogicalElementHeader header;
        public JTBaseNodeData baseNodeData;
        public byte versionNumber;
        public int childNodeObjectID;
    }

    /// <summary>
    /// Part nodes represent the root node for a particular part within an LSG.
    /// </summary>
    public struct JTPartNodeElement : IJTGraphElement
    {
        public JTLogicalElementHeader header;
        public JTMetaDataNodeData metadata;
        public byte version;
        // int32 empty field
    }

    /// <summary>
    /// Stores references to "late loaded" data such as properties and PMI.
    /// </summary>
    public struct JTMetaDataNodeElement : IJTGraphElement
    {
        public JTLogicalElementHeader header;
        public JTMetaDataNodeData metadata;
    }

    /// <summary>
    /// LOD nodes store alternative LODs of a subassembly.
    /// </summary>
    public struct JTLODNodeElement : IJTGraphElement
    {
        public JTLogicalElementHeader header;
        public JTLODNodeData lodNodeData;
    }

    /// <summary>
    /// LOD nodes store alternative LODs of a subassembly. Stores the range over which this LOD is
    /// to be used.
    /// </summary>
    public struct JTRangeLODNodeElement : IJTGraphElement
    {
        public JTLogicalElementHeader header;
        public JTLODNodeData lodNodeData;
        public byte versionNumber;
        public VecF32 rangeLimits;
        public CoordF32 centre;
    }

    /// <summary>
    /// Essentially the same as the GroupNodeElement but allows a single child to be selected to be displayed.
    /// </summary>
    public struct JTSwitchNodeElement : IJTGraphElement
    {
        public JTLogicalElementHeader header;
        public JTGroupNodeData groupNodeData;
        public byte versionNumber;
        public uint selectedChild;
    }

    /// <summary>
    /// Represents the simplest form of a shape node that can exist in the LSG.
    /// </summary>
    public struct JTBaseShapeNodeElement : IJTGraphElement
    {
        public JTLogicalElementHeader header;
        public JTBaseShapeData shapeData;
    }

    /// <summary>
    /// Represents shapes defined by a collection of vertices.
    /// </summary>
    public struct JTVertexShapeNodeElement : IJTGraphElement
    {
        public JTLogicalElementHeader header;
        public JTVertexShapeData vertexShapeData;
    }

    /// <summary>
    /// Represents shapes defined by a collection of of independant and unconnected triangle strips.
    /// </summary>
    public struct JTTriStripSetShapeNodeElement : IJTGraphElement
    {
        public JTLogicalElementHeader header;
        public JTVertexShapeData vertexShapeData;
    }

    public struct JTPolylineSetShapeNodeElement : IJTGraphElement
    {
        public JTLogicalElementHeader header;
        public JTVertexShapeData vertexShapeData;
        public byte versionNumber;
        /// <summary>
        /// Area Factor specifies a multiplier factor applied to a Polyline Set computed surface area. In JT data 
        /// viewer applications there may be LOD selection semantics that are based on screen coverage
        /// calculations. The so-called ”surface area” of a polyline is computed as if each line segment were a
        /// square. This Area Factor turns each edge into a narrow rectangle. Valid Area Factor values lie in the
        /// range (0,1].
        /// </summary>
        public float areaFactor;
    }

    public struct JTPointSetShapeNodeElement : IJTGraphElement
    {
        public JTLogicalElementHeader header;
        public JTVertexShapeData vertexShapeData;
        public byte versionNumber;
        public byte areaFactor;
        // If versionNumber == 1
        public ulong vertexBindings;
    }

    public struct JTPolygonSetShapeNodeElement: IJTGraphElement
    {
        public JTLogicalElementHeader header;
        public JTVertexShapeData vertexShapeData;
    }

    public struct JTNullShapeNodeElement : IJTGraphElement
    {
        public JTLogicalElementHeader header;
        public JTBaseShapeData shapeData;
        public byte version;
    }

    public struct JTPrimitiveSetShapeNodeElement : IJTGraphElement
    {
        public JTLogicalElementHeader header;
        public JTBaseShapeData shapeData;
        public byte version;
        public ulong vertexBindings;
        public JTTexCoordGenType texCoordGenType;
        public byte shapeVersion;
        public JTPrimitiveSetQuantizationParameters quantizationParameters;
    }

    #region Attribute Elements

    public struct JTMaterialAttributeElement : IJTGraphElement
    {
        public JTLogicalElementHeader header;
        public JTBaseAttributeData attributeData;
        public byte version;
        public JTMaterialAttributeDataFlags dataFlags;
        public RGBA ambient;
        public RGBA diffuseAlpha;
        public RGBA specular;
        public RGBA emission;
        public float shininess;
        public float reflectivity;
        public float bumpiness;
        public JTBaseAttributeDataFieldsV2 attributeDataFieldsV2;
    }

    public struct JTImageAttributeElement : IJTGraphElement
    {
        public JTLogicalElementHeader header;
        public JTBaseAttributeData attributeData;
        public byte version;
        public JTTextureDataV1 textureDataV1;
        public JTBaseAttributeDataFieldsV2 attributeDataFieldsV2;
    }

    public struct JTDrawStyleAttributeElement : IJTGraphElement
    {
        public JTLogicalElementHeader header;
        public JTBaseAttributeData attributeData;
        public byte version;
        public JTDrawStyleFlags drawStyleFlags;
        public JTBaseAttributeDataFieldsV2 attributeDataV2;
    }

    public struct JTLightSetAttributeElement : IJTGraphElement
    {
        public JTLogicalElementHeader header;
        public JTBaseAttributeData attributeData;
        public byte version;
        public int lightCount;
        public int[] lightObjectID;
        public JTBaseAttributeDataFieldsV2 attributeDataV2;
    }

    public struct JTLinestyleAttributeElement : IJTGraphElement
    {
        public JTLogicalElementHeader header;
        public JTBaseAttributeData attributeData;
        public sbyte version;
        public JTLinestyleFlags dataFlags;
        public float lineWidth;
        public JTBaseAttributeDataFieldsV2 attributeDataV2;
    }

    public struct JTPointstyleAttributeElement : IJTGraphElement
    {
        public JTLogicalElementHeader header;
        public JTBaseAttributeData attributeData;
        public sbyte version;
        public JTPointstyleFlags dataFlags;
        public float pointSize;
        public JTBaseAttributeDataFieldsV2 attributeDataV2;
    }

    public struct JTGeometricTransformAttributeElement : IJTGraphElement
    {
        public JTLogicalElementHeader header;
        public JTBaseAttributeData attributeData;
        public sbyte version;
        /// <summary>
        /// Bits in this field are set to true if the corresponding field of the matrix has been 
        /// modified from the identity matrix.<para/>
        /// Bits are assigned as follows:
        /// <code>
        /// [ 15  14  13  12 ]
        /// [ 11  10   9   8 ]
        /// [  7   6   5   4 ]
        /// [  3   2   1   0 ]
        /// </code>
        /// </summary>
        public ushort storedValuesMask;
        /// <summary>
        /// In the JT file this is encoded as an array of doubles containing only the fields 
        /// specified by the <seealso cref="storedValuesMask"/>.
        /// </summary>
        public Mx4F64 matrix;
    }

    public struct JTPaletteMapAttributeElement : IJTGraphElement
    {
        public JTLogicalElementHeader header;
        public JTBaseAttributeData attributeData;
        public sbyte version;
        public VecU32 paletteMapVector;
        public JTBaseAttributeDataFieldsV2 attributeDataV2;
    }

    public struct JTSabotV104AttributeElement : IJTGraphElement
    {
        public JTLogicalElementHeader header;
        // TODO:
    }

    public struct JTInfiniteLightAttributeElement : IJTGraphElement
    {
        public JTLogicalElementHeader header;
        public JTBaseLightData lightData;
        public sbyte version;
        public DirF32 direction;
    }

    public struct JTPointLightAttributeElement : IJTGraphElement
    {
        public JTLogicalElementHeader header;
        public JTBaseLightData lightData;
        public CoordF32 position;
        public JTAttenuationCoefficients attenuationCoefficients;
        /// <summary>
        /// Angle of the half beam in degrees. 
        /// </summary>
        public float spreadAngle;
        public DirF32 spotDirection;
        public int spotIntensity;
    }

    public struct JTMappingPlaneAttributeElement : IJTGraphElement
    {
        public JTLogicalElementHeader header;
        public byte version;
        public Mx4F64 mappingPlaneMatrix;
        public JTCoordSystem coordSystem;
    }

    public struct JTMappingCylinderAttributeElement : IJTGraphElement
    {
        public JTLogicalElementHeader header;
        public byte version;
        public Mx4F64 mappingCylinderMatrix;
        public JTCoordSystem coordSystem;
    }

    public struct JTMappingSphereAttributeElement : IJTGraphElement
    {
        public JTLogicalElementHeader header;
        public byte version;
        public Mx4F64 mappingSphereMatrix;
        public JTCoordSystem coordSystem;
    }

    public struct JTMappingTriplanarAttributeElement : IJTGraphElement
    {
        public JTLogicalElementHeader header;
        public byte version;
        public Mx4F64 mappingTriplanarMatrix;
        public JTCoordSystem coordSystem;
    }

    #endregion

    #region Property Atom Elements

    public struct JTBasePropertyAtomElement : IJTGraphElement
    {
        public JTLogicalElementHeader header;
        public JTBasePropertyAtomData atomData;
    }

    public struct JTStringPropertyAtomElement : IJTGraphElement
    {
        public JTLogicalElementHeader header;
        public JTBasePropertyAtomData atomData;
        public byte version;
        public string value;
    }

    public struct JTIntegerPropertyAtomElement : IJTGraphElement
    {
        public JTLogicalElementHeader header;
        public JTBasePropertyAtomData atomData;
        public byte version;
        public int value;
    }

    public struct JTFloatPropertyAtomElement : IJTGraphElement
    {
        public JTLogicalElementHeader header;
        public JTBasePropertyAtomData atomData;
        public byte version;
        public float value;
    }

    public struct JTObjectReferencePropertyAtomElement : IJTGraphElement
    {
        public JTLogicalElementHeader header;
        public JTBasePropertyAtomData atomData;
        public byte version;
        public int objectID;
    }

    public struct JTDatePropertyAtomElement : IJTGraphElement
    {
        public JTLogicalElementHeader header;
        public JTBasePropertyAtomData atomData;
        public byte version;
        public short year;
        public short month;
        public short day;
        public short hour;
        public short minute;
        public short second;
    }

    public struct JTLateLoadedPropertyAtomElement : IJTGraphElement
    {
        public JTLogicalElementHeader header;
        public JTBasePropertyAtomData atomData;
        public byte version;
        public GUID segmentID;
        public JTSegmentType segmentType;
        public int payloadObjectID;
        /// <summary>
        /// Must be >= 1
        /// </summary>
        public int reserved;
    }

    public struct JTVector4FPropertyAtomElement : IJTGraphElement
    {
        public JTLogicalElementHeader header;
        public JTBasePropertyAtomData atomData;
        public byte version;
        public float valueX;
        public float valueY;
        public float valueZ;
        public float valueW;
    }

    #endregion

    #region Element Property Table

    public struct JTPropertyTable
    {
        public short version;
        public int elementPropertyTableCount;
        public JTElementPropertyTableItem[] propertyTables;
    }

    public struct JTElementPropertyTableItem
    {
        public int elementObjectID;
        public Dictionary<int, int> elemntPropertyTable;
    }

    #endregion

    #region Data Structs

    public struct JTBaseNodeData
    {
        public byte versionNumber;
        public JTNodeFlags nodeFlags;
        public int[] attributeObjectIDs;
    }

    public struct JTBaseAttributeData
    {
        public byte version;
        public JTBaseAttributeDataFieldsV1 attributeDataV1;
    }

    /// <summary>
    /// Stores an ordered list of references to children.
    /// </summary>
    public struct JTGroupNodeData
    {
        public JTBaseNodeData baseNodeData;
        public byte versionNumber;
        public int[] childNodeObjectIDs;
    }

    public struct JTMetaDataNodeData
    {
        public JTGroupNodeData groupNodeData;
        public byte versionNumber;
    }

    public struct JTLODNodeData
    {
        public JTGroupNodeData groupNodeData;
        public byte versionNumber;
    }

    public struct JTBaseShapeData
    {
        public JTBaseNodeData baseNodeData;
        public byte versionNumber;
        /// <summary>
        /// Untransformed axis aligned bounding box of the geometry contained within the shape node.
        /// </summary>
        public BBoxF32 untrasformedBBox;
        /// <summary>
        /// Total surface area of all the children of this node.
        /// </summary>
        public float area;
        /// <summary>
        /// Stores the smallest and largest number of vertices that this node can contain.
        /// </summary>
        public JTCountRange vertexCountRange;
        public JTCountRange nodeCountRange;
        public JTCountRange polygonCountRange;
        /// <summary>
        /// Size in memory (in bytes) of the referenced shape LOD element. Set to 0 if this is unknown.
        /// </summary>
        public uint size;
        /// <summary>
        /// Stores the qualitative compression ratio applied to the referenced shape node.
        /// </summary>
        public float compressionLevel;
    }

    public struct JTVertexShapeData
    {
        public JTBaseShapeData shapeData;
        public byte versionNumber;
        /// <summary>
        /// Stores a collection of of normal, texcoord, and colour binding information in a single ulong.
        /// </summary>
        public ulong vertexBinding;
    }

    public struct JTPrimitiveSetQuantizationParameters
    {
        public byte bitsPerVertex;
        public byte bitsPerColour;
    }

    public struct JTBaseAttributeDataFieldsV1
    {
        public JTAttributeStateFlags stateFlags;
        public JTFieldInhibitFlags fieldInhibitFlags;
        public JTFieldFinalFlags fieldFinalFlags;
    }

    public struct JTBaseAttributeDataFieldsV2
    {
        public uint palletteIndex;
    }

    public struct JTTextureDataV1
    {
        public JTTextureType textureType;
        public JTTextureEnvironment textureEnvironment;
        public JTTexCoordGenParameters texCoordGenParameters;
        public int textureChannel;
        public int texCoordChannel;
        public uint empty;
        public byte inlineImageStorageFlag;
        public int imageCount;
        public JTInlineTextureImageData[] imageData;
        public string[] externalImageData;
    }

    public struct JTTextureEnvironment
    {
        public JTBorderMode borderMode;
        public JTMipmapMagnificationFilter mipmapMagnificationFilter;
        public JTMipmapMinificationFilter mipmapMinificationFilter;
        public JTTextureDimensionWrapMode sDimensionWrapMode;
        public JTTextureDimensionWrapMode tDimensionWrapMode;
        public JTTextureDimensionWrapMode rDimensionWrapMode;
        public JTTextureBlendType blendType;
        public JTInternalTextureCompressionLevel compressionLevel;
        public RGBA blendColour;
        public RGBA borderColour;
        public Mx4F32 textureTransform;
    }

    public struct JTTexCoordGenParameters
    {
        public JTTexCoordGenMode sCoordGenMode;
        public JTTexCoordGenMode tCoordGenMode;
        public JTTexCoordGenMode rCoordGenMode;
        public JTTexCoordGenMode qCoordGenMode;
        public PlaneF32 sTexCoordRefPlane;
        public PlaneF32 tTexCoordRefPlane;
        public PlaneF32 rTexCoordRefPlane;
        public PlaneF32 qTexCoordRefPlane;
    }

    public struct JTInlineTextureImageData
    {
        public JTImageFormatDescription imageFormatDescription;
        public int totalImageDataSize;
        public JTInlineTextureMipmapData[] mipMaps;
    }

    public struct JTInlineTextureMipmapData
    {
        public int mipmapImageByteCount;
        public byte[] mipmapTexelData;
    }

    public struct JTImageFormatDescription
    {
        public JTPixelFormat pixelFormat;
        public JTPixelDataType pixelDataType;
        public JTTextureDimensionality dimensionality;
        public short rowAlignment;
        public short width;
        public short height;
        public short depth;
        public short numberBorderTexels;
        public JTSharedImageFlag sharedImageFlag;
        public short mipmapsCount;
        public JTBaseAttributeDataFieldsV2 attributeDataFieldsV2;
    }

    public struct JTBaseLightData
    {
        public sbyte version;
        public JTLogicalElementHeader header;
        public RGBA ambientColour;
        public RGBA diffuseColour;
        public RGBA specularColour;
        public float brightness;
        public JTCoordSystem coordSystem;
        public JTShadowCasterFlag shadowCasterFlag;
        public float shadowOpacity;
        public float nonShadowAlphaFactor;
        public float shadowAlphaFactor;
    }

    public struct JTAttenuationCoefficients
    {
        public float constantAttenuation;
        public float linearAttenuation;
        public float quadraticAttenuation;
    }

    public struct JTCountRange
    {
        public int minCount;
        public int maxCount;
    }

    public struct JTBasePropertyAtomData
    {
        public byte version;
        public JTAtomDataStateFlags stateFlags;
    }

#endregion

    #region Enums

    public enum JTTexCoordGenType
    {
        /// <summary>
        /// The texture will be stretched to fit the face/feature
        /// </summary>
        SingleTile = 0, 
        /// <summary>
        /// The texture will be repeated accross the face/feature such that the texels remain square
        /// </summary>
        Isotropic = 1
    }

    [Flags]
    public enum JTPartitionFlags : int
    {
        None = 0,
        UntrasformedBBox = 1,
    }

    [Flags]
    public enum JTNodeFlags : uint
    {
        None = 0,
        /// <summary>
        /// Algorithms traversing the LSG should skip this node and it's children.
        /// </summary>
        Ignore = 1,
    }

    public enum JTCompressionAlgorithm
    {
        None = 1,
        ZLib = 2,
        LZMA = 3,
    }

    [Flags]
    public enum JTAttributeStateFlags : byte
    {
        Unused = 1,
        /// <summary>
        /// Accumulation of this attribute is forced, the ancestor's final flag is overridden.
        /// </summary>
        AccumulationForce = 2,
        /// <summary>
        /// Attribute should be ignored.
        /// </summary>
        AccumulationIgnore = 4,
        /// <summary>
        /// Attribute should be persistable to a JT file.
        /// </summary>
        AttributePersistable = 8,
    }

    [Flags]
    public enum JTFieldInhibitFlags : uint
    {
        // Material Attribute Flags
        AmbientCommonRGBValue = 1,
        AmbientColour = 1,
        SpecularCommonRGBValue = 2,
        SpecularColour = 2,
        EmissionCommonRGBValue = 4,
        EmissionColour = 4,
        BindingFlag = 8,
        SourceBlendingFactor = 8,
        DestinationBlendingFactor = 8,
        OverrideVertexColour = 16,
        MaterialReflectivity = 32,
        DiffuseColour = 64,
        DiffuseAlpha = 128,

        // Texture Image Attribute Flags
        TextureType = 1,
        MipMapImageTexelData = 1,
        TextureName = 1,
        SharedImageFlag = 1,
        BorderMode = 2,
        BorderColour = 2,
        MipmapMinificationFilter = 4,
        MipmapMagnificationFilter = 4,
        SDimensionWrapMode = 8,
        TDimensionWrapMode = 8,
        RDimensionWrapMode = 8,
        BlendType = 16,
        BlendColour = 16,
        TextureTransform = 32,
        TexCoordGenMode = 64,
        TexCoordReferencePlane = 64,
        InternalCompressionLevel = 128,

        // Draw Style Attribute Flags
        TwoSidedLighting = 1,
        BackFaceCulling = 2,
        OutlinedPolygons = 4,
        LightingEnabled = 8,
        FlatShading = 16,
        SeparateSpecular = 32,
    }

    [Flags]
    public enum JTFieldFinalFlags : uint
    {
        //TODO:
    }

    [Flags]
    public enum JTMaterialAttributeDataFlags : ushort
    {
        /// <summary>
        /// Enable OpenGL blending.
        /// </summary>
        Blending = 0x0010,
        /// <summary>
        /// Whether to override the vertex colours of the shape with this attribute.
        /// </summary>
        OverrideVertexColour = 0x0020,
        // Source blending factors
        Src_GL_ZERO = 0 << 6,
        Src_GL_ONE = 1 << 6,
        Src_GL_SRC_COLOUR = 2 << 6,
        Src_GL_DST_COLOUR = 3 << 6,
        Src_GL_ONE_MINUS_SRC_COLOUR = 4 << 6,
        Src_GL_ONE_MINUS_DST_COLOUR = 5 << 6,
        Src_GL_SRC_ALPHA = 6 << 6,
        Src_GL_ONE_MINUS_SRC_ALPHA = 7 << 6,
        Src_GL_DST_ALPHA = 8 << 6,
        Src_GL_ONE_MINUS_DST_ALPHA = 9 << 6,
        Src_GL_SRC_ALPHA_SATURATE = 10 << 6,
        // Destination blending factors
        Dst_GL_ZERO = 0 << 11,
        Dst_GL_ONE = 1 << 11,
        Dst_GL_SRC_COLOUR = 2 << 11,
        Dst_GL_DST_COLOUR = 3 << 11,
        Dst_GL_ONE_MINUS_SRC_COLOUR = 4 << 11,
        Dst_GL_ONE_MINUS_DST_COLOUR = 5 << 11,
        Dst_GL_SRC_ALPHA = 6 << 11,
        Dst_GL_ONE_MINUS_SRC_ALPHA = 7 << 11,
        Dst_GL_DST_ALPHA = 8 << 11,
        Dst_GL_ONE_MINUS_DST_ALPHA = 9 << 11,
        Dst_GL_SRC_ALPHA_SATURATE = 10 << 11,
    }

    public enum JTTextureType : int
    {
        None = 0,
        OneDimPostLit = 1,
        TwoDimPostLit = 2,
        ThreeDimPostLit = 3,
        TwoDimNormalMap = 4,
        CubePostLit = 5,
        CubePreLit = 7,
        OneDimPreLit = 8,
        TwoDimPreLit = 9,
        ThreeDimPreLit = 10,
        CubeEnvironmentMap = 11,
        OneDimGlossMap = 12,
        TwoDimGlossMap = 13,
        ThreeDimGlossMap = 14,
        CubeGlossMap = 15,
        TwoDimBumpMap = 16,
        TwoDimWorldSpaceNormalMap = 17,
        TwoDimSphereEnvMap = 18,
        TwoDimLatLongEnvMap = 19,
        TwoDimSphereDiffuseMap = 20,
        CubeDiffuseMap = 21,
        TwoDimLatLongDiffuseMap = 22,
        TwoDimSphereSpecularMap = 23,
        CubeSpecularMap = 24,
        TwoDimLatLongSpecularMap = 25,
        ResetTextureState = 26,
    }

    public enum JTInlineImageStorageFlag : byte
    {
        External = 0,
        Internal = 1
    }

    public enum JTBorderMode : int
    {
        None = 0,
        /// <summary>
        /// Constant border colour taken from the borderColour field.
        /// </summary>
        Constant = 1,
        /// <summary>
        /// Use the border texel ring in the the texture image definition.
        /// </summary>
        Explicit = 2,
    }

    public enum JTMipmapMagnificationFilter : int
    {
        None = 0,
        Nearest = 1,
        Linear = 2,
    }

    public enum JTMipmapMinificationFilter : int
    {
        None = 0,
        Nearest = 1,
        Linear = 2,
        NearestInMipmap = 3,
        LinearInMipmap = 4,
        NearestBetweenMipmaps = 5,
        LinearBetweenMipmaps = 6,
    }

    public enum JTTextureDimensionWrapMode : int
    {
        None = 0,
        Clamp = 1,
        Repeat = 2,
        Mirror = 3,
        /// <summary>
        /// Border is always ignored and the edge texels are repeated.
        /// </summary>
        ClampToEdge = 4,
        /// <summary>
        /// Nearest border texel is chosen to be repeated outisde the mapping range.
        /// </summary>
        ClampToBorder = 5,
    }

    public enum JTTextureBlendType : int
    {
        None = 0,
        Decal = 1,
        Modulate = 2,
        Replace = 3,
        Blend = 4,
        Add = 5,
        Combine = 6
    }

    public enum JTInternalTextureCompressionLevel : int
    {
        None = 0,
        /// <summary>
        /// Lossless compression of texture data.
        /// </summary>
        Conservative = 1,
        /// <summary>
        /// Texels truncated to 8 bits each.
        /// </summary>
        Moderate = 2,
        /// <summary>
        /// Texels truncated to 4 bits each (5 bits for RGB images)
        /// </summary>
        Aggressive = 3,
    }

    public enum JTTexCoordGenMode : int
    {
        None = 0,
        ModelCoordinateSystemLinear = 1,
        ViewCoordinateSystemLinear = 2,
        SphereMap = 3,
        ReflectionMap = 4,
        NormalMap = 5,
    }

    public enum JTPixelFormat : uint
    {
        None = 0,
        RGB = 1,
        RGBA = 2,
        Lum = 3,
        LumA = 4,
        Stencil = 5,
        Depth = 6,
        Red = 7,
        Green = 8,
        Blue = 9,
        Alpha = 10,
        BGR = 11,
        BGRA = 12,
        DepthStencil = 13
    }

    public enum JTPixelDataType : uint
    {
        None = 0,
        Signed8Bit = 1,
        Float32Bit = 2,
        Unsigned8Bit = 3,
        BitsInUnsignedByte = 4,
        Unsigned16Bit = 5,
        Signed16Bit = 6,
        Unsigned32Bit = 7,
        Signed32Bit = 8,
        /// <summary>
        /// IEEE-754, 1 sign bit, 5 exponent bits, 10 mantissa bits
        /// </summary>
        Float16Bit = 9,
    }

    public enum JTTextureDimensionality : short
    {
        OneDimension = 1,
        TwoDimensions = 2,
        ThreeDimensions = 3,
    }

    public enum JTSharedImageFlag : uint
    {
        NotShareable = 0,
        Shareable = 1
    }

    [Flags]
    public enum JTDrawStyleFlags : byte
    {
        BackFaceCulling = 0x01,
        TwoSidedLighting = 0x02,
        /// <summary>
        /// Wireframe display.
        /// </summary>
        OutlinedPolygons = 0x04,
        LightingEnabled = 0x08,
        FlatShading = 0x10,
        SeparateSpecular = 0x20,
    }

    [Flags]
    public enum JTLinestyleFlags : byte
    {
        // Line type
        LineSolid = 0,
        LineDash = 1,
        LineDot = 2,
        LineDashDot = 3,
        LineDashDotDot = 4,
        LongDash = 5,
        CentreDash = 6,
        CentreDashDash = 7,

        // Antialiasing flag
        AntiAliasDisable = 0,
        AntiAliasEnabled = 1 << 4,
    }

    [Flags]
    public enum JTPointstyleFlags : byte
    {
        PointTypeDefault = 0,

        // Antialiasing flag
        AntiAliasDisable = 0,
        AntiAliasEnabled = 1 << 4,
    }

    public enum JTCoordSystem : int
    {
        Viewpoint = 1,
        Model = 2,
        World = 3
    }

    public enum JTShadowCasterFlag : byte
    {
        DontCastShadows = 0,
        ShadowCaster = 1,
    }

    [Flags]
    public enum JTAtomDataStateFlags : uint
    {
        // Low bits (0-7) are free for the application to use; all other bits are reserved for
        // future extension.
        Default = 0x40000000
    }

    #endregion
}
