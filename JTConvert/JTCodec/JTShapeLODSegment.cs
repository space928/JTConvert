using JTConvert.JTCodec.JTCompression;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JTConvert.JTCodec
{
    public class JTShapeLODSegment : JTSegment
    {
        public override bool Compressable => false;
        public IJTShapeLODElement shapeLODElement;

        internal static JTSegment LoadSegment(ref BinaryJTReader reader, int version)
        {
            // Segment header
            JTShapeLODSegment shapeSeg = new();
            shapeSeg.segmentID = reader.ReadGUID();
            shapeSeg.segmentType = (JTSegmentType)reader.ReadInt32();
            shapeSeg.length = reader.ReadInt32();

            // Read compressed logical element header
            JTLogicalElementHeader elemHeader = new();
            LoadLEHeader(ref reader, shapeSeg.Compressable, ref elemHeader);

            if (!JTObjectTypeIdentifiers.ObjectTypeIdentifiersReverse.ContainsKey(elemHeader.objectTypeID))
            {
                Logger.Log($"Unrecognised shape element encountered! Type id: {elemHeader.objectTypeID} position: {reader.BaseStream.Position} bytes.", Logger.VerbosityLevel.WARN);
                return null;
            }
            Logger.Log($"Loading {JTObjectTypeIdentifiers.ObjectTypeIdentifiersReverse[elemHeader.objectTypeID]}...", Logger.VerbosityLevel.DEBUG);
            IJTShapeLODElement element = (IJTShapeLODElement)Activator.CreateInstance(JTObjectTypeIdentifiers.ObjectTypeIdentifiersReverse[elemHeader.objectTypeID]);
            switch (element)
            {
                case JTNullShapeLODElement:
                    break;
                case JTTriStripSetShapeLODElement e:
                    e.header = elemHeader;
                    e.vertexShapeLODData = LoadVertexShapeLODData(reader, version, true);
                    e.version = reader.ReadVersion(version);

                    element = e;
                    break;

                case JTPolylineSetShapeLODElement e:
                    e.header = elemHeader;
                    e.vertexShapeLODData = LoadVertexShapeLODData(reader, version);
                    e.version = reader.ReadVersion(version);

                    element = e;
                    break;

                case JTPointSetShapeLODElement e:
                    e.header = elemHeader;
                    e.vertexShapeLODData = LoadVertexShapeLODData(reader, version);
                    e.version = reader.ReadVersion(version);

                    element = e;
                    break;

                case JTPolygonSetShapeLODElement e:
                    e.header = elemHeader;
                    e.vertexShapeLODData = LoadVertexShapeLODData(reader, version);
                    e.version = reader.ReadVersion(version);

                    element = e;
                    break;
                default:
                    Logger.Log($"Shape has invalid element type: {element.GetType()}!", Logger.VerbosityLevel.WARN);
                    break;
            }

            shapeSeg.shapeLODElement = element;

            return shapeSeg;
        }

        private static JTVertexShapeLODData LoadVertexShapeLODData(BinaryJTReader reader, int version, bool isTriStrip = false)
        {
            JTVertexShapeLODData ret = new();

            ret.baseData = new()
            {
                version = reader.ReadVersion(version),
            };
            ret.version = (sbyte) reader.ReadVersion(version);
            ret.vertexBindings = (JTVertexBindings) reader.ReadUInt64();
            if (isTriStrip)
                ret.meshDataTopologicallyCompressed = LoadTopoMeshTopologicallyCompressed(reader, version);
            else
                ret.meshDataCompressed = LoadTopoMeshCompressed(reader, version);

            return ret;
        }

        private static JTTopoMeshCompressedLODData LoadTopoMeshCompressed(BinaryJTReader reader, int version)
        {
            JTTopoMeshCompressedLODData ret = new();
            ret.meshLODData = new()
            {
                version = reader.ReadVersion(version),
                vertexRecordsObjectID = reader.ReadUInt32()
            };
            ret.version = reader.ReadVersion(version);

            // Load data
            JTTopoMeshCompressedRepData data = new();
            data.numberOfFaceGroupListIndices = reader.ReadUInt32();
            data.numberOfPrimitiveListIndices = reader.ReadUInt32();
            data.numberOfVertexListIndices = reader.ReadUInt32();
            //data.faceGroupListIndices = reader.ReadVecI32();
            //throw new NotImplementedException();

            ret.data = data;

            return ret;
        }

        private static JTTopoMeshTopologicallyCompressedLODData LoadTopoMeshTopologicallyCompressed(BinaryJTReader reader, int jtVersion)
        {
            JTTopoMeshTopologicallyCompressedLODData ret = new();
            ret.meshLODData = new()
            {
                version = reader.ReadVersion(jtVersion),
                vertexRecordsObjectID = reader.ReadUInt32()
            };
            ret.version = reader.ReadVersion(jtVersion);
            ret.data.faceDegreesA = JTCompressedDataPacketCodec.ReadIntCDP(reader, jtVersion, false);
            //ret.data.faceDegreesB = JTCompressedDataPacketCodec.ReadIntCDP(reader, jtVersion, false);
            //ret.data.faceDegreesC = JTCompressedDataPacketCodec.ReadIntCDP(reader, jtVersion, false);
            //ret.data.faceDegreesD = JTCompressedDataPacketCodec.ReadIntCDP(reader, jtVersion, false);
            //ret.data.faceDegreesE = JTCompressedDataPacketCodec.ReadIntCDP(reader, jtVersion, false);
            //ret.data.faceDegreesF = JTCompressedDataPacketCodec.ReadIntCDP(reader, jtVersion, false);
            //ret.data.faceDegreesG = JTCompressedDataPacketCodec.ReadIntCDP(reader, jtVersion, false);
            //ret.data.faceDegreesH = JTCompressedDataPacketCodec.ReadIntCDP(reader, jtVersion, false);
            return ret;
        }
    }
}
