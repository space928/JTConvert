using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JTConvert.JTCodec
{
    public class JTShapeLODSegment : JTSegment
    {
        public override bool Compressable => true;
        public IJTShapeLODElement shapeLODElement;

        internal static JTSegment LoadSegment(ref BinaryJTReader reader, int version)
        {
            Logger.Log($"Loading shape segment...");
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
            IJTGraphElement element = (IJTGraphElement)Activator.CreateInstance(JTObjectTypeIdentifiers.ObjectTypeIdentifiersReverse[elemHeader.objectTypeID]);
            switch (element)
            {
                case JTTriStripSetShapeNodeElement
            }

            return shapeSeg;
        }

        private static JTShapeLODElement LoadShapeLODElement(BinaryJTReader reader, int version)
        {
            throw new NotImplementedException();
        }
    }
}
