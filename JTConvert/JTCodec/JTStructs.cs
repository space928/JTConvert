using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JTConvert.JTCodec
{
    public class JTFile
    {
        public string version;
        public int versionMajor;
        /// <summary>
        /// False is LSB first, True is MSB first.
        /// </summary>
        public bool byteOrder;
        public int emptyField;
        /// <summary>
        /// Table of contents.
        /// </summary>
        public Dictionary<GUID, JTSegment> toc;
        public GUID lsgSegmentID;
        public JTSegment lsgSegment;
    }

    /// <summary>
    /// Internal struct for deserialising a TOC entry into.
    /// </summary>
    public struct JTTOCEntry
    {
        public GUID guid;
        public ulong offset;
        public uint length;
        public uint attributes;
    }

    public struct JTLogicalElementHeader
    {
        public bool compressed;
        public int compressedDataLength;
        public JTCompressionAlgorithm compressionAlgorithm;
        public int elementLength;
        /// <summary>
        /// Globally unique identifier for the object type.
        /// </summary>
        public GUID objectTypeID;
        public JTObjectBaseType objectBaseType;
        /// <summary>
        /// Identifier for this object. 
        /// Used by other objects to reference this object.
        /// </summary>
        public int objectID;
    }

    /// <summary>
    /// The type of sgement stored in this JTSegment.
    /// All segment types other than any of the "Shape" types support compression.
    /// </summary>
    public enum JTSegmentType
    {
        Undefined = 0,
        LogicalSceneGraph = 1,
        JTBRep = 2,
        PMIData = 3,
        MetaData = 4,
        /// <summary>
        /// Use this type (as opposed to one of the LOD shape types) when:
        ///  * The shape is not a descendant of a LOD node
        ///  * The shape us referenced by more than one LOD node
        ///  * The shape has built-in LODs
        ///  * The shape has no definite LOD
        /// </summary>
        Shape = 6,
        ShapeLOD0 = 7,
        ShapeLOD1 = 8,
        ShapeLOD2 = 9,
        ShapeLOD3 = 10,
        ShapeLOD4 = 11,
        ShapeLOD5 = 12,
        ShapeLOD6 = 13,
        ShapeLOD7 = 14,
        ShapeLOD8 = 15,
        ShapeLOD9 = 16,
        XTBRep = 17,
        WireframeRepresentation = 18,
        ULP = 19,
        STT = 23,
        LWPA = 24,
        MultiXTBRep = 30,
        InfoSegment = 31,
        AECShape = 32,
        STEPBRep = 33,
    }

    public enum JTObjectBaseType : byte
    {
        None = 255,
        BaseGraphNodeObject = 0,
        GroupGraphNodeObject = 1,
        ShapeGraphNodeObject = 2,
        BaseAttributeObject = 3,
        ShapeLOD = 4,
        BasePropertyObject = 5,
        JTObjectReferenceObject = 6,
        JTLateLoadedPropertyObject = 8,
        JTBase = 9,
    }

    #region Composite Data Types
    public struct BBoxF32
    {
        public CoordF32 minCorner;
        public CoordF32 maxCorner;
    }

    public struct CoordF32
    {
        public float x, y, z;
    }

    public struct DirF32
    {
        public float x, y, z;
    }

    public struct GUID
    {
        public uint a;
        public ushort b, c;
        public byte d, e, f, g, h, i, j, k;
        internal static readonly GUID Empty = new();

        public GUID(uint a, int b, int c, int d, int e, int f, int g, int h, int i, int j, int k)
        {
            this.a = a;
            this.b = (ushort)b;
            this.c = (ushort)c;
            this.d = (byte)d;
            this.e = (byte)e;
            this.f = (byte)f;
            this.g = (byte)g;
            this.h = (byte)h;
            this.i = (byte)i;
            this.j = (byte)j;
            this.k = (byte)k;
        }

        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            GUID o = (GUID)obj;
            return a == o.a
                && b == o.b
                && c == o.c
                && d == o.d
                && e == o.e
                && f == o.f
                && g == o.g
                && h == o.h
                && i == o.i
                && j == o.j
                && k == o.k;
        }

        public static bool operator ==(GUID left, GUID right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(GUID left, GUID right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash = (hash * 17) ^ (int)a ^ (int)(a>>32);
            hash = (hash * 17) ^ b;
            hash = (hash * 17) ^ c;
            hash = (hash * 17) ^ d;
            hash = (hash * 17) ^ e;
            hash = (hash * 17) ^ f;
            hash = (hash * 17) ^ g;
            hash = (hash * 17) ^ h;
            hash = (hash * 17) ^ e;
            hash = (hash * 17) ^ i;
            hash = (hash * 17) ^ j;
            return (hash * 17) ^ k;
        }

        public override string ToString()
        {
            return $"GUID[0x{a:x8}, 0x{b:x4}, 0x{c:x4}, 0x{d:x2}, 0x{e:x2}, 0x{f:x2}, 0x{g:x2}, 0x{h:x2}, 0x{i:x2}, 0x{j:x2}, 0x{k:x2}]";
        }
    }

    public struct MbString
    {
        public int count;
        public ushort[] chars;
    }

    public struct Mx4F32
    {
        public float m00, m01, m02, m03,
            m10, m11, m12, m13,
            m20, m21, m22, m23,
            m30, m31, m32, m33;
    }

    public struct Mx4F64
    {
        public double m00, m01, m02, m03,
            m10, m11, m12, m13,
            m20, m21, m22, m23,
            m30, m31, m32, m33;
    }

    public struct PlaneF32
    {
        public float a, b, c, d;
    }

    public struct Quaternion
    {
        public float x, y, z, w;
    }

    public struct RGB
    {
        public float r, g, b;
    }

    public struct RGBA
    {
        public float r, g, b, a;
    }

    public struct String
    {
        public int count;
        public char[] chars;
    }

    public struct VecF32
    {
        public int count;
        public float[] data;
    }

    public struct VecF64
    {
        public int count;
        public double[] data;
    }

    public struct VecI16
    {
        public int count;
        public short[] data;
    }

    public struct VecU16
    {
        public int count;
        public ushort[] data;
    }

    public struct VecI32
    {
        public int count;
        public int[] data;
    }

    public struct VecU32
    {
        public int count;
        public uint[] data;
    }
    #endregion
}
