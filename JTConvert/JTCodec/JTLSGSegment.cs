namespace JTConvert.JTCodec
{
    public class JTLSGSegment : JTSegment
    {
        public override bool Compressable => true;
        //TODO: ADG of scene graph
        public IJTGraphElement[] graphElements;

        internal static JTSegment LoadSegment(ref BinaryJTReader reader, int version)
        {
            Logger.Log($"Loading logical scene graph segment...");
            // Segment header
            JTLSGSegment lsgSeg = new();
            lsgSeg.segmentID = reader.ReadGUID();
            lsgSeg.segmentType = (JTSegmentType)reader.ReadInt32();
            lsgSeg.length = reader.ReadInt32();

            // Logical element header
            // LoadLEHeader(ref reader, lsgSeg.Compressable, ref lsgSeg.logicalElementHeader);

            // Object data
            GUID lastGuid = GUID.Empty;
            List<IJTGraphElement> graphElements = new();
            bool firstElement = true;
            while (lastGuid != JTObjectTypeIdentifiers.ObjectTypeIdentifiers[typeof(JTNullElement)])
            {
                // Read compressed logical element header
                JTLogicalElementHeader elemHeader = new();
                LoadLEHeader(ref reader, firstElement && lsgSeg.Compressable, ref elemHeader);
                // Only decompress the first element we encounter since it decompresses the whole segment.
                firstElement = false;

                if (!JTObjectTypeIdentifiers.ObjectTypeIdentifiersReverse.ContainsKey(elemHeader.objectTypeID))
                {
                    Logger.Log($"Unrecognised element encountered! Type id: {elemHeader.objectTypeID} position: {reader.BaseStream.Position} bytes.", Logger.VerbosityLevel.WARN);
                    continue;
                }
                Logger.Log($"Loading {JTObjectTypeIdentifiers.ObjectTypeIdentifiersReverse[elemHeader.objectTypeID]}...");
                IJTGraphElement element = (IJTGraphElement)Activator.CreateInstance(JTObjectTypeIdentifiers.ObjectTypeIdentifiersReverse[elemHeader.objectTypeID]);
                switch (element)
                {
                    case JTBaseNodeElement e:
                        e.header = elemHeader;
                        e.baseNodeData = LoadBaseNodeData(reader, version);

                        element = e;
                        break;

                    case JTGroupNodeElement e:
                        e.header = elemHeader;
                        e.groupNodeData = LoadGroupNodeData(reader, version);

                        element = e;
                        break;

                    case JTSwitchNodeElement e:
                        e.header = elemHeader;
                        e.groupNodeData = LoadGroupNodeData(reader, version);
                        e.versionNumber = reader.ReadVersion(version);
                        e.selectedChild = reader.ReadUInt32();

                        element = e;
                        break;

                    case JTPartitionNodeElement e:
                        e.header = elemHeader;
                        e.groupNodeData = LoadGroupNodeData(reader, version);
                        e.partitionFlags = (JTPartitionFlags)reader.ReadInt32();
                        e.fileName = reader.ReadMbString();
                        e.transformedBBox = reader.ReadBBoxF32();
                        e.area = reader.ReadSingle();
                        e.vertexCountRange = reader.ReadCountRange();
                        e.nodeCountRange = reader.ReadCountRange();
                        e.polygonCountRange = reader.ReadCountRange();
                        if ((e.partitionFlags & JTPartitionFlags.UntrasformedBBox) == JTPartitionFlags.UntrasformedBBox)
                            e.untransformedBBox = reader.ReadBBoxF32();

                        element = e;
                        break;

                    case JTMetaDataNodeElement e:
                        e.header = elemHeader;
                        e.metadata = LoadMetaData(reader, version);

                        element = e;
                        break;

                    case JTPartNodeElement e:
                        e.header = elemHeader;
                        e.metadata = LoadMetaData(reader, version);
                        e.version = reader.ReadVersion(version);
                        reader.Skip(4); // Empty field

                        element = e;
                        break;

                    case JTMaterialAttributeElement e:
                        e.header = elemHeader;
                        e.attributeData = LoadAttributeData(reader, version);
                        e.version = reader.ReadVersion(version);
                        e.dataFlags = (JTMaterialAttributeDataFlags)reader.ReadUInt16();
                        e.ambient = reader.ReadRGBA();
                        e.diffuseAlpha = reader.ReadRGBA();
                        e.specular = reader.ReadRGBA();
                        e.emission = reader.ReadRGBA();
                        e.shininess = reader.ReadSingle();
                        if(e.version >= 2)
                            e.reflectivity = reader.ReadSingle();
                        if(version >= 10)
                            e.bumpiness = reader.ReadSingle();
                        e.attributeDataFieldsV2 = LoadAttributeDataV2(reader, e.attributeData.version);

                        element = e;
                        break;

                    default:
                        Logger.Log($"Element decoder not implemented for {element.GetType().Name}!", Logger.VerbosityLevel.WARN);
                        Logger.Log($"Segment couldn't be decoded!", Logger.VerbosityLevel.ERROR);
                        lastGuid = JTObjectTypeIdentifiers.ObjectTypeIdentifiers[typeof(JTNullElement)];
                        break;
                }

                graphElements.Add(element);
            }

            lsgSeg.graphElements = graphElements.ToArray();

            return lsgSeg;
        }

        private static JTBaseAttributeDataFieldsV2 LoadAttributeDataV2(BinaryJTReader reader, int attributeVersion)
        {
            var ret = new JTBaseAttributeDataFieldsV2();
            if(attributeVersion >= 2)
                ret.palletteIndex = reader.ReadUInt32();

            return ret;
        }

        private static JTBaseAttributeData LoadAttributeData(BinaryJTReader reader, int version)
        {
            var ret = new JTBaseAttributeData();
            ret.version = reader.ReadVersion(version);
            ret.attributeDataV1.stateFlags = (JTAttributeStateFlags)reader.ReadByte();
            ret.attributeDataV1.fieldInhibitFlags = (JTFieldInhibitFlags)reader.ReadUInt32();
            if(version >= 10)
                ret.attributeDataV1.fieldFinalFlags = (JTFieldFinalFlags)reader.ReadUInt32();

            return ret;
        }

        private static JTMetaDataNodeData LoadMetaData(BinaryJTReader reader, int version) => new()
        {
            groupNodeData = LoadGroupNodeData(reader, version),
            versionNumber = reader.ReadVersion(version)
        };

        internal static JTGroupNodeData LoadGroupNodeData(BinaryJTReader reader, int version)
        {
            var ret = new JTGroupNodeData();
            ret.baseNodeData = LoadBaseNodeData(reader, version);
            ret.versionNumber = reader.ReadVersion(version);
            ret.childNodeObjectIDs = new int[reader.ReadInt32()];
            for(int i = 0; i < ret.childNodeObjectIDs.Length; i++)
                ret.childNodeObjectIDs[i] = reader.ReadInt32();

            return ret;
        }

        internal static JTBaseNodeData LoadBaseNodeData(BinaryJTReader reader, int version)
        {
            var baseNodeData = new JTBaseNodeData();
            baseNodeData.versionNumber = reader.ReadVersion(version);
            baseNodeData.nodeFlags = (JTNodeFlags)reader.ReadUInt32();
            baseNodeData.attributeObjectIDs = new int[reader.ReadInt32()];
            for(int i = 0; i < baseNodeData.attributeObjectIDs.Length; i++)
                baseNodeData.attributeObjectIDs[i] = reader.ReadInt32();

            return baseNodeData;
        }
    }
}
